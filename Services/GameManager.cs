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
        Node[] children = GetChildren().ToArray();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] is StatManager)
            {
                GD.Print("StatManager Found: " + children[i].Name);
            }
        }
    }

    public void respawnPlayer(int pc, int pid, Vector2 pos)
    {
        if (!IsMultiplayerAuthority())
            return;
        Rpc("RPCrespawnPlayer", pc, pid, pos);
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
            player.Name = "Player" + currentPlayerID;
            player.pclass = players[currentPlayerID].As<int>();
            Rpc("AddPlayerToTree", player, currentPlayerID);
            // player.Position = new Vector2(100 + i * -100, 100);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void AddPlayerToTree(Player player, int id)
    {
        AddChild(player);
        player.AddToGroup("players");
        player.triggerMultiplayerAuthority(id);
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
