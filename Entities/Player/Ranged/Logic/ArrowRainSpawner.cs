using Godot;
using System;

public partial class ArrowRainSpawner : Node2D
{
	[Export]
	PackedScene arrowScene;
	[Export]
	int numArrows = 16;

	
	float damage = 0;

	Vector2 bpos = new Vector2();


	public void setDamage(float dmg)
	{
		damage = dmg * 1.5f;
	}
	public void setBasePos(Vector2 pos){
		bpos = pos;
	}


	public void SpawnArrows() {
		GD.Print("ArrowRainSpawner entered tree with base pos: " + GlobalPosition);
		GD.Print("is at right position"+ (GlobalPosition == bpos) );

		Vector2[] posi = new Vector2[numArrows];

		for (int i = 0; i < numArrows; i++)
		{
			// Generate a random position for the arrow
			Vector2 pos = new Vector2();
			pos.X = (float)GD.RandRange(-10,10) + GlobalPosition.X;
			pos.Y = GlobalPosition.Y - 50 - (float)GD.RandRange(-10,10);

			posi[i] = pos;	
		}
		

		Rpc("SpawnArrowsRPC", posi);

		QueueFree();	
	}



	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]	

	public  void SpawnArrowsRPC(Vector2[] posi){
		for (int i = 0; i < numArrows; i++)
		{
			ArrowRainArrow arrow = arrowScene.Instantiate<ArrowRainArrow>();
			arrow.setDamage(damage/numArrows);
			GetTree().Root.GetNode<Node2D>("Game").AddChild(arrow, true);
			arrow.GlobalPosition = posi[i];
		}



	}



}
