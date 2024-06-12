using Godot;
using System;

public partial class RoguePrimaryAttack : Node2D
{
    // Called when the node enters the scene tree for the first time.
    [Export]
    PackedScene primaryAttack;

    public void OnActivated()
    {
        Vector2 mpos = GetViewport().GetMousePosition();
        //GD.Print(mpos);

        Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
        dir = dir.Normalized();

        Rpc("OnActivatedRPC", dir);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void OnActivatedRPC(Vector2 dir)
    {
        DaggerStab stab = primaryAttack.Instantiate<DaggerStab>();
        Node2D player = GetParent<Node2D>().GetParent<Node2D>();
        StatManager sm = player.GetNode<StatManager>("StatManager");
        stab.setDamage(sm.getDamage());

        stab.TreeExited += GetParent<AbilityInput>().resetPrimaryDebounce;

        stab.Position = player.Position;

        GetTree().Root.GetNode<Node2D>("Game").AddChild(stab, true);

        stab.stab(dir);
    }
}
