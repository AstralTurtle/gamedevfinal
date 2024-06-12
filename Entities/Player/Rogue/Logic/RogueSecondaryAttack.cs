using Godot;
using System;

public partial class RogueSecondaryAttack : Node2D
{
    [Export]
    PackedScene secondaryAttack;

    public override void _Ready()
    {
        secondaryAttack = GD.Load<PackedScene>(
            "res://Entities/Player/Rogue/Scenes/DaggerThrow.tscn"
        );
    }

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
        DaggerThrow dagger = secondaryAttack.Instantiate<DaggerThrow>();
        Node2D player = GetParent<Node2D>().GetParent<Node2D>();
        StatManager sm = player.GetNode<StatManager>("StatManager");
        dagger.setDamage(sm.getDamage());

        dagger.Position = player.Position;

        dagger.RotationDegrees = 0;

        dagger.TreeExited += GetParent<AbilityInput>().resetSecondaryDebounce;

        // copy player velocity



        // dagger
        // GetTree().Root.GetNode<Node2D>("Game").AddChild(dagger);
        GetTree().Root.GetNode<Node2D>("Game").AddChild(dagger, true);

        dagger.Position += new Vector2(25, 25) * dir;

        dagger.throwObj(dir);
    }
}
