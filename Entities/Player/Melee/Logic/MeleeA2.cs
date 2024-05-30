
using Godot;
using System;

public partial class MeleeA2 : Node2D
{
	

	public void OnActivated(){
		Rpc("OnActivatedRPC");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void OnActivatedRPC() {
		
	}
}
