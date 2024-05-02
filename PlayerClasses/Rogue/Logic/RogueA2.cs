using Godot;
using System;

public partial class RogueA2 : Node2D
{
	PackedScene WarpProjectile;


	public override void _Ready()
	{
		WarpProjectile = GD.Load<PackedScene>("res://PlayerClasses/Rogue/Scenes/RogueWarp.tscn");
		GD.Print("RogueA2 ready");
		GD.Print(WarpProjectile);
	}

	

	public void OnActivated() {
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();
		GD.Print(player.Name);
		RogueWarp warp = WarpProjectile.Instantiate<RogueWarp>();
		warp.setPlayer(player);
		
		warp.Position = player.Position;
		warp.Position += new Vector2(0, -50);

		warp.RotationDegrees = 0;

		// copy player velocity
		

			 	
		
		GetTree().Root.AddChild(warp);
		
		Vector2 mpos = GetViewport().GetMousePosition();
		GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();
		GD.Print(dir);
		warp.throwWarp(dir);

	}
}
