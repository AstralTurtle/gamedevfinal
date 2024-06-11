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

    public int deathCount = 0;

    [Export]
    bool testmode = false;

    public override void _Process(double delta)
    {
        if (testmode)
        {
            GD.Print("deathcount: " + deathCount);
        }

        base._Process(delta);
    }

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

    public void setBaseValue(Stat stat, float value)
    {
        switch (stat)
        {
            case Stat.Health:
                maxHealth = value;
                break;
            case Stat.Damage:
                damage = value;
                break;
            case Stat.Speed:
                speed = value;
                break;
            case Stat.Jumps:
                maxJumps = (int)value;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public void setFlatMod(Stat stat, float val)
    {
        switch (stat)
        {
            case Stat.Health:
                hpfmod += val;
                break;
            case Stat.Damage:
                dmgfmod += val;
                break;
            case Stat.Speed:
                speedfmod += val;
                break;
            case Stat.Jumps:
                Jumpsfmod += (int)val;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public void setPercentMod(Stat stat, float mod)
    {
        switch (stat)
        {
            case Stat.Health:
                hppmod += mod;
                break;
            case Stat.Speed:
                speedpmod += mod;
                break;
            case Stat.Damage:
                dmgpmod += mod;
                break;
        }
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }

    public void onPlayerDie()
    {
        deathCount++;
        Name = "StatManager" + GetParent().Name;
        // move to game so player doesn't lose all stats
        Reparent(GetTree().Root.GetNode<Node2D>("Game"));
        // change name so player can be respawned
    }

    public void onPlayerSpawn()
    {
        // check if there is already a statmanager
        var oldSM = GetTree().Root.GetNodeOrNull<Node2D>("Game/StatManager" + GetParent().Name);
        if (oldSM != null)
        {
            oldSM.Reparent(GetParent());
            Name = GD.Randi().ToString();
            oldSM.Name = "StatManager";
            // if there is, delete this one
            QueueFree();
            return;
        }
    }

    public override void _EnterTree()
    {
        CallDeferred("onPlayerSpawn");
        base._EnterTree();
    }

    public override void _Ready()
    {
        EmitSignal(
            SignalName.StatChanged,
            new float[] { getHealth(), getSpeed(), (float)getJumps() }
        );
    }
}
