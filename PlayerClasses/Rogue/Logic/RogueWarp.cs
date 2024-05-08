using Godot;
using System;

public partial class RogueWarp : Node2D
{
	Node2D player;
	RigidBody2D rb;

	[Export]
	float speed = 500;

	const float rotSpeed = 3.14f * 5;
	public void setPlayer(Node2D p){
		player = p;
	}

	Sprite2D sprite;



	public override void _Ready()
	{
		rb = GetNode<RigidBody2D>("RigidBody2D");
		sprite = GetNode<Sprite2D>("RigidBody2D/Sprite2D");
		// rb.ApplyImpulse(new Vector2)e
	}

	// Debug
	public override void _Process(double delta){
		
		sprite.Rotate(rotSpeed * (float)delta);
		// GD.Print(rb.LinearVelocity);
		// GD.Print()
		// GD.Print(rb.Position + "vs" + Position);	
		// GD.Print(rb.GetCollidingBodies()); 		 		 
	}


    public override void _PhysicsProcess(double delta)
    {
		
        // base._PhysicsProcess(delta);
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

		
		Node2D colShape = GetNode<Node2D>("RigidBody2D/CollisionShape2D");
		player.Position = colShape.GlobalPosition;
		GD.Print("Real Movement: " + player.Position);

		QueueFree();
		// GD.Print(GetParent().Name);
	}
}
