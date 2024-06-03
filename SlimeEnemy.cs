using Godot;
using System;

public partial class SlimeEnemy : Enemy
{
    public float jumpcdt = 0.0f;
    public float jumpcd = 2.0f;

    Vector2 direction = Vector2.Zero;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        jumpcdt += (float)delta;

        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        if (jumpcdt > jumpcd)
        {
            direction = Vector2.Zero;
            GD.Print("Jumping");
            // shapecast to see if there is a player nearby
            var shape = GetNode<ShapeCast2D>("ShapeCast2D");

            if (shape.IsColliding())
            {
                for (int i = 0; i < shape.GetCollisionCount(); i++)
                {
                    var collision = shape.GetCollider(i);
                    if (collision is Player)
                    {
                        direction = (
                            (collision as Player).GlobalPosition - GlobalPosition
                        ).Normalized();

                        break;
                    }
                }
            }
            jumpcdt = 0.0f;
            velocity.Y = JumpVelocity;
        }

        if (!IsOnFloor() && direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
        base._PhysicsProcess(delta);
    }
}
