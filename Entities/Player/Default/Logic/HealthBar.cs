using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public void OnHealthChanged(float health){
		Value = health;
	}
}
