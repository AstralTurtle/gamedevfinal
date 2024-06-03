using Godot;
using System;

public partial class InfiniteMode : GameManager
{
	int wave = 1;

	[Export]
	int waveSize = 5;

	[Export]
	float waveDelay = 5;

	public int enemiesdead = 0;

	[Export]
	PackedScene enemySpawnerScene;

	public override void _Ready(){
		base._Ready();
		StartWave();
	}

	public void StartWave()
	{
		for (int i = 0; i < waveSize; i++)
		{
			EnemySpawner enemySpawner = enemySpawnerScene.Instantiate<EnemySpawner>();
			enemySpawner.Position = new Vector2(100 + i * 100, 100);
			AddChild(enemySpawner);
		}

	}	

	public void OnEnemyDied()
	{
		enemiesdead++;
		giveCoins(1);
		if (enemiesdead >= waveSize)
		{
			EndWave();
		}
	}

	

	public void EndWave()
	{
		giveGems(1);
		wave++;
		enemiesdead = 0;
		waveSize += Mathf.RoundToInt(waveSize/2);
		GetTree().CreateTimer(waveDelay).Timeout += StartWave;
	}



}
