using Godot;
using System;

public partial class CurrencyManager : Node
{
    public static int gems { get; private set; }
    public int coins { get; private set; }


    [Export] bool testmode = false;

    uint testID = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        if (testmode)
        {
            testID = (uint)GD.Randi();
        }

     }

    public override void _Process(double delta){
        if (testmode){
            GD.Print(testID);
        }
    }
    public void AddGems(int amount)
    {
        gems += amount;
    }

    public void writeGems()
    {
        using var saveGame = FileAccess.Open("user://currency.save", FileAccess.ModeFlags.Write);
        var jsonString = Json.Stringify(serializeGems());
        saveGame.StoreString(jsonString);
    }

    public void readGems()
    {
        if (!FileAccess.FileExists("user://currency.save"))
        {
            return;
        }

        using var saveGame = FileAccess.Open("user://currency.save", FileAccess.ModeFlags.Read);
        var jsonString = saveGame.GetAsText();
        var data = Json.ParseString(jsonString);
        if (data.GetType() == typeof(Godot.Collections.Dictionary))
        {
            var dictionary = (Godot.Collections.Dictionary)data;
            gems = (int)dictionary["gems"];
        }
    }

    public Godot.Collections.Dictionary<String, Variant> serializeGems()
    {
        return new Godot.Collections.Dictionary<String, Variant>() { { "gems", gems } };
    }

    public void resetCoins()
    {
        coins = 0;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        if (testmode)
        {
            GD.Print("Added " + amount + " coins. Total: " + coins + "for player " + testID);
        }
    }

    public bool SpendGems(int amount)
    {
        if (gems < amount)
        {
            return false;
        }

        gems -= amount;
        return true;
    }

    public bool SpendCoins(int amount)
    {
        if (coins < amount)
        {
            return false;
        }

        coins -= amount;
        return true;
    }
}
