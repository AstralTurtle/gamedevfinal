using Godot;
using System;

public partial class RangedAbilityInput : AbilityInput
{
	[Export]
	float secondaryCD = 1f;
	float secondaryCDT = 0;

	bool isBowMode = true;

	[Signal]
	public delegate void ModeChangedEventHandler(bool mode);
	[Signal]
	public delegate void LaserFiringEventHandler(bool isFiring);
	[Signal]
	public delegate void ArrowFiredEventHandler(float chargeTime);



	float arrowChargeTime = 0;
	float maxLaserCharge = 100f;
	float laserRegenRate = 25f;
	float laserChargeTime = 0;


	public override void _Process(double delta)
	{
		secondaryCDT += (float)delta;
		laserChargeTime += (float)delta * laserRegenRate;
		if (laserChargeTime > maxLaserCharge)
		{
			laserChargeTime += laserRegenRate * (float)delta;
		}
		base._Process(delta);
	}
	public override void _Input(InputEvent @event)
	{
		if (!IsMultiplayerAuthority()) return;


		if (@event.IsActionReleased("secondary_attack"))
		{
			if (secondaryCDT >= secondaryCD)
			{
				secondaryCDT = 0;
				isBowMode = !isBowMode;
				EmitSignal(SignalName.ModeChanged, isBowMode);
				EmitSignal(AbilityInput.SignalName.SecondaryAttack);

			}
			else GD.Print("Secondary Attack is on cooldown");
			// EmitSignal(AbilityInput.SignalName.SecondaryAttack);
			GD.Print("isBow?:" + isBowMode);
		}
		else if (isBowMode)
		{
			BowModeInput(@event);
		}
		else
		{
			MageModeInput(@event);
		}
		// base._Input(@event);
	}

	void MageModeInput(InputEvent @event)
	{
		if (@event.IsActionReleased("primary_attack"))
		{
			laserChargeTime -= 10;
			GD.Print("sending false");
			EmitSignal(SignalName.LaserFiring, false);

		}
		else if (@event.IsAction("primary_attack"))
		{
			if (laserChargeTime > 0)
			{
				laserChargeTime -= 1;
				GD.Print("sending true");

				EmitSignal(SignalName.LaserFiring, true);
			}
			else
			{
				GD.Print("sending false");

				EmitSignal(SignalName.LaserFiring, false);
			}
		}
		 
	}

	void BowModeInput(InputEvent @event)
	{
		if (@event.IsAction("primary_attack"))
		{
			arrowChargeTime += 1;
		}
		else if (@event.IsActionReleased("primary_attack"))
		{
			if (arrowChargeTime > 0)
			{
				EmitSignal(AbilityInput.SignalName.PrimaryAttack);
				GD.Print("Primary Attack");
			}
			arrowChargeTime = 0;
		}
	}




	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public new void haste()
	{
		secondaryCD += secondaryCDT / 2;
		base.haste();

	}


}
