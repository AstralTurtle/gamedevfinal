using Godot;
using System;

public partial class HealthLabel : Label
{
	public void OnHealthChanged(float health){
		Text = Mathf.FloorToInt(health).ToString();
	}
}
