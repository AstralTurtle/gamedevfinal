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
    [Export]
    PackedScene lose;

    Node lobby;

    Dictionary players;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
         lobby = GetNode("/root/Lobby");
        players = lobby.Get("players").As<Dictionary>();
        // CallDeferred("spawnAllPlayers");
        spawnAllPlayers();

      var start = GD.Load<PackedScene>("res://Services/Rooms/Start.tscn");
      var obby = GD.Load<PackedScene>("res://Services/Rooms/Obby.tscn");

      for (int i = 0; i < 5; i++) {
        if (GD.Randf() > 0.5) {
          var instance = (Node2D) start.Instantiate();
          instance.Position = new Vector2(24 * 32 * i, 128);
          AddChild(instance);
        } else {
          var instance = (Node2D) obby.Instantiate();
          instance.Position = new Vector2(24 * 32 * i, 128);
          AddChild(instance);
        }
      }
    }

    // Called every frame. 'delta' is the elaps.ed time since the previous frame.
    public override void _Process(double delta)
    {
        if (GetTree().GetNodesInGroup("players").Count == 0)
        {
            GD.Print("losing?");
            Rpc("Lose");
        }
        
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void Lose(){
        // GD.Print("???");
        GetTree().Root.AddChild(lose.Instantiate());
        // lobby.Call("leaveLobby");
        // lobby.QueueFree();
        
    }

    public void respawnPlayer(int pc, int pid, Vector2 pos)
    {
        if (!IsMultiplayerAuthority())
            return;
        Rpc("RPCrespawnPlayer", pc, pid, pos);
    }

    public void Unload(){
        Node[] children = GetChildren().ToArray();
        GD.Print("unloading " + children.Length + " children");
        for (int i = 0; i < children.Length; i++)
        {
            GD.Print("unloading " + children[i].Name);
            children[i].QueueFree();
        }
        QueueFree();
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void RPCrespawnPlayer(int pclass, int pnid, Vector2 pos)
    {
        Player player = playerPrefabs[pclass].Instantiate<Player>();
        player.Name = "Player" + pnid;
        player.pclass = pclass;

        AddChild(player);
        player.AddToGroup("players");
        player.triggerMultiplayerAuthority(pnid);
        player.Position = pos;
    }

    public void spawnAllPlayers()
    {
        GD.Print(playerPrefabs);
        Variant[] playerID = players.Keys.ToArray();
        // int[] playerIDs = (int[])playerID; // Cast playerID to int[]
        for (int i = players.Count - 1; i > -1; i--)
        {
            // GD.Print(i);
            int currentPlayerID = playerID[i].As<int>();
            // cursed gdscript to c# conversion
            // GD.Print(playerPrefabs[i]);
            Player player = playerPrefabs[players[currentPlayerID].As<int>()].Instantiate<Player>();

            // player.SetMultiplayerAuthority(currentPlayerID);
            // Rpc("rpcSetAuth", player, currentPlayerID);
            player.ZIndex = 10;
            player.Name = "Player" + currentPlayerID;
            player.pclass = players[currentPlayerID].As<int>();

            AddChild(player);
            player.AddToGroup("players");
            player.triggerMultiplayerAuthority(currentPlayerID);
            player.Position = new Vector2(100 + i * -100, 0);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void rpcSetAuth(Player player, int id)
    {
        player.SetMultiplayerAuthority(id);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void RpcGiveCoins(int amount)
    {
        CurrencyManager localCM = GetTree().Root.GetNode<CurrencyManager>("CurrencyManager");
        localCM.AddCoins(amount);
    }

    public void giveCoins(int amount)
    {
        Rpc("RpcGiveCoins", amount);
    }

    public void giveGems(int amount)
    {
        Rpc("RpcGiveGems", amount);
    }

    public void RpcGiveGems(int amount)
    {
        CurrencyManager localCM = GetTree().Root.GetNode<CurrencyManager>("CurrencyManager");
        localCM.AddGems(amount);
    }
}
