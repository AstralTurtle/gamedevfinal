using Godot;
using System;

public partial class BowA2 : Node2D
{
    [Export]
    PackedScene stormScene;

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
        FocusShot fs = stormScene.Instantiate<FocusShot>();
        StatManager sm = GetParent().GetParent().GetNode<StatManager>("StatManager");
        fs.setDamage(sm.getDamage());
        fs.setDir(dir);
        fs.Position = player.Position + new Vector2(25, 25) * dir;
        GetTree().Root.GetNode<Node2D>("Game").AddChild(fs, true);

       
    }
}
