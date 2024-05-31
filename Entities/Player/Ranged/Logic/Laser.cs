using Godot;
using System;
using System.Diagnostics;

public partial class Laser : RayCast2D
{
    bool isFiring = false;
    StatManager sm;
    GpuParticles2D particles;

    Line2D line;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sm = GetParent().GetNode<StatManager>("StatManager");
        line = GetNode<Line2D>("Line2D");
        particles = GetNode<GpuParticles2D>("Collision");
        line.SetPointPosition(1, Vector2.Zero);
        particles.Emitting = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        Pivot();
        // GD.Print("f: " + isFiring + "c: " + IsColliding());
        if (isFiring)
        {
            var CastPoint = TargetPosition;
            ForceRaycastUpdate();
            if (IsColliding())
            {
                GD.Print(GetCollisionPoint);
                CastPoint = ToLocal(GetCollisionPoint());
                particles.Emitting = true;
                if (GetCollider() is Node)
                {
                    GD.Print("collider: " + (GetCollider() as Node).Name);
                }
                if (GetCollider() is Enemy)
                {
                    (GetCollider() as Enemy).triggerDamage(sm.getDamage() * (float)delta);
                }
                particles.GlobalRotation = GetCollisionNormal().Angle();
                particles.Position = CastPoint;
            }
            else
            {
                particles.Emitting = false;
            }
            line.SetPointPosition(1, CastPoint);
        }
        else
        {
            line.SetPointPosition(1, new Vector2(0, 0));
            particles.Emitting = false;
        }
    }

    void Pivot()
    {
        if (!IsMultiplayerAuthority())
            return;

        //  GD.Print("pivoting");
        Vector2 mpos = GetViewport().GetMousePosition();
        Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
        dir = dir.Normalized();

        float ang = GetAngleFromDirection(dir);

        // GD.Print("ang:"+ ang);
        // GD.Print("b4"+Rotation);
        Rotation = ang;
        // GD.Print("aft"+Rotation);
    }

    private float GetAngleFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.Y, direction.X);
    }

    void StartLaserTween()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(line, "width", 10, 0.1);
    }

    void EndLaserTween()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(line, "width", 0, 0.1);
    }

    void triggerLaser(bool laser)
    {
        GD.Print(laser);
        Rpc("triggerLaserRPC", laser);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    void triggerLaserRPC(bool laser)
    {
        GD.Print("laser:" + laser);
        isFiring = laser;
        // Enabled = isFiring;
        if (isFiring)
        {
            StartLaserTween();
        }
        else
        {
            EndLaserTween();
        }
    }
}
