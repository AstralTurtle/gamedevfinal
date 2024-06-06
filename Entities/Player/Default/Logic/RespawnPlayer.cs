using Godot;
using System;
using System.Linq;

public partial class RespawnPlayer : Area2D
{
    int pclass = -1;
    int pnid = -1;

    [Export]
    public float respawnTime = 10.0f;
    public float respawnTimer = 0.0f;

    // Signals
    [Signal]
    public delegate void updateValueEventHandler(float value);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    public void init(int c, int id)
    {
        pclass = c;
        pnid = id;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (respawnTimer >= respawnTime)
        {
            GameManager gm = GetNode<GameManager>("/root/Game");
            gm.respawnPlayer(pclass, pnid, Position);

            // EmitSignal(nameof(updateValueEventHandler), 1.0f);
            QueueFree();
        }

        Node2D[] bodies = GetOverlappingBodies().ToArray();
        if (bodies.Length < 0 || bodies == null)
            return;
        GD.Print(bodies.Length);
        foreach (Node2D body in bodies)
        {
            if (body is Player)
            {
                respawnTimer += (float)delta;
                EmitSignal(SignalName.updateValue, respawnTimer / respawnTime);
            }
        }
    }
}
