using Godot;
using System;

public partial class DefaultSecondaryAttack : Node2D
{
	[Export]
	PackedScene secondaryAttack;
	public  override void _Ready() {
		
	}

	public void OnActivated() {
		

	}

	public void changeWeapon(PackedScene newWeapon){
		secondaryAttack = newWeapon;
	}
}
