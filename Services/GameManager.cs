using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Resolvers;

public enum RoomType {
    Default,
    StairUp,
    StairDown,
    Boss,
    Shop
}


public partial class GameManager : Node2D
{
  
  private static Stack<RoomType> rooms = new();
	[Export]
	PackedScene[] playerPrefabs = new PackedScene[4];

	Dictionary players;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var lobby = GetNode("/root/Lobby");
		players = lobby.Get("players").As<Dictionary>();
		spawnAllPlayers();

    var start = GD.Load<PackedScene>("res://Services/Rooms/Start.tscn");
    var instance = start.Instantiate();

    AddChild(instance);
	}

	// Called every frame. 'delta' is the elaps.ed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void spawnAllPlayers(){
		GD.Print(playerPrefabs);
		Variant[] playerID = players.Keys.ToArray();
		// int[] playerIDs = (int[])playerID; // Cast playerID to int[]
		for(int i = players.Count - 1 ; i > -1; i--){
			GD.Print(i);
			int currentPlayerID = playerID[i].As<int>();
			// cursed gdscript to c# conversion
			GD.Print(playerPrefabs[i]);
			Player player = playerPrefabs[players[currentPlayerID].As<int>()].Instantiate<Player>();
			
			// player.SetMultiplayerAuthority(currentPlayerID);
			// Rpc("rpcSetAuth", player, currentPlayerID);
			player.Name = "Player" + currentPlayerID;
			AddChild(player);
			player.AddToGroup("players");
			player.triggerMultiplayerAuthority(currentPlayerID);
			// player.Position = new Vector2(100 + i * -100, 100);
		}
	}

  public void generate(int rooms) {
    int floor = 0;
    for (int i = 0; i < rooms - 1; i++) {
      float random = new RandomNumberGenerator().Randf();

      if (random <= 0.025) {
        addRoom(RoomType.StairUp, floor);
        floor++;
        continue;
      }

      if (random <= 0.05) {
        addRoom(RoomType.StairDown, floor);
        floor--;
        continue;
      }
              
      if (random <= 0.055) {
        addRoom(RoomType.Shop, floor);
      }

      addRoom(RoomType.Default, floor);
    }

    addRoom(RoomType.Boss, floor);
  }

  public static void addRoom(RoomType type, int floor) {
    rooms.Push(type);
    
  }

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true )]
	public void rpcSetAuth(Player player,int id){
		player.SetMultiplayerAuthority(id);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true )]
	public void RpcGiveCoins(int amount){
		CurrencyManager localCM = GetTree().Root.GetNode<CurrencyManager>("CurrencyManager");
		localCM.AddCoins(amount);
	}

	public void giveCoins(int amount){
		Rpc("RpcGiveCoins", amount);
	}

	public void giveGems(int amount){
		Rpc("RpcGiveGems", amount);
	}

	public void RpcGiveGems(int amount){
		CurrencyManager localCM = GetTree().Root.GetNode<CurrencyManager>("CurrencyManager");
		localCM.AddGems(amount);
	}

}
