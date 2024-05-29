
using Godot;
using System;

public partial class MeleeA1 : Node2D
{
    [Export]
    PackedScene shieldToss;


	public void OnActivated(){
        Vector2 mpos = GetViewport().GetMousePosition();
		//GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();
		Rpc("OnActivatedRPC", dir);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC(Vector2 dir) {
        Node2D player = GetParent<Node2D>().GetParent<Node2D>();
		GD.Print(player.Name);
		ShieldToss shield = shieldToss.Instantiate<ShieldToss>();
		shield.setPlayer(player);
		
		shield.Position = player.Position + new Vector2(25, 25) * dir;
		

		shield.RotationDegrees = 0;

		// copy player velocity
		// this desyncs the projectile in multiplayer for some reason so :shrug:
		// warp.inheritVelocity((player as CharacterBody2D).Velocity);
        MeleePlayer mp = (MeleePlayer)player;

        float dmg = mp.ThrowShield();
        if (dmg == 0){
            shield.QueueFree();
            return;
        }
        shield.setDamage(dmg);

        shield.TreeExited += mp.ShieldReturned;
        

			 	
		
		// Rpc("addToTree",warp);	
		GetTree().Root.GetNode<Node2D>("Game").AddChild(shield ,true);

		
		
		// GD.Print(dir);

		

		shield.throwObj(dir);
	}
}
