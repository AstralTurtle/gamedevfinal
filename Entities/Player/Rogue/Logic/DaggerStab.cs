using Godot;
using System;

public partial class DaggerStab : Node2D
{
    [Export]
    float speed = 500;

    [Export]
    float distance = 10;

    Vector2 location;

    float damage = 0;

    public void stab(Vector2 dir)
    {
        location = Position + (dir * distance);

        float angle = Position.AngleTo(location);
        Rotation = -angle;

        GD.Print(location);
        //rotate towards location
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public override void _Process(double delta)
    {
        Position = Position.MoveToward(location, (float)delta * speed);

        // GD.Print(Position + " vs " + location);
        if (Position == location)
        {
            QueueFree();
        }
    }

    public void OnCollisionEntered(Node body)
    {
        if (body is Player)
            return;
        if (body is Enemy)
        {
            Enemy enemy = (Enemy)body;
            enemy.triggerDamage(damage);
        }
        QueueFree();
    }
}
