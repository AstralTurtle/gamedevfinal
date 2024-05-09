using Godot;
using System;

public partial class AbilityHUD : CanvasLayer
{
	[Signal]
	public delegate void setCooldownIconsEventHandler(Texture2D[] icons);
	[Signal]
	public delegate void updateCooldownsEventHandler(float[] cooldowns);
	[Signal]
	public delegate void setCooldownsEventHandler(float[] cooldowns);

	public void triggerUpdateCooldowns(float[] cooldowns)
	{
		EmitSignal(SignalName.updateCooldowns, cooldowns);
	}

	public void triggerSetCooldownIcons(Texture2D[] icons)
	{
		EmitSignal(SignalName.setCooldownIcons, icons);
	}

	public void triggerSetCooldowns(float[] cooldowns)
	{
		EmitSignal(SignalName.setCooldowns, cooldowns);
	}

	


}
