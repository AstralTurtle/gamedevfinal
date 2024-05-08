using Godot;
using System;

public partial class CooldownIconManager : Control
{
	CooldownIcon[] icons = new CooldownIcon[3];

	public override void _Ready()
	{
		icons[0] = GetNode<CooldownIcon>("CooldownIcon1");
		icons[1] = GetNode<CooldownIcon>("CooldownIcon2");
		icons[2] = GetNode<CooldownIcon>("CooldownIcon3");
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
		for (int i = 0; i < 3; i++)
		{
			icons[i].setCooldown(cooldowns[i]);
		}
	}

}
