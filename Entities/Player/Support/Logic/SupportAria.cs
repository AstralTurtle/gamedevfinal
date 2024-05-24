using Godot;
using System;
using System.Linq;

public partial class SupportAria : Area2D
{
	Player[] inAria = new Player[4];

	int playerCount = 0;

	[Export]
 	float healAmount = 3.0f;

	[Export]
	float speedboost = 10.0f;

	bool isHealing = true;

	const String color2 = "00ff00";
	const String color1 = "00ffff";

	GpuParticles2D particles;
	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += SupportAriaEntered;
		BodyExited += SupportAriaExited;
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print(GetTree().GetNodesInGroup("players"))	;
		if (!IsMultiplayerAuthority()) return;
		// GD.Print(playerCount);
		
			if (playerCount < 1){
				playerCount = 1;
				
			}
			if (inAria[0] == null){
				inAria[0] = GetParent<Player>();
			}
			foreach (Player player in inAria){
				if (player != null){
					if (isHealing){
					player.triggerHeal(healAmount * (float)delta);
					}
					else {
						player.triggerSpeedBoost(speedboost);
					}
				}
			}
			// GD.Print(inAria);
		



	}

	public void triggerPulseAria(){
		Rpc("pulseAria");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void pulseAria(){
		Player[] players = GetTree().GetNodesInGroup("players").Cast<Player>().ToArray();
		if (isHealing){
			foreach (Player player in players){
				if (player != null){
					if (inAria.Contains<Player>(player)){
						player.triggerHeal(healAmount * 2);
					} else  {
						player.triggerHeal(healAmount);
					}
				}
			}
		}
		else {
			foreach (Player player in players){
				if (player != null){
					if (inAria.Contains<Player>(player)){
						player.triggerSpeedBoost(speedboost * 2);
					} else  {
						player.triggerSpeedBoost(speedboost);
					}
				}
			}
		}

	}


	public void triggerFlip(){
		Rpc("flipAria");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void flipAria(){
		isHealing = !isHealing;
		updateAria();

	}

	public void updateAria(){
		particles.ProcessMaterial.Set("color", isHealing ? color2 : color1);
		particles.Restart();


	}

	// true = healing, false = speed boostocessMaterial.Set("color", isHealing ? color1 : color2);
	public bool getCurrentAria(){
		return isHealing;
	}

	public void SupportAriaEntered(object body)
	{
		if (body is Player)
		{

			Player player = (Player)body;
			if (!inAria.Contains(player)){
							// GD.Print("SupportAriaEntered "+ playerCount);

				inAria[playerCount] = player;	
				
			}
			playerCount++;
			
		}
	}

	public void SupportAriaExited(object body)
	{
		if (body is Player)
		{
			Player player = (Player)body;
			if (inAria.Contains(player)){
				for (int i = 0; i < inAria.Length; i++){
					if (inAria[i] == player){
						inAria[i] = null;
					}
				}
			}
			playerCount--;
			if (playerCount < 1){
				playerCount = 1;
				inAria[0] = GetParent<Player>();
			}	
			
		}
	}



}
