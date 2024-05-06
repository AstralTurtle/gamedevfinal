using Godot;
using System;

public partial class CooldownIcon : TextureProgressBar
{
	public void updateCooldown(float cooldown) {
		Value = cooldown;
	}

	public override void _Ready(){
		if (!IsMultiplayerAuthority()) Hide();
	}

	public void setCooldown(float max){
		MaxValue = max;
		Value = max;
	}
}
