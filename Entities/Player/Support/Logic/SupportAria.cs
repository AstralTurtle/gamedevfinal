using Godot;
using System;
using System.Linq;

public partial class SupportAria : Area2D
{
	Player[] players = new Player[4];

	[Export]
	float healAmount = 10.0f;

	[Export]
	float speedboost = 1.0f;

	bool isHealing = false;

	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (players.Length > 0){
			foreach (Player player in players){
				if (player != null){
					if (isHealing){
					player.triggerHeal(healAmount * (float)delta);
					}
					else {
						player.triggerSpeedBoost(speedboost);
					}
				}
			}
		}
	}

	public void SupportAriaEntered(object body)
	{
		if (body is Player)
		{
			Player player = (Player)body;
			if (!players.Contains(player)){
				players[players.Length] = player;	
			}
			
		}
	}

	public void SupportAriaExited(object body)
	{
		if (body is Player)
		{
			Player player = (Player)body;
			if (players.Contains(player)){
				players = players.Where(val => val != player).ToArray();
			}
		}
	}



}
