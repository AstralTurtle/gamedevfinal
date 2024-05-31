using Godot;
using System;

public partial class MageA1 : Node2D
{
    [Export]
    PackedScene stormScene;

    public void OnActivated()
    {
        GD.Print("Ability Triggered");
        Vector2 mpos = GetGlobalMousePosition();
        Rpc("OnActivatedRPC", mpos);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void OnActivatedRPC(Vector2 mpos)
    {
        ChaosStorm storm = stormScene.Instantiate<ChaosStorm>();
        StatManager sm = GetParent().GetParent().GetNode<StatManager>("StatManager");
        storm.setDamage(sm.getDamage());
        GetTree().Root.GetNode<Node2D>("Game").AddChild(storm, true);

        storm.GlobalPosition = mpos;
        GD.Print("mpos: " + mpos);
        GD.Print("storm pos: " + storm.GlobalPosition);
        GD.Print("player pos: " + GetParent<Node2D>().GetParent<Node2D>().GlobalPosition);
    }
}
