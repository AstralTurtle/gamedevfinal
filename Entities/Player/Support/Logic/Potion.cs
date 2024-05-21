using Godot;
using System;

public partial class Potion : Area2D
{
	int type;

	[Export]
	float healAmount = 20.0f;

	[Export]
	float speedboost = 100.0f;

	[Signal]
	public delegate void SetSpriteEventHandler(Texture2D texture);

	public void setType(int t){
		type = t;
	}

	public override void _Ready(){
		BodyEntered += PotionEntered;
	}

	public void PotionEntered(Node body){
		if (!(body is Player)) return;
		Player player = (Player)body;

		switch (type)
		{
			case 0:
				player.triggerHeal(healAmount);
				GD.Print("Healing");
				break;
			case 1:
				player.triggerSpeedBoost(speedboost);
				GD.Print("Speedboost");
				break;
			case 2:
				player.triggerHaste();
				GD.Print("Haste");
				break;
			
			default:
				player.triggerHeal(3.0f);
				break;
		}

		QueueFree();


	}

	public void changeSprite(Texture2D texture){
		EmitSignal(SignalName.SetSprite, texture);
	}


}
