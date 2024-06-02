using Godot;
using System;

public partial class CurrencyManager : Node
{
    public static int gems { get; private set; }
    public int coins { get; private set; }

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

    public void AddCoins(int amount)
    {
        coins += amount;
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
