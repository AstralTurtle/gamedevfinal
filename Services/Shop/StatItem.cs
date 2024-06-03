using Godot;
using System;

public enum StatType
{
	Health,
	Damage,
	Speed,
	Jumps
}

public partial class StatItem : Node
{
	// Button buyButton;
	public int price = 10;
	public float value;

	bool percent = false;

	[Export]
	int priceRange = 5;

	[Export]
	float[] commonValues = { 1f, 2f };
	float[] commonPercentValues = { 0.05f, 0.1f };

	[Export]
	float[] uncommonValues = { 3f, 4f };

	float[] uncommonPercentValues = { 0.15f, 0.2f };

	[Export]
	float[] rareValues = { 4f, 6f };
	float[] rarePercentValues = { 0.2f, 0.3f };

	[Export]
	float[] legendaryValues = { 6f, 10f };
	float[] legendaryPercentValues = { 0.3f, 0.5f };

	StatType statType;

	// common, uncommon, rare, legendary, should add up to 1
	[Export]
	float common = 0.5f;

	[Export]
	float uncommon = 0.2f;

	[Export]
	float rare = 0.09f;

	[Export]
	float legendary = 0.01f;

	String rarity = "common";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// buyButton = GetNode<Button>("Button");
	}

	public StatItem()
	{
		// choose a random mode
		percent = GD.Randf() < 0.2f;

		// choose a random type
		var type = GD.RandRange(0, 3);
		statType = (StatType)type;
		if (statType == StatType.Jumps)
			percent = false;

		// set price
		price += GD.RandRange(-priceRange, priceRange);
		float minValue = 0;
		float maxValue = 0;

		// choose a random rarity
		var rarity = GD.Randf();
		if (percent)
		{
			minValue = calculateBasePercentValue(rarity)[0];
			maxValue = calculateBasePercentValue(rarity)[1];
		}
		else
		{
			minValue = calculateBaseFlatValue(rarity)[0];
			maxValue = calculateBaseFlatValue(rarity)[1];
		}

		if (!percent)
		{
			value = calcStat(statType, (float)GD.RandRange(minValue, maxValue));
		}
		else
		{
			value = (float)GD.RandRange(minValue, maxValue);
		}
		if (value < 1)
		{
			// delete this item
			QueueFree();
		}

		setRarityDisplay(rarity);
	}

	float[] calculateBaseFlatValue(float rarity)
	{
		if (rarity < common)
		{
			return commonValues;
		}
		else if (rarity < common + uncommon)
		{
			return uncommonValues;
		}
		else if (rarity < common + uncommon + rare)
		{
			return rareValues;
		}
		else
		{
			return legendaryValues;
		}
	}

	float[] calculateBasePercentValue(float rarity)
	{
		if (rarity < common)
		{
			return commonPercentValues;
		}
		else if (rarity < common + uncommon)
		{
			return uncommonPercentValues;
		}
		else if (rarity < common + uncommon + rare)
		{
			return rarePercentValues;
		}
		else
		{
			return legendaryPercentValues;
		}
	}

	public bool OnBuy(Player player)
	{
		CurrencyManager CurrencyManager = GetNode<CurrencyManager>("/root/CurrencyManager");
		if (CurrencyManager.SpendCoins(price))
		{
			Rpc("changeStat", player, (int)statType, value, percent);
			return true;
		}
		return false;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void changeStat(Player p, StatType statType, float val, bool mode)
	{
		if (statType.GetType() == typeof(int))
		{
			statType = (StatType)statType;
		}
		StatManager sm = p.GetNode<StatManager>("StatManager");
		if (!mode)
			sm.setFlatMod(statType, val);
		else
			sm.setPercentMod(statType, val);
	}

	public void OnButtonPressed()
	{
		var player = GetTree().Root.GetNode<Player>("Player");
		if (OnBuy(player))
		{
			QueueFree();
		}
	}

	public String setRarityDisplay(float rare)
	{
		if (rare < common)
		{
			rarity = "common";
		}
		else if (rare < common + uncommon)
		{
			rarity = "uncommon";
		}
		else if (rare < common + uncommon + rare)
		{
			rarity = "rare";
		}
		else
		{
			rarity = "legendary";
		}
		return rarity;
	}

	public float calcStat(StatType stat, float baseval)
	{
		switch (stat)
		{
			case StatType.Health:
				return getHealthValue(baseval);
			case StatType.Damage:
				return getDamageValue(baseval);
			case StatType.Speed:
				return getSpeedValue(baseval);
			case StatType.Jumps:
				return getJumpValue(baseval);
		}
		return 0;
	}

	public float getHealthValue(float baseval)
	{
		return baseval * 10;
	}

	public float getSpeedValue(float baseval)
	{
		return baseval * 10;
	}

	public float getDamageValue(float baseval)
	{
		return baseval;
	}

	public override String ToString()
	{
		if (percent)
			return statType + " " + Math.Round(value * 100) + "% for: " + price + " | " + rarity;
		else

			return statType + " " + Math.Round(value) + " for: " + price + " | " + rarity;
	}

	public int getJumpValue(float baseval)
	{
		return Mathf.FloorToInt(baseval / 5);
	}

	// (baseval / 5
}
