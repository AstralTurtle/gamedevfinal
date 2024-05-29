using Godot;
using System;

public partial class StatManager : Node2D
{
	[Signal]
	public delegate void StatChangedEventHandler(float[] change);	

	// Base Stats
	[Export]
	public float maxHealth = 100;
	[Export]
	public float speed = 100;
	[Export]
	public float damage = 10;

	// Percent Modifiers
	[Export]
	public float hppmod = 1;
	[Export]
	public float speedpmod = 1;
	[Export]
	public float dmgpmod = 1;
	// Flat Modifiers

	[Export]
	public float hpfmod = 0;
	[Export]
	public float speedfmod = 0;
	[Export]
	public float dmgfmod = 0;

	public float getHealth(){
		return (maxHealth * hppmod) + hpfmod;
	}

	public float getSpeed(){
		return  (speed * speedpmod) + speedfmod;
	}

	public float getDamage(){
		return (damage * dmgpmod) + dmgfmod;
	}

	public void setBaseValue(String stat, float value){
		switch (stat){
			case "health":
				maxHealth = value;
				break;
			case "speed":
				speed = value;
				break;
			case "damage":
				damage = value;
				break;
		}
		EmitSignal(SignalName.StatChanged, new float[] {getHealth(), getSpeed()});
	}

	public void setFlatMod(string stat, float mod){
		switch(stat){
			case "health":
				hpfmod = mod;
				break;
			case "speed":
				speedfmod = mod;
				break;
			case "damage":
				dmgfmod = mod;
				break;
		}
		EmitSignal(SignalName.StatChanged, new float[] {getHealth(), getSpeed()});
	}

	public void setPercentMod(string stat, float mod){
		switch(stat){
			case "health":
				hppmod = mod;
				break;
			case "speed":
				speedpmod = mod;
				break;
			case "damage":
				dmgpmod = mod;
				break;
		}
		EmitSignal(SignalName.StatChanged, new float[] {getHealth(), getSpeed()});
	}

	public override void _Ready()
	{
		EmitSignal(SignalName.StatChanged, new float[] {getHealth(), getSpeed()});
	}



}
