using Godot;
using System;

public partial class EnemyAnimHandler : AnimatedSprite2D
{
	[Export]
	String[] AnimNames = new String[7];


	public override void _Ready()
	{
		// Connect the animation finished signal to the animation finished function

	}



		public void triggerAnim(string animName){
			
		if (Array.IndexOf(AnimNames, animName) == -1)
		{
			// GD.Print("Animation not found: " + animName);
			return;
		}
		if (Animation == animName)
		{
			// GD.Print("Animation already playing: " + animName);
			return;
		}
		// GD.Print("triggering anim: " + animName);
		PlayAnimation(animName);
	}

	public void SetLookDir(Vector2 lookDir){
		// GD.Print(lookDir + "looking at");
		if (lookDir.X < 0)
		{
			FlipH = true;
		}
		else
		{
			FlipH = false;
		}
	}




   
	public void PlayAnimation(String animName)
	{
		// If the animation is already playing, don't play it again
		if (animName == Animation)
		{
			return;
		}

		// If the animation is not in the list of animations, don't play it
		if (Array.IndexOf(AnimNames, animName) == -1)
		{
			return;
		}

		// Play the animation
		Play(animName);
	}
}
