using Godot;
using System;

public partial class ZombieEnemy : Enemy
{
    Vector2 velocity = Vector2.Zero;
  
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
            EmitSignal(Enemy.SignalName.AnimChanged, "run");
            velocity.X = direction.X * Speed;
        }
        else
        {
            EmitSignal(Enemy.SignalName.AnimChanged, "jump");
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (direction == Vector2.Zero){
            EmitSignal(Enemy.SignalName.AnimChanged, "idle");
        }

        if (direction.Y < -0.5 && IsOnFloor())
        {
            EmitSignal(Enemy.SignalName.AnimChanged, "jump");
            velocity.Y = JumpVelocity;
        }

        Velocity = velocity;

        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

}
