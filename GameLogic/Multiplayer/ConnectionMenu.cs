using Godot;
using System;

public partial class ConnectionMenu : Control
{
	[Signal]
	public delegate void OnHostEventHandler();
	[Signal]
	public delegate void OnJoinEventHandler();
	// Attempt to host a server
	public void OnHostPressed(){
		EmitSignal(SignalName.OnHost);
	}

	public void OnJoinPressed(){
		EmitSignal(SignalName.OnJoin);
	}

	public void onSuccessfulConnection(){
		Visible = false;
	}
}
