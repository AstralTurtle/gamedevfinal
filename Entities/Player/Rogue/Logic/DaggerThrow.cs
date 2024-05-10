using Godot;
using System;

public partial class DaggerThrow : RigidBody2D
{
	[Export]
	float speed = 600;
	[Export]
	float damage = 20;
	float rotSpeed = 10;

	AnimatedSprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	
	public void throwObj(Vector2 dir){
		ApplyImpulse(dir * speed);
	}

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
	}

    public override void _Process(double delta)
    {
		sprite.Rotate(rotSpeed * (float)delta);
		rotSpeed++;
		damage -= (float)delta;
        base._Process(delta);
    }

	public void OnCollisionEntered(Node body){
		if (body is Player) return;
		if(body is Enemy){
			GD.Print("Hit Enemy");
		}
		QueueFree();
	}
	
}
