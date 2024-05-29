using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class MeleePlayer : Player
{
	[Export]
	float shieldHealth = 50;
	[Export]
	float shieldRegenRate = 0.5f;
	[Export]
	float shieldMaxHealth = 0;
	[Export]
	float shieldRespawnThreshold = 50;

	Node2D shieldSprite;

	bool hasShield = true;

	
	public override void _Ready()
	{
		shieldSprite = GetNode<Node2D>("ShieldSprite");
		shieldSprite.Visible = false;
		base._Ready();
		shieldMaxHealth += maxHealth/4;
		shieldHealth = shieldMaxHealth;
		shieldRespawnThreshold = shieldMaxHealth;

	}

	public new void updateStats(float[] stats){
		maxHealth = stats[0];
		Speed = stats[1];
		shieldMaxHealth =  maxHealth/4;
		shieldRespawnThreshold = shieldMaxHealth;
		GD.Print("shieldMaxHealth update: " + shieldMaxHealth);
	}

	bool isShieldActive = false;
	bool isShieldDestroyed =false;

	public float ThrowShield(){
		if (isShieldDestroyed){
			return 0;
		}
		hasShield = false;
		shieldSprite.Visible = false;
		return shieldHealth;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public new void takeDamage(float damage){
		GD.Print("takingDamage");
		if (isShieldActive){
			GD.Print("pre sh: " + shieldHealth);
			shieldHealth -= damage;	
			GD.Print("sh: " + shieldHealth);
			if (shieldHealth <= 0){
				shieldSprite.Visible = false;
				isShieldDestroyed = true;
				isShieldActive = false;
				// EmitSignal(SignalName.triggerAnimation, AnimNames[7]);
			}
			return;

		}
		
		

		
		
		if (!canBeHit) {
			GD.Print("Debounce");
			return;
		}




		GD.Print("no Debounce");
		canBeHit = false;
		EmitSignal(Player.SignalName.PlayerHit, 0.5);


		EmitSignal(Player.SignalName.triggerAnimation, AnimNames[5]);
		GD.Print("takeDamage");
		health -= damage;
		GD.Print("hp: " +health);	
		if(health <= 0){
			// QueueFree();
			// EmitSignal(SignalName.PlayerDied);
			EmitSignal(Player.SignalName.triggerAnimation, AnimNames[6]);
		}
		EmitSignal(Player.SignalName.HealthChanged, health);		
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void activateShield(){
		if (!hasShield || isShieldDestroyed){
			isShieldActive = false;
			return;
		}

		isShieldActive = !isShieldActive;
		shieldSprite.Visible = isShieldActive;
		if (isShieldActive){
			GD.Print("Shield Active");
		} else {
			GD.Print("Shield Deactivated");
		}
	}

	public void handleShieldRegen(){
		if (!hasShield){
			isShieldActive = false;
			return;
		}
		if (isShieldDestroyed){
			shieldHealth += shieldRegenRate/2;
		} else {
			shieldHealth += shieldRegenRate;
		}
		if (isShieldDestroyed && shieldHealth >= shieldRespawnThreshold){
			isShieldDestroyed = false;
		}
		if (shieldHealth > shieldMaxHealth){
			shieldHealth = shieldMaxHealth;
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);

		handleShieldRegen();
		
        
    }

	public void ShieldReturned(){
		hasShield = true;
		shieldHealth = shieldHealth /2;
	}

	public void toggleShield(){
		Rpc("activateShield");
	}

}
