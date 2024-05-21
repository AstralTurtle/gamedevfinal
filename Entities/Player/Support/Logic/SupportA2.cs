using Godot;
using System;

public partial class SupportA2 : Node2D
{
	[Export]
	PackedScene potion;

	[Export]
	public Texture2D[] textures = new Texture2D[3];

	// 0 = healing, 1 = speed, 2 = haste
	public void OnActivated(){
		int type = GD.RandRange(0,2);
		GD.Print("Type: "+type);

		Rpc("OnActivatedRPC", type);


	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC(int type){
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();

		Potion newpot = potion.Instantiate<Potion>();
		newpot.setType(type);
		newpot.Position = new Vector2(player.Position.X, player.Position.Y - 50);
		newpot.changeSprite(textures[type]);
		GetTree().Root.GetNode<Node2D>("Game").AddChild(newpot ,true);
		
		
	}

}
