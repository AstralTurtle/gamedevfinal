using Godot;
using System;

public partial class Projectile : RigidBody2D
{

	// send signal from rigidbody here
	public void OnCollisionEntered(Node body){
		GD.Print("collided with something");
	}
}
