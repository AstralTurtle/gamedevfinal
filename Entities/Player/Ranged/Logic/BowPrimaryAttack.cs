using Godot;
using System;

public partial class BowPrimaryAttack : Node2D
{
    [Export]
    PackedScene primaryAttack;

    StatManager sm;

    public override void _Ready()
    {
        sm = GetParent().GetParent().GetNode<StatManager>("StatManager");
    }

    public void OnActivated(float chargeTime)
    {
        Vector2 mpos = GetViewport().GetMousePosition();
        //GD.Print(mpos);

        Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;

        dir = dir.Normalized();

        Rpc("OnActivatedRPC", dir, chargeTime);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void OnActivatedRPC(Vector2 dir, float chargeTime)
    {
        Node2D player = GetParent<Node2D>().GetParent<Node2D>();
        Arrow arrow = primaryAttack.Instantiate<Arrow>();
        arrow.Position = player.Position + new Vector2(25, 25) * dir;
        GD.Print(arrow.Position + " vs " + (player.Position + new Vector2(25, 25) * dir));
        arrow.SetChargeTime(chargeTime);
        arrow.setDamage(sm.getDamage());
        GetTree().Root.GetNode<Node2D>("Game").AddChild(arrow, true);
        arrow.throwObj(dir);
    }

    public void changeWeapon(PackedScene newWeapon)
    {
        primaryAttack = newWeapon;
    }
}
