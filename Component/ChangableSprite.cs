using Godot;
using System;

public partial class ChangableSprite : Sprite2D
{
	public void setTexture(Texture2D texture){
		Texture = texture;
	}
}
