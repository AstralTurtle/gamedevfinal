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

	[Export]

	int maxPrimary = 1;
	int primary = 0;
	[Export]
	int maxSecondary = 1;
	int secondary = 0;
	



	// Signals
	[Signal]
	public delegate void Ability1EventHandler();
	[Signal]
	public delegate void Ability2EventHandler();

	[Signal]
	public delegate void PrimaryAttackEventHandler();
	[Signal]
	public delegate void SecondaryAttackEventHandler();

	[Signal]
	public delegate void updateCooldownsEventHandler(float[] cooldowns);

	[Signal]
	public delegate void setCooldownsEventHandler(float[] cooldown);
	[Signal]
	public delegate void setAbilityIconsEventHandler(Texture2D[] icons);

	[Export]
	Texture2D[] abilityIcons = new Texture2D[3];

	[Export]
	PackedScene AbilityHUD;

	public override void _Process(double delta)
	{
		a1cdt += (float)delta;
		a2cdt += (float)delta;
		EmitSignal(SignalName.updateCooldowns, new float[] { a1cdt, a2cdt });
		

		// base._Process(delta);
	}

    public override void _Ready()
    {
		

		// hud.Connect(SignalName.updateCooldowns, new Callable(hud,"triggerUpdateCooldowns" ) );
		// EmitSignal(SignalName.setCooldowns, new float[] { a1cd, a2cd, a3cd });
		// EmitSignal(SignalName.setAbilityIcons, abilityIcons);


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
		
		else if (@event.IsActionReleased("primary_attack") && (maxPrimary > primary))
		{
			primary++;
			EmitSignal(SignalName.PrimaryAttack);
			GD.Print("Primary Attack");
		}
		else if (@event.IsActionReleased("secondary_attack")  && (maxSecondary > secondary))
		{
			secondary++;
			EmitSignal(SignalName.SecondaryAttack);
			GD.Print("Secondary Attack");
		}
	}


	public void resetPrimaryDebounce(){
		primary--;
	}

	public void resetSecondaryDebounce(){
		secondary--;
	}
	

	public void triggerHaste()
	{
		Rpc("haste");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void haste()
	{
		a1cdt += a1cd / 2;
		a2cdt += a2cd / 2;
		primary--;
		secondary--;

	}


}
