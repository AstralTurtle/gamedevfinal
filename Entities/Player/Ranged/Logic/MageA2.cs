using Godot;
using System;

public partial class MageA2 : Node2D
{
    [Export]
    PackedScene bombScene;

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
        Node2D player = GetParent<Node2D>().GetParent<Node2D>();
        GD.Print(player.Name);
        ChaosBomb bomb = bombScene.Instantiate<ChaosBomb>();
        StatManager sm = GetParent().GetParent().GetNode<StatManager>("StatManager");

        bomb.Position = player.Position + new Vector2(30, 30) * dir;

        bomb.RotationDegrees = 0;

        bomb.setDamage(sm.getDamage());

        // Rpc("addToTree",warp);
        GetTree().Root.GetNode<Node2D>("Game").AddChild(bomb, true);

        // GD.Print(dir);



        bomb.throwObj(dir);
    }
}
