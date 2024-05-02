using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	



	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	

	[Export]
	public float health = 100.0f;
	[Signal]
	public delegate void HealthChangedEventHandler(float health);

	public override void _Ready()
	{
		EmitSignal(SignalName.HealthChanged, health);
	}

	public void takeDamage(float damage){
		health -= damage;
		if(health <= 0){
			// QueueFree();
		}
		EmitSignal(SignalName.HealthChanged, health);
		
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("ui_accept")){
			takeDamage(10);
		}
        // base._Input(@event);
    }


    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "jump", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
