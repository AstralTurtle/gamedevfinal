using Godot;
using System;
using System.Collections;

public partial class Shop : Control
{
	ItemList itemList;

	[Export]
	PackedScene statItemScene;

	[Export]
	bool testMode = false;

	[Export]
	int numOptions = 4;

	StatItem[] items;

	bool isActive = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemList = GetNode<ItemList>("ItemList");
		refreshShop();
	}

	public void refreshButton()
	{
		if (testMode)
		{
			refreshShop();
			return;
		}
		CurrencyManager currencyManager = GetNode<CurrencyManager>("/root/CurrencyManager");
		if (currencyManager.SpendCoins(1))
		{
			refreshShop();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("open_shop"))
		{
			isActive = !isActive;
			Visible = isActive;
		}
	}

	void refreshShop()
	{
		items = new StatItem[numOptions];
		itemList.Clear();
		for (int i = 0; i < numOptions; i++)
		{
			StatItem item = new StatItem();
			items[i] = item;
			if (item.value > 0)
				itemList.AddItem(item.ToString());
		}
	}

	public void buyItem()
	{
		int index = itemList.GetSelectedItems()[0];
		if (index < 0 || index >= items.Length)
		{
			return;
		}
		CurrencyManager currencyManager = GetNode<CurrencyManager>("/root/CurrencyManager");
		if (currencyManager.SpendCoins(items[index].price))
		{
			StatItem item = items[index];

			item.OnBuy(
				// how do i even get the player
				GetTree().Root
					.GetNode("Game")
					.GetNode<Player>("Player" + GetMultiplayerAuthority())
			);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
