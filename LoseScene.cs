using Godot;
using System;

public partial class LoseScene : Control
{
	CurrencyManager cm;
	int newgems = 0;
	int coins = 0;
	Label l;

	Callable func;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager game = GetNodeOrNull<GameManager>("/root/Game");
		if (game != null){
			game.Unload();
		}
		Node lobby= GetNodeOrNull("/root/Lobby");
		GD.Print("lobby: " + lobby);
		lobby.PrintTree();
		GD.Print(lobby.GetType());
		if (lobby != null){
			GD.Print("leaving lobby");
			lobby.Call("exitLobby");
		}

		cm = GetNode<CurrencyManager>("/root/CurrencyManager");
		int[] destructure = cm.ConvertCurrency();
		cm.writeGems();
		coins = destructure[0];
		int newgems = destructure[1];
		l = GetNode<Label>("CurrencyEarned");
		l.Text = "0";
	func = new Callable(this, "SetText");
	
		// tween to count up the currency
		Tween t = CreateTween();
		t.TweenMethod(func, 0, newgems, 2);
		t.Finished += showGems;
	}

	public void Menu(){
		GetTree().ChangeSceneToFile("res://MainMenu.tscn");
		QueueFree();
	
	}


	public void showGems(){
		Label g = GetNode<Label>("Gems");
		g.Text = "Gems: " + CurrencyManager.gems;
		g.Visible = true;
	}

	public void SetText(int num){
		l.Text = coins.ToString() + " -> " + num.ToString(); 
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
