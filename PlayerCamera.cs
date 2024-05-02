using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(IsMultiplayerAuthority()){
			Enabled = true;
		} else {
			Enabled = false;
		}
	}
}
