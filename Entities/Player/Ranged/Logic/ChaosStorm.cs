using Godot;
using System;
using System.Linq;

public partial class ChaosStorm : Area2D
{
    float damage = 0;

    [Export]
    float lifetime = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        GetTree().CreateTimer(lifetime).Timeout += () =>
        {
            QueueFree();
        };
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // GD.Print("chaos storm @ " + Position);
        Node2D[] bodies = GetOverlappingBodies().ToArray();
        if (bodies == null || bodies.Length == 0)
            return;
        foreach (Node2D body in bodies)
        {
            if (body is Enemy)
            {
                (body as Enemy).triggerDamage(damage * (float)delta);
            }
        }
    }
}
