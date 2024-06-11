using Godot;
using System;

public partial class Thrower : Enemy
{	

    Vector2 velocity = Vector2.Zero;
	    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


	[Export]
	PackedScene throwObjScene;
	[Export]
	float throwcd = 2;
	 float throwcdt = 0;

	
	// Throw above the player
	 Vector2 throwDirOffset = new Vector2(0, -1);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		throwcdt += (float)delta;
		bool isplayer = PlayerDetection();

		if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

		if (throwcdt >= throwcd)
		{
			throwcdt = 0;
			
						if (IsMultiplayerAuthority())
			Rpc("throwObj", direction);

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
        }}

		Velocity = velocity;
		MoveAndSlide();
		base._PhysicsProcess(delta);
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void throwObj(Vector2 dir){
		ThrowableProjectile throwObj = throwObjScene.Instantiate<ThrowableProjectile>();
		throwObj.SetDamage(damage);
		throwObj.Position = Position + new Vector2(0, -50);
		GetTree().Root.GetNode("Game").AddChild(throwObj);


		// needs to throw up and in the players direction
		throwObj.throwObj(dir + throwDirOffset	);
	

		



	}

	
}
