using Godot;
using System;
using System.Threading.Tasks;

public partial class SupportAttack : Area2D
{
    [Signal]
    public delegate void SuccessfulHitEventHandler();
    float damage = 0;

    [Export]
    public float speed = 200f;

    Vector2 dir = new Vector2(0, 0);

    [Export]
    public float lifetime = 5f;

    public void setDir(Vector2 dr)
    {
        dir = dr;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += OnSupportAttackBodyEntered;
        destroyOnTime();
    }

    public override void _Process(double delta)
    {
        Position = Position + dir * speed * (float)delta;

        base._Process(delta);
    }

    public async Task destroyOnTime()
    {
        GD.Print("Destroying");
        await ToSignal(GetTree().CreateTimer(lifetime), SceneTreeTimer.SignalName.Timeout);
        GD.Print("destroyed");
        QueueFree();
    }

    public void OnSupportAttackBodyEntered(Node body)
    {
        if (body is Enemy)
        {
            GD.Print("Hit Enemy");
            (body as Enemy).triggerDamage(damage);
            EmitSignal(SignalName.SuccessfulHit);
        }
        QueueFree();
    }
}
