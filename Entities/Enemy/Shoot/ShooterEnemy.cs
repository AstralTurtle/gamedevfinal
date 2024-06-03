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

	Vector2 direction = Vector2.Zero;
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
		PlayerDetection();

		if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

		if (throwcdt >= throwcd)
		{
			throwcdt = 0;
			
			shootObj(direction);

		}

		if (direction != Vector2.Zero)
        {
            velocity.X = -(direction.X * Speed);
        }
        else
        {
            velocity.X = -Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (direction.Y < -0.5 && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
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

	public void PlayerDetection()
    {
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
    }
}
