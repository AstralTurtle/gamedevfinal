using Godot;
using System;

public partial class CurrencyManager : Node
{
    [Signal]
    public delegate void CoinsChangedEventHandler(int coins);

    [Signal]
    public delegate void GemsChangedEventHandler(int gems);

    public static int gems { get; private set; }
    public int coins { get; private set; }

    [Export]
    bool testmode = false;

    uint testID = 0;

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            writeGems();
            // GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
            GetTree().Quit(); // default behavior
        }
    }

    public int[] ConvertCurrency()
    {
        int retCoins = coins;
        int newgems = coins / 5;
        coins = 0;
        gems += newgems;
        EmitSignal(SignalName.GemsChanged, gems);
        EmitSignal(SignalName.CoinsChanged, coins);
        return new int[] { retCoins, newgems };
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode.ToString() == "T" && testmode)
            {
                GD.Print("T Pressed" + gems);
                AddGems(100);
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetTree().AutoAcceptQuit = false;
        if (testmode)
        {
            coins = 100;
            testID = (uint)GD.Randi();
        }
        readGems();
    }

    public override void _Process(double delta)
    {
        if (testmode)
        {
            // GD.Print(testID);
        }
    }

    public void AddGems(int amount)
    {
        gems += amount;
        EmitSignal(SignalName.GemsChanged, gems);
        if (testmode)
        {
            GD.Print("Added " + amount + " gems. Total: " + gems + "for player " + testID);
        }
    }

    public override void _ExitTree()
    {
        writeGems();
        base._ExitTree();
    }

    public void writeGems()
    {
        GD.Print("Writing Gems");
        using var saveGame = FileAccess.Open("user://currency.save", FileAccess.ModeFlags.Write);
        var jsonString = Json.Stringify(serializeGems());
        GD.Print(jsonString);
        saveGame.StoreString(jsonString);
    }

    public void readGems()
    {
        if (!FileAccess.FileExists("user://currency.save"))
        {
            GD.Print("No save file found");
            return;
        }
        GD.Print("Reading Gems");
        using var saveGame = FileAccess.Open("user://currency.save", FileAccess.ModeFlags.Read);
        var jsonString = saveGame.GetAsText();
        var data = Json.ParseString(jsonString);
        var d = data.AsGodotDictionary();
        if (d != null)
        {
            GD.Print("Data is dictionary");
            GD.Print(d);
            GD.Print(d["gems"]);
            gems = (int)d["gems"];
        }
        EmitSignal(SignalName.GemsChanged, gems);
    }

    public Godot.Collections.Dictionary<String, Variant> serializeGems()
    {
        return new Godot.Collections.Dictionary<String, Variant>() { { "gems", gems } };
    }

    public void resetCoins()
    {
        coins = 0;
        EmitSignal(SignalName.CoinsChanged, coins);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        EmitSignal(SignalName.CoinsChanged, coins);
        GD.Print("Added " + amount + " coins. Total: " + coins);
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
        EmitSignal(SignalName.GemsChanged, gems);
        return true;
    }

    public bool SpendCoins(int amount)
    {
        if (coins < amount)
        {
            return false;
        }

        coins -= amount;
        EmitSignal(SignalName.CoinsChanged, coins);

        return true;
    }
}
