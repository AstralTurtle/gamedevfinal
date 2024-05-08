using Godot;
using System;

public partial class CharacterSelect : Control
{
	[Signal]
	public delegate void selectCharacterEventHandler(int character);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void openLobby(){
		Visible = true;
	}
	public void updatePlayerCountText(int players){
		Label playerCount = GetNode<Label>("PlayerCount");
		playerCount.Text = "Players: " + players.ToString();
	}
}

