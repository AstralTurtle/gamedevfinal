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

	public void inheritVelocity(Vector2 vel){
		if (rb == null) rb = GetNode<RigidBody2D>("RigidBody2D");
		rb.LinearVelocity = vel;
	}

	public void OnCollisionEntered(Node body){
		GD.Print();
		GD.Print("Positions: " +rb.Position + "vs" + player.Position + "vs" + Position);
				
		
		Node2D colShape = GetNode<Node2D>("RigidBody2D/CollisionShape2D");
		GD.Print("colShape: " + colShape.GetGlobalTransformWithCanvas().Origin);
		player.Position = colShape.GlobalPosition;
		GD.Print("Real Movement: " + player.Position);

		QueueFree();
		// GD.Print(GetParent().Name);
	}
}
