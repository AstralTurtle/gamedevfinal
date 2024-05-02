using Godot;
using System;

public partial class RogueWarp : Node2D
{
	Node2D player;
	RigidBody2D rb;

	[Export]
	float speed = 500;

	public void setPlayer(Node2D p){
		player = p;
	}


	public override void _Ready()
	{
		rb = GetNode<RigidBody2D>("RigidBody2D");
		// rb.ApplyImpulse(new Vector2)e
	}

	// Debug
	public override void _Process(double delta){
		// GD.Print(rb.LinearVelocity);
		// GD.Print()
		// GD.Print(rb.Position + "vs" + Position);	
		// GD.Print(rb.GetCollidingBodies()); 		 		 
	}

	public void throwWarp(Vector2 dir){
		
		rb.ApplyImpulse(dir * speed);
		// rb.Freeze = false;
	}

	public void OnCollisionEntered(Node body){
		player.Position = rb.Position;
		QueueFree();
		// GD.Print(GetParent().Name);
	}
}
