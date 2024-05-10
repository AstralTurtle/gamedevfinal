using Godot;
using System;

public partial class CharacterSelect : Control
{
	[Signal]
	public delegate void selectCharacterEventHandler(int character);

	[Signal]
	public delegate void StartGamePressedEventHandler();
	

	public void openLobby(){
		Visible = true;
	}

	public void startGame(){
		Visible = false;
	
	}

	// mr passthrough himself
	public void triggerStartGamePressed(){
		if (!IsMultiplayerAuthority()) return;
		EmitSignal(SignalName.StartGamePressed);
	}

	public void updatePlayerCountText(int players){
		Label playerCount = GetNode<Label>("PlayerCount");
		playerCount.Text = "Players: " + players.ToString();
	}


	public void triggerCharacterChosen(int index){
		GD.Print(index);
		EmitSignal(SignalName.selectCharacter, index);
	}
}

