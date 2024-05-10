using Godot;
using System;

public partial class CooldownIconManager : Control
{
	CooldownIcon[] icons = new CooldownIcon[3];

	public override void _Ready()
	{
		getIcons();
	}

	void getIcons(){
		icons[0] = GetNode<CooldownIcon>("CooldownIcon1");
		icons[1] = GetNode<CooldownIcon>("CooldownIcon2");
		icons[2] = GetNode<CooldownIcon>("CooldownIcon3");
	
	}

	public void setIcons(Texture2D[] icons) {
		for (int i = 0; i < 3; i++)
		{
			this.icons[i].TextureProgress = icons[i];
		}
	}	

	public void updateCooldown(float[] cooldowns)
	{
		
		for (int i = 0; i < 3; i++)
		{
			icons[i].updateCooldown(cooldowns[i]);
		}
	}

	public void setCooldowns(float[] cooldowns)
	{
		if (icons[0] == null)
		{
			getIcons();
		}
		for (int i = 0; i < 3; i++)
		{
			icons[i].setCooldown(cooldowns[i]);
		}
	}

}
