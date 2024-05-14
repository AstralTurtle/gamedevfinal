using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GameManager : Node2D
{
	[Export]
	PackedScene[] playerPrefabs = new PackedScene[4];

	Dictionary players;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var lobby = GetNode("/root/Lobby");
		players = lobby.Get("players").As<Dictionary>();
		spawnAllPlayers();
	}

	// Called every frame. 'delta' is the elaps.ed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void spawnAllPlayers(){
		GD.Print(playerPrefabs);
		Variant[] playerID = players.Keys.ToArray();
		// int[] playerIDs = (int[])playerID; // Cast playerID to int[]
		for(int i = 0; i < players.Count; i++){
			int currentPlayerID = playerID[i].As<int>();
			// cursed gdscript to c# conversion
			GD.Print(playerPrefabs[i]);
			Player player = playerPrefabs[players[currentPlayerID].As<int>()].Instantiate<Player>();
			// player.triggerMultiplayerAuthority(currentPlayerID);
			player.SetMultiplayerAuthority(currentPlayerID);
			Rpc("rpcSetAuth", player, currentPlayerID);
			player.Name = "Player" + currentPlayerID;
			AddChild(player);
		
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true )]
	public void rpcSetAuth(Player player,int id){
		player.SetMultiplayerAuthority(id);
	}

}
