using Godot;
using System;

public partial class RogueSecondaryAttack : Node2D
{
	[Export]
	PackedScene secondaryAttack;
	public  override void _Ready() {
		secondaryAttack =  GD.Load<PackedScene>("res://Entities/Player/Rogue/Scenes/DaggerThrow.tscn");
	}

	public void OnActivated() {
		DaggerThrow dagger = secondaryAttack.Instantiate<DaggerThrow>();
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();


		dagger.Position = player.Position;
		

		dagger.RotationDegrees = 0;

		// copy player velocity
		

			 	
		// dagger
		// GetTree().Root.GetNode<Node2D>("Game").AddChild(dagger);
		Rpc("addToTree",dagger);

		Vector2 mpos = GetViewport().GetMousePosition();

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();

		dagger.Position += new Vector2(25, 25) * dir;

		dagger.throwObj(dir);
	

	}

	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true)]
	public void addToTree(DaggerThrow proj){
		GetTree().Root.GetNode<Node2D>("Game").AddChild(proj, true);
	}


}
