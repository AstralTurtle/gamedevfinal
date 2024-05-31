using Godot;
using System;

public partial class BowA1 : Node2D
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
        ArrowRainSpawner ArrowRain = stormScene.Instantiate<ArrowRainSpawner>();
        StatManager sm = GetParent().GetParent().GetNode<StatManager>("StatManager");
        ArrowRain.setDamage(sm.getDamage());
        
        GetTree().Root.GetNode<Node2D>("Game").AddChild(ArrowRain, true);;
       

        ArrowRain.GlobalPosition = mpos;
        ArrowRain.setBasePos(mpos);
        GD.Print("mpos: " + mpos);
        GD.Print("storm pos: " + ArrowRain.GlobalPosition);
        GD.Print("player pos: " + GetParent<Node2D>().GetParent<Node2D>().GlobalPosition);
        ArrowRain.setBasePos(mpos);
        

         ArrowRain.SpawnArrows();   
        GD.Print("new pos: " + ArrowRain.GlobalPosition);


    }
}
