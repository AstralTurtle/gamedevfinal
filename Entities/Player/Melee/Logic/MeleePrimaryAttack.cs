using Godot;
using System;

public partial class MeleePrimaryAttack : Node2D
{
	[Export]
	PackedScene primaryAttack;
	public  override void _Ready() {
		
	}

	public void OnActivated() {
        Vector2 mpos = GetViewport().GetMousePosition();
		//GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();
		Rpc("OnActivatedRPC", dir);
		
	}

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]

	public void OnActivatedRPC(Vector2 dir){
        SwordSwing swing = primaryAttack.Instantiate<SwordSwing>();
        MeleePlayer player = (MeleePlayer)GetParent<Node2D>().GetParent<Node2D>();
        StatManager sm = player.GetNode<StatManager>("StatManager");
        swing.setDamage(sm.getDamage());
        swing.Position = player.Position + new Vector2(25, 15) * dir;
        swing.Rotation = player.Position.AngleToPoint(swing.Position);
        swing.RotationDegrees += 90;
        swing.TreeExited += GetParent<AbilityInput>().resetPrimaryDebounce;
        GetTree().Root.GetNode<Node2D>("Game").AddChild(swing ,true);
        swing.Reparent(player);
        


        
	}

	



	public void changeWeapon(PackedScene newWeapon){
		primaryAttack = newWeapon;
	}
}
