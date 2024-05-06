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

	bool primaryDebunce = true;
	bool secondaryDebunce = true;

	// Signals
	[Signal]
	public delegate void Ability1EventHandler();
	[Signal]
	public delegate void Ability2EventHandler();
	[Signal]
	public delegate void Ability3EventHandler();
	[Signal]
	public delegate void PrimaryAttackEventHandler();
	[Signal]
	public delegate void SecondaryAttackEventHandler();

	[Signal]
	public delegate void A1CDChangedEventHandler(float val);
	[Signal]
	public delegate void A2CDChangedEventHandler(float val);
	[Signal]
	public delegate void A3CDChangedEventHandler(float val);
	[Signal]
	public delegate void setA1CDEventHandler(float cd);
	[Signal]
	public delegate void setA2CDEventHandler(float cd);
	[Signal]
	public delegate void setA3CDEventHandler(float cd);


	public override void _Process(double delta)
	{
		a1cdt += (float)delta;
		a2cdt += (float)delta;
		a3cdt += (float)delta;
		EmitSignal(SignalName.A1CDChanged, a1cdt);
		EmitSignal(SignalName.A2CDChanged, a2cdt);
		EmitSignal(SignalName.A3CDChanged, a3cdt);

		// base._Process(delta);
	}

    public override void _Ready()
    {
		EmitSignal(SignalName.setA1CD, a1cd);
		EmitSignal(SignalName.setA2CD, a2cd);
		EmitSignal(SignalName.setA3CD, a3cd);

        base._Ready();
    }


    public override void _Input(InputEvent @event)
	{
		if (!IsMultiplayerAuthority()) return;
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
		else if (@event.IsActionReleased("primary_attack"))
		{
			EmitSignal(SignalName.PrimaryAttack);
			GD.Print("Primary Attack");
		}
		else if (@event.IsActionReleased("secondary_attack") )
		{
			EmitSignal(SignalName.SecondaryAttack);
			GD.Print("Secondary Attack");
		}
	}


}