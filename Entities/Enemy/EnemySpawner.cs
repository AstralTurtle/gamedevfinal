using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	PackedScene[] enemyScenes = new PackedScene[4];
	// Called when the node enters the scene tree for the first time.
	public void Spawn(){
		Enemy enemy = enemyScenes[GD.Randi() % enemyScenes.Length].Instantiate<Enemy>();
		enemy.GlobalPosition = GlobalPosition;
		GetTree().Root.GetNode("Game").AddChild(enemy);


	}

	public override void _EnterTree(){
		CallDeferred("Spawn");
	}
}
