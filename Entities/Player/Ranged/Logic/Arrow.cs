using Godot;
using System;

public partial class Arrow : RigidBody2D
{
    [Export]
    float speed = 350;

    float damage = 0;

    float chargeTime = 0;

    public override void _EnterTree()
    {
        GD.Print(
            "Arrow entered tree with charge: "
                + chargeTime
                + " with damage: "
                + (damage * getChargeMultiplier())
        );

        base._EnterTree();
    }

    public override void _ExitTree()
    {
        GD.Print("Arrow exited tree");
        base._ExitTree();
    }

    public void SetChargeTime(float time)
    {
        chargeTime = time;
    }

    public float getChargeMultiplier()
    {
        if (chargeTime > 0.75)
        {
            return chargeTime + 0.5f;
        }
        else
        {

            return chargeTime;
        }
    }

    public void throwObj(Vector2 dir)
    {
        ApplyImpulse(dir * speed * getChargeMultiplier());
    }

    public override void _Ready()
    {
        BodyEntered += OnCollisionEntered;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public override void _Process(double delta)
    {
        // GD.Print(RotationDegrees);

        float angle = 0;
        // rotate sprite to match velocity
        if (LinearVelocity.Length() > 0)
        {
            angle = Mathf.Atan2(LinearVelocity.Y, LinearVelocity.X);
        }
        Rotation = angle;

        base._Process(delta);
    }

    public void OnCollisionEntered(Node body)
    {
        if (body is Enemy)
        {
            (body as Enemy).triggerDamage(damage * getChargeMultiplier());
        }
        QueueFree();
    }
}
