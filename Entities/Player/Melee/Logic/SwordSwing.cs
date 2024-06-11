using Godot;
using System;

public partial class SwordSwing : Area2D
{
    float damage = 10;

    [Export]
    float lifetime = 0.5f;

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        // BodyEntered += OnCollisionEntered;
        GetTree().CreateTimer(lifetime).Timeout += deleteSelf;

        base._EnterTree();
    }

    void deleteSelf()
    {
        QueueFree();
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public override void _Ready()
    {
        BodyEntered += OnCollisionEntered;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.


    public void OnCollisionEntered(Node body)
    {
        if (body is Player)
            return;
        if (body is Enemy)
        {
            GD.Print("Hit Enemy");
            Enemy e = (Enemy)body;
            e.triggerDamage(damage);
        }
    }
}
