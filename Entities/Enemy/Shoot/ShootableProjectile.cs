using Godot;
using System;

public partial class ShootableProjectile : Area2D
{

	[Export]
	float speed = 300;

	float damage = 0;

	Vector2 dir = Vector2.Zero;

	public void SetDamage(float dmg)
	{
		damage = dmg;
	}


	public void SetDirection(Vector2 direction)
	{
		dir = direction * speed;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnCollisionEnter;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RotationDegrees = dir.Angle();
		Position += dir * (float)delta;

	}

	public void OnCollisionEnter(Node body){
		if(body is Player){
			Player player = (Player)body;
			player.triggerDamage(damage);
			QueueFree();
		}
	}
}
