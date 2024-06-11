using Godot;
using System;

public enum Stat {
	Health,
	Damage,
	Speed,
	Jumps
}

public enum Rarity {
    Common,
    Uncommon,
    Rare,
    Legendary
}

public partial class StatItem : Node
{
	// The price of the item in coins
	public int price = 10;
    
    // The range of the price in coins
    [Export]
	int priceRange = 5;

    // The percentage of the stat
	public float percentageValue = 0f;

    // The raw value of the stat
    public float rawValue = 0f;

    // The ranges of the raw and percentage values based on the rarity
	[Export]
	float[] commonRawValues = { 1f, 2f };
	float[] commonPercentageValues = { 0.05f, 0.1f };
	[Export]
	float[] uncommonRawValues = { 3f, 4f };
	float[] uncommonPercentageValues = { 0.15f, 0.2f };
	[Export]
	float[] rareRawValues = { 4f, 6f };
	float[] rarePercentageValues = { 0.2f, 0.3f };
	[Export]
	float[] legendaryRawValues = { 6f, 10f };
	float[] legendaryPercentageValues = { 0.3f, 0.5f };

    // The stat of the item
	Stat stat;

    // The rarity of the item
    Rarity rarity = Rarity.Common;

	// The chances of obtaining a rarity
	[Export]
	float common = 0.5f;
	[Export]
	float uncommon = 0.2f;
	[Export]
	float rare = 0.09f;
	[Export]
	float legendary = 0.01f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {}

	public StatItem() {
        // Determine the price of the item
        price += GD.RandRange(-priceRange, priceRange);

        // Determine the rarity of the item
        float rarity = GD.Randf();

        // Determine the state of the item
		stat = (Stat) GD.RandRange(0, 3);

        // Determine the mode of the item (percentage, raw, etc)
        float mode = GD.Randf();

		if (stat == Stat.Jumps) mode = 0.2f;

        if (mode < 0.2f) {
            percentageValue = (float) GD.RandRange(CalcPercentageValue(rarity)[0], CalcPercentageValue(rarity)[1]);
            if (percentageValue < 1) QueueFree();
		} else {
            rawValue = CalcStat(stat, (float) GD.RandRange(CalcRawValue(rarity)[0], CalcRawValue(rarity)[1]));
            if (rawValue < 1) QueueFree();
        }

		SetRarityDisplay(rarity);
	}

	float[] CalcRawValue(float rarity) {
		if (rarity < common) return commonRawValues;
		if (rarity < common + uncommon) return uncommonRawValues;
		if (rarity < common + uncommon + rare) return rareRawValues;
		return legendaryRawValues;
	}

	float[] CalcPercentageValue(float rarity) {
		if (rarity < common) return commonPercentageValues;
		if (rarity < common + uncommon) return uncommonPercentageValues;
		if (rarity < common + uncommon + rare) return rarePercentageValues;
		return legendaryPercentageValues;
	}

	public bool OnBuy(Player player) {
		CurrencyManager CurrencyManager = GetNode<CurrencyManager>("/root/CurrencyManager");
		if (!CurrencyManager.SpendCoins(price)) return false;

        if (percentageValue == 0f) {
            Rpc("changeStat", player, (int) stat, rawValue, false);
            return true;
        }

        Rpc("changeStat", player, (int) stat, percentageValue, true);
        return true;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void ChangeStat(Player p, Stat stat, float value, bool mode) {
		if (stat.GetType() == typeof(int)) this.stat = stat;
		StatManager sm = p.GetNode<StatManager>("StatManager");
		if (!mode)
			sm.setFlatMod(stat, value);
		else
			sm.setPercentMod(stat, value);
	}

	public void OnButtonPressed() {
		var player = GetTree().Root.GetNode<Player>("Player");
		if (OnBuy(player)) QueueFree();
	}

	public Rarity SetRarityDisplay(float rarity) {
		if (rarity < common) return Rarity.Common;
		if (rarity < common + uncommon) return Rarity.Uncommon;
		if (rarity < common + uncommon + rare) return Rarity.Rare;
		return Rarity.Legendary;
	}

	public static float CalcStat(Stat stat, float baseValue) {
        switch (stat) {
            case Stat.Health:
                return baseValue * 10;
            case Stat.Damage:
                return baseValue * 10;
            case Stat.Speed:
                return baseValue;
            case Stat.Jumps:
                return Mathf.FloorToInt(baseValue / 5);
        }
        return 0;
	}

	public override string ToString() {
		if (percentageValue == 0f) return "Type: " + stat.ToString() + " | Modifier: " + Math.Round(rawValue) + " | Price:  " + price + " | Rarity: " + rarity.ToString();
		return "Type: " + stat.ToString() + " | Modifier: " + Math.Round(percentageValue * 100) + "% | Price: " + price + " | Rarity: " + rarity.ToString();
	}
}
