using Godot;
using System;
using System.Collections;

public partial class Shop : Control
{
    ItemList itemList;

    [Export]
    PackedScene statItemScene;

    [Export]
    bool debug = false;

    [Export]
    int numberOfItems = 4;

    StatItem[] items;

    bool active = false;

    CurrencyManager currencyManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;
        itemList = GetNode<NinePatchRect>("Panel").GetChild<ItemList>(0);
        refreshShop();
        currencyManager = GetNode<CurrencyManager>("/root/CurrencyManager");
        currencyManager.CoinsChanged += showCoins;
        showCoins(currencyManager.coins);
    }

    public void showCoins(int c)
    {
        GD.Print("Coins: " + c);
        GetNode<Label>("Coins/Label").Text = "Coins: " + c;
    }

    public void RefreshButton()
    {
        if (debug)
        {
            refreshShop();
            return;
        }

        if (currencyManager.SpendCoins(1))
        {
            refreshShop();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("open_shop"))
        {
            active = !active;
            Visible = active;
        }
    }

    void refreshShop()
    {
        items = new StatItem[numberOfItems];
        itemList.Clear();
        for (int i = 0; i < numberOfItems; i++)
        {
            StatItem item = new StatItem();

            items[i] = item;
            AddChild(item);
            if (item.percentageValue > 0f || item.rawValue > 0f)
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
        StatItem item = items[index];

        GD.Print(
            item.OnBuy(
                // how do i even get the player
                GetTree().Root
                    .GetNode("Game")
                    .GetNode<Player>("Player" + GetMultiplayerAuthority())
            )
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        GD.Print("Shop: " + GetChildCount());
    }
}
