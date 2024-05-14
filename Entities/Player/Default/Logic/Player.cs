using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public const float Speed = 300.0f;
	[Export]
	public const float JumpVelocity = -400.0f;

	[Export]
	public String[] AnimNames = new String[] {"idle", "run", "jump_idle", "jump_run", "land", "hit", "death"};
	
	


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	

	[Export]
	public float health = 100.0f;
	[Signal]
	public delegate void HealthChangedEventHandler(float health);
	[Signal]
	public delegate void onAuthChangedEventHandler(int id);

	[Signal]
	public delegate void PlayerDiedEventHandler();

	[Signal]
	public delegate void PlayerRespawnedEventHandler();

	[Signal]
	public delegate void triggerAnimationEventHandler(string animationName);

	bool wasOnFloor = false;

	float lastHit = 0;

	public override void _Ready()
	{
		EmitSignal(SignalName.HealthChanged, health);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void takeDamage(float damage){
		EmitSignal(SignalName.triggerAnimation, AnimNames[5]);
		GD.Print("takeDamage");
		health -= damage;
		GD.Print(health);
		if(health <= 0){
			// QueueFree();
			// EmitSignal(SignalName.PlayerDied);
			EmitSignal(SignalName.triggerAnimation, AnimNames[6]);
		}
		EmitSignal(SignalName.HealthChanged, health);		
	}

	public void triggerMultiplayerAuthority(int id){
		GD.Print("triggerMultiplayerAuthority");
		// GD.Print("nameof: " + nameof(rpcMultiplayerAuthority));
		GD.Print(Rpc(nameof(setAuth), id));
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void setAuth(int id){
		GD.Print("setAuth");
		SetMultiplayerAuthority(id, true);
		EmitSignal(SignalName.onAuthChanged, id);

	}

    public override void _Input(InputEvent @event)
    {		
		if (!IsMultiplayerAuthority()) return;
		if (@event.IsAction("ui_down")){
			// GD.Print("ui_accept");
					Rpc("takeDamage", 10.0f);

		}
        // base._Input(@event);
    }	


    public override void _PhysicsProcess(double delta)
	{
		
		if (!IsMultiplayerAuthority()) return;

		Vector2 velocity = Velocity;

		wasOnFloor = IsOnFloor();

		// Add the gravity.
		if (!IsOnFloor()){
			velocity.Y += gravity * (float)delta;
			if (velocity.X == 0){
				EmitSignal(SignalName.triggerAnimation, AnimNames[2]);
			} else EmitSignal(SignalName.triggerAnimation, AnimNames[3]);
		}
		if (IsOnFloor() && !wasOnFloor){
			EmitSignal(SignalName.triggerAnimation, AnimNames[4]);
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor()){
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "jump", "ui_down");
		if (direction != Vector2.Zero)	
		{
			velocity.X = direction.X * Speed;
			if (direction.X < 0 && IsOnFloor()){
				// EmitSignal(SignalName.triggerAnimation, AnimNames[1]);
				EmitSignal(SignalName.triggerAnimation, AnimNames[1]);
			} else EmitSignal(SignalName.triggerAnimation, AnimNames[1]);
		} 
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		if (velocity.X == 0 && IsOnFloor()){
			EmitSignal(SignalName.triggerAnimation, AnimNames[0]);
		}


		

		Velocity = velocity;
	
		MoveAndSlide();
	}
}
