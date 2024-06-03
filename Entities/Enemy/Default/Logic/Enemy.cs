using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody2D
{
    [Signal]
    public delegate void AnimChangedEventHandler(string animName);
    [Signal]
    public delegate void lookDirectionEventHandler(Vector2 dir);

    [Export]
    String[] animNames = { "idle", "run", "jump"};


    [Export]
    float health = 20;

    [Export]
    protected float Speed = 100;

    [Export]
    protected float JumpVelocity = -200;

    [Export]
    protected float damage = 10;

    protected Vector2 direction = Vector2.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.


    public void OnCollisionEnter(Node body)
    {
        GD.Print("Collision" + body.Name);
        if (body is Player)
        {
            GD.Print("Player Collision");
            Player player = (Player)body;
            player.triggerDamage(damage);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i).GetCollider();
            if (collision is Player)
            {
                Player player = (Player)collision;
                player.triggerDamage(damage);
            }
        }
    }

    public virtual bool PlayerDetection()
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
                    EmitSignal(SignalName.lookDirection, direction);
                     return true;
                }
            }
        }
        return false;
       
    }

    public void triggerDamage(float dmg)
    {
        Rpc("takeDamage", dmg);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void takeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            QueueFree();
        }
        GD.Print("Health: " + health);
    }
}
