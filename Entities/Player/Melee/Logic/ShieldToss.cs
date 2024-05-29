using Godot;
using System;

public partial class ShieldToss : RigidBody2D
{
	[Export]
	float speed = 600;

	float damage = 10;

	float rotSpeed = 10;

	float timeout = 600;
	float timer = 0;
	bool isReturning = false;

	Node2D player;

	Sprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	
	public void throwObj(Vector2 dir){
		ApplyImpulse(dir * speed);
	}

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		BodyEntered += OnCollisionEntered;
	}

	public void setPlayer(Node2D p){
		player = p;
	}

	public void setDamage(float dmg){
		damage = dmg;
	}

    public override void _Process(double delta)
    {
		timer++;
		if (timer > timeout){
			QueueFree();
		}
		GD.Print(timeout - timer);
		sprite.Rotate(rotSpeed * (float)delta);
        base._Process(delta);

		if (isReturning) {
			Position = Position.MoveToward(player.Position, speed * (float)delta);
		}
		if (Position == player.Position){
			QueueFree();
		}

    }

	public void OnCollisionEntered(Node body){
		if (body == player) QueueFree();

		if (body is Player) return;
		if(body is Enemy){
			GD.Print("Hit Enemy");
		}
		if (!isReturning) {
			timer = 0;
			// GravityScale = 0;
			LinearVelocity = Vector2.Zero;
			CallDeferred("disablePhysics");
			isReturning = true;
		}
	}

	public void disablePhysics(){
FreezeMode = FreezeModeEnum.Static;
Freeze = true;
	}
	
}

