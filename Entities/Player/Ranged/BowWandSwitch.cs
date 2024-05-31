using Godot;
using System;

public partial class BowWandSwitch : Sprite2D
{
	[Export]
	 Rect2 BOW_RECT = new Rect2(0, 0, 32, 32);
	[Export]
	 Rect2 WAND_RECT = new Rect2(32, 0, 32, 32);

	public void triggerSwitch(bool mode){
		Rpc("SwitchRPC", mode);
	}

	    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void SwitchRPC(bool mode){
		if (mode){
			RegionRect = BOW_RECT;
		}
		else{
			RegionRect = WAND_RECT;
		}
	}


}
