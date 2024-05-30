using Godot;
using System;
using System.Diagnostics;

public partial class Laser : RayCast2D
{
	bool isFiring = false;
	StatManager sm;

	Line2D line;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sm = GetParent().GetParent().GetNode<StatManager>("StatManager");
		line = GetNode<Line2D>("Line2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GD.Print("f: " + isFiring + "c: " +IsColliding());
		if (isFiring)
		{

			ForceRaycastUpdate();
			if (IsColliding())
			{
				GD.Print(GetCollisionPoint);
				line.SetPointPosition(1,GetCollisionPoint());
				GD.Print(line.Points[1]);
				if (GetCollider() is Enemy)
				{
					(GetCollider() as Enemy).triggerDamage(sm.getDamage() * (float)delta);
				}

			}

		}
	}

	void StartLaserTween()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(line, "width", 10, 1);

	}
	void EndLaserTween()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(line, "width", 0, 0.5);
	}

	void triggerLaser(bool laser)
	{
		GD.Print(laser);
		Rpc("triggerLaserRPC", laser);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	void triggerLaserRPC(bool laser)
	{
		GD.Print("laser:" + laser);
		isFiring = laser;
		// Enabled = isFiring;
		if (isFiring)
		{
			StartLaserTween();
		}
		else
		{
			EndLaserTween();
		}
	}
}
