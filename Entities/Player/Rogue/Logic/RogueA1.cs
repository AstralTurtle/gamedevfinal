using Godot;
using System;

public partial class RogueA1 : Node2D
{
	[Signal]
	public delegate void IFramesEventHandler(bool res);
	[Signal]
	public delegate void DurationEventHandler(float duration);

	public void OnActivated(){
		Rpc("OnActivatedRPC");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC() {
		EmitSignal(SignalName.IFrames, false);
		EmitSignal(SignalName.Duration, 3f);
		CpuParticles2D effect = GetNode<CpuParticles2D>("CPUParticles2D");
		effect.Emitting = true;
	}
}
