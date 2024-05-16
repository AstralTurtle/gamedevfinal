using Godot;
using System;


public partial class DaggerStab : Node2D
{
	[Export]
	float speed = 500;

	[Export]
	float distance = 10;

	Vector2 location;

	public void stab(Vector2 dir){
		location = Position  + (dir * distance);

		float angle = Position.AngleTo(location);
		Rotation = -angle;

		GD.Print(location);
		//rotate towards location
		


	}

	public override void _Process(double delta){
		Position = Position.MoveToward(location,(float)delta * speed);

		// GD.Print(Position + " vs " + location);
		if (Position == location){
			QueueFree();
		}
	}


}
