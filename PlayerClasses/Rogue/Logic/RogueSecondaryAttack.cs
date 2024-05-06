using Godot;
using System;

public partial class RogueSecondaryAttack : Node2D
{
	[Export]
	PackedScene secondaryAttack;
	public  override void _Ready() {
		secondaryAttack = GD.Load<PackedScene>("res://PlayerClasses/Rogue/Scenes/DaggerThrow.tscn");
	}

	public void OnActivated() {
		DaggerThrow dagger = secondaryAttack.Instantiate<DaggerThrow>();
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();


		dagger.Position = player.Position;
		

		dagger.RotationDegrees = 0;

		// copy player velocity
		

			 	
		
		GetTree().Root.AddChild(dagger);

		Vector2 mpos = GetViewport().GetMousePosition();
		GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();

		dagger.Position += new Vector2(25, 25) * dir;

		dagger.throwObj(dir);


	}

	public void changeWeapon(PackedScene newWeapon){
		secondaryAttack = newWeapon;
	}
}
