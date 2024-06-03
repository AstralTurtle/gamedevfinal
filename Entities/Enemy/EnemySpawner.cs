using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	PackedScene[] enemyScenes = new PackedScene[4];
	// Called when the node enters the scene tree for the first time.



	public void Spawn(){
		if (!IsMultiplayerAuthority())
		return;	

		Rpc("SpawnEnemy", GD.Randi() % enemyScenes.Length);


	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true )]
	public void SpawnEnemy(int i){
		Enemy enemy = enemyScenes[i].Instantiate<Enemy>();
		enemy.GlobalPosition = GlobalPosition;
		
		GetTree().Root.GetNode("Game").AddChild(enemy, true);
	}

	public override void _EnterTree(){
		CallDeferred("Spawn");
	}
}
