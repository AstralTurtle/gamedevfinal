using Godot;
using System;

public partial class DefaultAnimationController : AnimatedSprite2D
{
	String[] AnimNames = new String[] {""};


	public override void _Ready()
	{
		// Connect the animation finished signal to the animation finished function

	}

	public void goIdle(){
		Play("idle");
	}

		public void triggerAnim(string animName){
		if (Array.IndexOf(AnimNames, animName) == -1)
		{
			
			return;
		}
		if (Animation == animName)
		{
		
			return;
		}
		PlayAnimation(animName);
	}

	public void setLookDirection(Vector2 dir){
		if (!IsMultiplayerAuthority()) return;
		if (dir.X < 0)
		{
			FlipH = true;
		}
		else
		{
			FlipH = false;
		}
	}

	public Vector2 getLookDirection(){
		
		// get mouse position
		Vector2 mpos = GetViewport().GetMousePosition();

		// get player position
		Vector2 ppos = GetGlobalTransformWithCanvas().Origin;

		// get direction
		Vector2 dir = mpos - ppos;
		dir = dir.Normalized();
	
		return dir;
	}


    public override void _PhysicsProcess(double delta)
    {
		if (!IsMultiplayerAuthority()) return;
		setLookDirection(getLookDirection());
        // base._PhysicsProcess(delta);
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
