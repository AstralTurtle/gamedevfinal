using Godot;
using System;

public partial class ThrowableProjectile : RigidBody2D
{
	[Export]
	float rotSpeed = (float)Math.PI * 10;
	[Export]
	float speed = 300;
	float damage = 0;

	Sprite2D sprite2D;

	public void SetDamage(float damage){
		this.damage = damage;
	}

	public void throwObj(Vector2 dir){
		ApplyImpulse(dir * speed);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		BodyEntered += OnCollisionEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		sprite2D.Rotation += rotSpeed * (float)delta;

	}

    public override void _PhysicsProcess(double delta)
    {
		//
        base._PhysicsProcess(delta);
    }

    public void OnCollisionEntered(Node body){
		if (body is ThrowableProjectile){
			return;
		}
		if (body is Player){
			Player player = (Player)body;
			player.triggerDamage(damage);
		}
		QueueFree();
	}

}
