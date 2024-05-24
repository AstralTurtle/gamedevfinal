using Godot;
using System;

public partial class SupportPrimaryAttack : Node2D
{
	[Export]
	PackedScene primaryAttack;
	
	[Signal]
	public delegate void PulseAriaEventHandler();

	public void OnActivated() {
		Vector2 mpos = GetViewport().GetMousePosition();
		//GD.Print(mpos);

		Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
		dir = dir.Normalized();
		Rpc("OnActivatedRPC", dir);

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC(Vector2 dir){
		SupportAttack attack = primaryAttack.Instantiate<SupportAttack>();
		Node2D player = GetParent<Node2D>().GetParent<Node2D>();

		attack.TreeExited += GetParent<AbilityInput>().resetPrimaryDebounce;
		attack.SuccessfulHit += OnSupportAttackSuccessfulHit;

		attack.Position += new Vector2(25, 25) * dir;


		

		attack.setDir(dir);


		GetTree().Root.GetNode<Node2D>("Game").AddChild(attack, true);
	}
	
	public void OnSupportAttackSuccessfulHit()
	{
		EmitSignal(SignalName.PulseAria);
	}
	



	
}
