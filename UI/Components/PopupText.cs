using Godot;
using System;

public partial class PopupText : LineEdit
{
	public override void _Ready(){
		Visible = false;
	}

	public void popup(){
		Visible = !Visible;
	}
}
