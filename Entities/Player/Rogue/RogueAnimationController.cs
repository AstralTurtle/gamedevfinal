	using Godot;
using System;

public partial class RogueAnimationController : AnimatedSprite2D
{
	String[] AnimNames = new String[] {"idle", "run", "jump_idle", "jump_run", "land", "hit", "death"};

	public override void _Ready()
	{
		// Connect the animation finished signal to the animation finished function
		// AnimationFinished += goIdle;
	}

	public void goIdle(){
		Play("idle");
	}

	public void triggerAnim(string animName){
		// GD.Print("playing animation: " + animName);
		if (Array.IndexOf(AnimNames, animName) == -1)
		{
			// GD.Print("nevermind big bro");
			return;
		}
		if (Animation == animName)
		{
			// GD.Print("already playing");
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

	public override void _Input(InputEvent @event)
    {		
		if (!IsMultiplayerAuthority()) return;
		if (@event.IsAction("ui_down")){
			
			// GD.Print("ui_accept");
					// Rpc("takeDamage", 10.0f);

		}
        // base._Input(@event);
    }	

    public override void _PhysicsProcess(double delta)
    {
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
