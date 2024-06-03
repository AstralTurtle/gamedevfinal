using Godot;
using System;

public partial class StatManager : Node2D
{
    [Signal]
    public delegate void StatChangedEventHandler(float[] change);

    // Base Stats
    [Export]
    public float maxHealth = 100;

    [Export]
    public float speed = 100;

    [Export]
    public float damage = 10;

    // Percent Modifiers
    [Export]
    public float hppmod = 0;

    [Export]
    public float speedpmod = 0;

    [Export]
    public float dmgpmod = 0;

    // Flat Modifiers

    [Export]
    public float hpfmod = 0;

    [Export]
    public float speedfmod = 0;

    [Export]
    public float dmgfmod = 0;

    [Export]
    public int maxJumps = 1;

    [Export]
    public int Jumpsfmod = 0;

    public float getHealth()
    {
        return (maxHealth + hpfmod) * (hppmod + 1);
    }

    public float getSpeed()
    {
        return (speed + speedfmod) * (speedpmod + 1);
    }

    public float getDamage()
    {
        return (damage + dmgfmod) * (dmgpmod + 1);
    }

    public int getJumps()
    {
        return maxJumps + Jumpsfmod;
    }

    public void setBaseValue(StatType stat, float value)
    {
        switch (stat)
        {
            case StatType.Health:
                maxHealth = value;
                break;
            case StatType.Damage:
                damage = value;
                break;
            case StatType.Speed:
                speed = value;
                break;
            case StatType.Jumps:
                maxJumps = (int)value;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public void setFlatMod(StatType stat, float val)
    {
        switch (stat)
        {
            case StatType.Health:
                hpfmod += val;
                break;
            case StatType.Damage:
                dmgfmod += val;
                break;
            case StatType.Speed:
                speedfmod += val;
                break;
            case StatType.Jumps:
                Jumpsfmod += (int)val;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public void setPercentMod(StatType stat, float mod)
    {
        switch (stat)
        {
            case StatType.Health:
                hppmod += mod;
                break;
            case StatType.Speed:
                speedpmod += mod;
                break;
            case StatType.Damage:
                dmgpmod += mod;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public override void _Ready()
    {
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }
}
