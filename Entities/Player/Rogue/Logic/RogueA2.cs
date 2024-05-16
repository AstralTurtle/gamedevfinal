using Godot;
using System;

public partial class RogueA2 : Node2D
{
	PackedScene WarpProjectile;


	public override void _Ready()
	{
		WarpProjectile = GD.Load<PackedScene>("res://Entities/Player/Rogue/Scenes/RogueWarp.tscn");
		GD.Print("RogueA2 ready");
		GD.Print(WarpProjectile);
	}

	
	public void OnActivated() {
		Vector2 mpos = GetViewport().GetMousePosition();
		//GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();


		Rpc("OnActivatedRPC", dir);
		
	}	


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC(Vector2 dir) {
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();
		GD.Print(player.Name);
		RogueWarp warp = WarpProjectile.Instantiate<RogueWarp>();
		warp.setPlayer(player);
		
		warp.Position = player.Position;
		

		warp.RotationDegrees = 0;

		// copy player velocity
		// this desyncs the projectile in multiplayer for some reason so :shrug:
		// warp.inheritVelocity((player as CharacterBody2D).Velocity);

			 	
		
		// Rpc("addToTree",warp);	
		GetTree().Root.GetNode<Node2D>("Game").AddChild(warp ,true);

		
		
		// GD.Print(dir);

		warp.Position += new Vector2(25, 25) * dir;

		warp.throwWarp(dir);

	}
	
}
