using Godot;
using System;

public partial class ShooterEnemy : Enemy
{
	 Vector2 velocity = Vector2.Zero;
	    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


	[Export]
	PackedScene throwObjScene;
	[Export]
	float throwcd = 2;
	 float throwcdt = 0;

	
	// Throw above the player
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		throwcdt += (float)delta;
		bool isplayer = PlayerDetection();

		if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

		if (throwcdt >= throwcd)
		{
			throwcdt = 0;
			
			Rpc("shootObj", direction);

		}
		if (isplayer){
		if (direction != Vector2.Zero)
        {
			EmitSignal(Enemy.SignalName.AnimChanged, "run");
            velocity.X = -(direction.X * Speed);
        }
        else
        {
			EmitSignal(Enemy.SignalName.AnimChanged, "run");
            velocity.X = -Mathf.MoveToward(Velocity.X, 0, Speed);
        }
		 if (direction.Y < -0.5 && IsOnFloor())
        {
			EmitSignal(Enemy.SignalName.AnimChanged, "jump");
            velocity.Y = JumpVelocity;
        }
		}
		if (direction == Vector2.Zero){
			EmitSignal(Enemy.SignalName.AnimChanged, "idle");
		}

       

		Velocity = velocity;
		MoveAndSlide();
		base._PhysicsProcess(delta);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void shootObj(Vector2 dir){
		ShootableProjectile throwObj = throwObjScene.Instantiate<ShootableProjectile>();
		throwObj.SetDamage(damage);
		throwObj.Position = Position + new Vector2(20, 0) * dir;
		GetTree().Root.GetNode("Game").AddChild(throwObj);


		// needs to throw up and in the players direction
		throwObj.SetDirection(dir);

		



	}

	
}
