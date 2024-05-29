using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public void OnHealthChanged(float health){
		Value = health;
	}

	public void maxHealthChanged(float[] maxHealth){
		MaxValue = maxHealth[0];
	}
}
