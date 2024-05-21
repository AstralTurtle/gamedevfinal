using Godot;
using System;

public partial class DefaultPrimaryAttack : Node2D
{
	[Export]
	PackedScene primaryAttack;
	public  override void _Ready() {
		
	}

	public void OnActivated() {
		

	}

	



	public void changeWeapon(PackedScene newWeapon){
		primaryAttack = newWeapon;
	}
}
