using Godot;
using System;

public partial class Play : Button
{
    [Export]
    PackedScene _scene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += OnPress;
    }

    void OnPress()
    {
        GetTree().ChangeSceneToPacked(_scene);
    }
}
