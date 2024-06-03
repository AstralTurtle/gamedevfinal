using Godot;
using System;

public partial class ZombieEnemy : Enemy
{
    Vector2 velocity = Vector2.Zero;
    Vector2 direction = Vector2.Zero;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        PlayerDetection();
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (direction.Y < -0.5 && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        Velocity = velocity;

        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

    public void PlayerDetection()
    {
        var shape = GetNode<ShapeCast2D>("ShapeCast2D");
        if (shape.IsColliding())
        {
            GD.Print("Is colliding " + shape.GetCollisionCount());
            for (int i = 0; i < shape.GetCollisionCount(); i++)
            {
                var collision = shape.GetCollider(i);
                if (collision is Player)
                {
                    direction = (
                        (collision as Player).GlobalPosition - GlobalPosition
                    ).Normalized();
                    GD.Print("direction: " + direction);

                    break;
                }
            }
        }
    }
}
