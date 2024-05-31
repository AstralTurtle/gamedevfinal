using Godot;
using System;

public partial class FocusShot : Area2D
{
	float damage = 0;

	Vector2 dir = new Vector2(0, 0);

	public void setDir(Vector2 dr)
	{
		Rotation = dr.Angle();
		dir = dr;
	}

	[Export]
	public float LifeTime = 10f;
	[Export]
	public float speed = 100f;

	public float timer = 0f;

	public void setDamage(float dmg)
	{
		damage = dmg * 1.75f;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnCollisionEntered;
	}

    public override void _Process(double delta)
    {
		Position = Position + dir * speed * (float)delta;

		timer += (float)delta;
		if (timer >= LifeTime)
		{
			QueueFree();
		}
    }

    public void OnCollisionEntered(Node body)
	{
		if (body is FocusShot) return;
		if (body is Player) return;

		if (body is Enemy)
		{
			damage = damage / 3;
			(body as Enemy).triggerDamage(damage);
		}
		else {
			QueueFree();
		}
	}

	

}
