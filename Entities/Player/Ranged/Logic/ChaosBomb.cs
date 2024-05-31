using Godot;
using System;
using System.Collections;
using System.Linq;

public partial class ChaosBomb : RigidBody2D
{
    GpuParticles2D explosion;
    CollisionShape2D projectileCollider;
    Area2D explosionArea;
    float damage = 10;
    Node2D pivot;

    [Export]
    float speed = 400;

    float rotSpeed = (float)Math.PI * 5;

    bool hasExploded = false;

    bool timerStarted = false;
    float timer = 0;
    float timeout = 1.1f;

    public void throwObj(Vector2 dir)
    {
        ApplyImpulse(dir * speed);
    }

    public void setDamage(float damage)
    {
        this.damage = damage;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        explosion = GetNode<GpuParticles2D>("ExplosionParticles");
        explosionArea = GetNode<Area2D>("Explosion");
        pivot = GetNode<Node2D>("SpritePivot");
        BodyEntered += OnCollisionEnter;
        projectileCollider = GetNode<CollisionShape2D>("ProjectileCollider");
        explosion.Finished += () =>
        {
            QueueFree();
        };
    }

    public override void _Process(double delta)
    {
        pivot.Rotate(rotSpeed * (float)delta);
        base._Process(delta);
        if (timerStarted)
        {
            // GD.Print("Timer: " + timer);
            timer += (float)delta * 2;
            if (timer > timeout)
            {
                QueueFree();
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    private void OnCollisionEnter(Node other)
    {
        timerStarted = true;
        if (other is Enemy)
        {
            (other as Enemy).triggerDamage(damage);
        }
        // Disable the projectile
        pivot.Visible = false;
        LinearVelocity = Vector2.Zero;

        explosionArea.Monitoring = true;
        explosion.Emitting = true;

        Node2D[] bodies = explosionArea.GetOverlappingBodies().ToArray();
        GD.Print(bodies);
        if (bodies == null || bodies.Length == 0)
        {
            GD.Print("No bodies in explosion area");
            return;
        }
        if (!hasExploded)
        {
            hasExploded = true;
            foreach (Node2D body in bodies)
            {
                GD.Print(body.Name);
                if (body is Enemy)
                {
                    GD.Print("Hit Enemy With Explosion" + body.Name);
                    (body as Enemy).triggerDamage(damage);
                }
            }
        }
        // projectileCollider.SetDeferred("disabled", true);
    }

    public void disablePhysics() { }
}
