using Godot;
using System;

public partial class ConnectionMenu : Control
{
    [Signal]
    public delegate void OnHostEventHandler();

    [Signal]
    public delegate void OnJoinEventHandler(String code);

    // Attempt to host a server
    public void OnHostPressed()
    {
        EmitSignal(SignalName.OnHost);
    }

    public void OnJoinPressed()
    {
        // Get the code from the input field
        LineEdit codeInput = GetNode<LineEdit>("CodeInput");
        String code = codeInput.Text;
        GD.Print(code);
        EmitSignal(SignalName.OnJoin, code);
    }

    public void onSuccessfulConnection()
    {
        Visible = false;
    }
}
