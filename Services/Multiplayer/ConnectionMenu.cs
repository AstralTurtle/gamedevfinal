using Godot;
using System;

public partial class ConnectionMenu : Control
{
    [Signal]
    public delegate void OnHostEventHandler();

    [Signal]
    public delegate void OnJoinEventHandler(String code);

    [Signal]
    public delegate void onHostLocalEventHandler();

    [Signal]
    public delegate void onJoinLocalEventHandler(String ip);

    // Attempt to host a server
    public void OnHostPressed()
    {
        EmitSignal(SignalName.OnHost);
    }

    public void onHostLocalPressed(){
        EmitSignal(SignalName.onHostLocal);
    }

    public void onJoinLocalPressed(string ip){
        GD.Print("attempt local join");
        EmitSignal(SignalName.onJoinLocal, ip);
    }


    public void openLobby()
    {
        Visible = false ;
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
