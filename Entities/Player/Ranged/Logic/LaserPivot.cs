using Godot;
using System;

public partial class LaserPivot : Node2D
{
    bool isLaser = false;

    [Export]
    float firingRotSpeed = 0.2f;



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!IsMultiplayerAuthority()) return;

        if (isLaser)
        {
            //  GD.Print("pivoting");
            Vector2 mpos = GetViewport().GetMousePosition();
            Vector2 dir = mpos - GetGlobalTransformWithCanvas().Origin;
            dir = dir.Normalized();


           
                float ang = GetAngleFromDirection(dir);

                // GD.Print("ang:"+ ang);
                // GD.Print("b4"+Rotation);
                Rotation = ang;
                // GD.Print("aft"+Rotation);
            

        }
    }
    private float GetAngleFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.Y, direction.X);
    }

    public void SetMode(bool mode)
    {
        isLaser = !mode;
    }
}