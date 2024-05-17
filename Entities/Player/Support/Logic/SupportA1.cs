using Godot;
using System;

public partial class SupportA1 : Node
{
	[Signal]
	public delegate void ActivatedEventHandler();
	
	public void OnActivated() {
		EmitSignal(SignalName.Activated);
	}	
}
