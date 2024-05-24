using Godot;
using System;

public partial class DefaultPrimaryAttack : Node2D
{
	[Export]
	PackedScene primaryAttack;
	public  override void _Ready() {
		
	}

	public void OnActivated() {
		Rpc("OnActivatedRPC");
		
	}

	public void OnActivatedRPC(Vector2 dir){

	}

	



	public void changeWeapon(PackedScene newWeapon){
		primaryAttack = newWeapon;
	}
}
