using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export]
    float health = 20;

    [Export]
    float speed = 100;




    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void triggerDamage(){
        Rpc("takeDamage", 10);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void takeDamage(float dmg){
        health -= dmg;
        if (health <= 0){
            QueueFree();
        }
    }

}
