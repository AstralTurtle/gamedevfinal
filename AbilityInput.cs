using Godot;
using System;

public partial class AbilityInput : Node2D
{
	[Export] // Ability 1 CD
	float a1cd = 1;
	// Ability 1 CD Timer
	float a1cdt = 0;

	[Export] // Ability 2 CD
	float a2cd = 1;
	// Ability 2 CD Timer
	float a2cdt = 0;

	[Export] // Ability 3 CD
	float a3cd = 1;
	// Ability 3 CD Timer
	float a3cdt = 0;

	// Signals
	[Signal]
	public delegate void Ability1EventHandler();
	[Signal]
	public delegate void Ability2EventHandler();
	[Signal]
	public delegate void Ability3EventHandler();

	public override void _Process(double delta)
	{
		a1cdt += (float)delta;
		a2cdt += (float)delta;
		a3cdt += (float)delta;

		// base._Process(delta);
	}


	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("ability_1"))
		{
			if (a1cdt >= a1cd)
			{
				EmitSignal(SignalName.Ability1);
				a1cdt = 0;
				GD.Print("Ability 1 used");
			}
			else GD.Print("Ability 1 is on cooldown");
		}
		else if (@event.IsActionReleased("ability_2"))
		{
			if (a2cdt >= a2cd)
			{
				EmitSignal(SignalName.Ability2);
				a2cdt = 0;
				GD.Print("Ability 2 used");
			}
			else GD.Print("Ability 2 is on cooldown");
		}
		else if (@event.IsActionReleased("ability_3"))
		{
			if (a3cdt >= a3cd)
			{
				EmitSignal(SignalName.Ability3);
				a3cdt = 0;
				GD.Print("Ability 3 used");
			}
			else GD.Print("Ability 3 is on cooldown");

		}
	}


}
