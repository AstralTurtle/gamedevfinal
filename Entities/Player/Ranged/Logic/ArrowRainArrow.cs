using Godot;
using System;

public partial class ArrowRainArrow : RigidBody2D
{
	float damage = 0;

	public void setDamage(float dmg)
	{
		damage = dmg;
	}

	public override void _Ready()
	{
		BodyEntered += OnCollisionEntered;
	}

	public void OnCollisionEntered(Node body)
	{
		if (body is ArrowRainArrow) return;

		if (body is Enemy)
		{
			(body as Enemy).triggerDamage(damage);
		}
		QueueFree();
	}


}
