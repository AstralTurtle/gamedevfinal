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
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();
		GD.Print(player.Name);
		RogueWarp warp = WarpProjectile.Instantiate<RogueWarp>();
		warp.setPlayer(player);
		
		warp.Position = player.Position;
		

		warp.RotationDegrees = 0;

		// copy player velocity
		warp.inheritVelocity((player as CharacterBody2D).Velocity);

			 	
		
		Rpc("addToTree",warp);	
		
		Vector2 mpos = GetViewport().GetMousePosition();
		//GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();
		// GD.Print(dir);

		warp.Position += new Vector2(25, 25) * dir;

		warp.throwWarp(dir);

	}

	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true)]
	public void addToTree(RogueWarp proj){
		GetTree().Root.GetNode<Node2D>("Game").AddChild(proj);
	}
}
