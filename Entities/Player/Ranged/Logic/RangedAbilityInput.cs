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

    bool isArrowCharging = false;
    float arrowChargeTime = 0;

    [Export]
    float maxArrowCharge = 1;

    [Export]
    float arrowChargeRate = 1f;
    bool isLaserFiring = false;

    [Export]
    float maxLaserCharge = 100f;

    [Export]
    float laserRegenRate = 40f;

    [Export]
    float laserTimeout = 5f;
    float laserChargeTime = 0;

    [Export]
    float laserDecayRate = 20f;

    public override void _Process(double delta)
    {
        secondaryCDT += (float)delta;
        if (!isLaserFiring)
            laserChargeTime += (float)delta * laserRegenRate;
        else
            laserChargeTime -= (float)delta * laserDecayRate;
        if (laserChargeTime >= maxLaserCharge)
        {
            laserChargeTime = maxLaserCharge;
        }
        if (laserChargeTime < 0)
        {
            laserChargeTime = -100;
            GetTree().CreateTimer(laserTimeout).Timeout += () =>
            {
                // GD.Print("Laser Charge Reset");
                laserChargeTime = maxLaserCharge / 2;
            };
            isLaserFiring = false;
            EmitSignal(SignalName.LaserFiring, false);
        }

        if (isArrowCharging)
        {
            arrowChargeTime += (float)delta * arrowChargeRate;
        }

        // GD.Print("Laser Charge: " + laserChargeTime);
        base._Process(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (!IsMultiplayerAuthority())
            return;

        if (@event.IsActionReleased("secondary_attack"))
        {
            if (secondaryCDT >= secondaryCD)
            {
                secondaryCDT = 0;
                isBowMode = !isBowMode;
                EmitSignal(SignalName.ModeChanged, isBowMode);
                EmitSignal(AbilityInput.SignalName.SecondaryAttack);
            }
            else
                GD.Print("Secondary Attack is on cooldown");
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
        if (@event.IsActionReleased("primary_attack") && laserChargeTime > 0)
        {
            isLaserFiring = false;

            laserChargeTime -= 10;
            if (laserChargeTime < 0)
                laserChargeTime = 0;
            GD.Print("sending false");
            EmitSignal(SignalName.LaserFiring, false);
        }
        else if (@event.IsAction("primary_attack") && laserChargeTime > 0)
        {
            if (laserChargeTime > 0)
            {
                isLaserFiring = true;

                EmitSignal(SignalName.LaserFiring, true);
            }
        }
    }

    void BowModeInput(InputEvent @event)
    {
        if (@event.IsActionReleased("primary_attack"))
        {
            if (arrowChargeTime > maxArrowCharge)
                arrowChargeTime = maxArrowCharge;

            EmitSignal(SignalName.ArrowFired, arrowChargeTime);
            GD.Print("Primary Attack");
            isArrowCharging = false;
            arrowChargeTime = 0;
        }
        else if (@event.IsAction("primary_attack"))
        {
            isArrowCharging = true;
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public new void haste()
    {
        secondaryCD += secondaryCDT / 2;
        base.haste();
    }
}
