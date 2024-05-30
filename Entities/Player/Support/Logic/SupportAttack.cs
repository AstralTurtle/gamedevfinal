using Godot;
using System;
using System.Threading.Tasks;

public partial class SupportAttack : Area2D
{
	[Signal]
	public delegate void SuccessfulHitEventHandler();
	[Export]
	int dmg = 1;
	[Export]
	public float speed = 100f;

	Vector2 dir = new Vector2(0, 0);

	public void setDir(Vector2 dr)
	{
		dir = dr;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnSupportAttackBodyEntered;
		destroyOnTime();

	}

    public override void _Process(double delta)
    {
		Position = Position + dir * speed * (float)delta;

        base._Process(delta);
    }

	

    public async Task destroyOnTime(){
		GD.Print("Destroying");
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
		GD.Print("destroyed");
		QueueFree();
	}

	public void OnSupportAttackBodyEntered(Node body)
	{
		if (body is Enemy)
		{
			// (body as Enemy).TakeDamage(dmg);
			EmitSignal(SignalName.SuccessfulHit);
		}
		QueueFree();
	}
}