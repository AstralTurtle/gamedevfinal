using Godot;
using System;
using System.Text;

public partial class HostHelper : HttpRequest
{
    const string APILINK = "https://webserver-zgik.onrender.com/ip/";

    [Signal]
    public delegate void OnCodeReceivedEventHandler(string code);

    public override void _Ready()
    {
        RequestCompleted += onCompleted;

        // base._Ready();
    }

    void getCode(String ip)
    {
        GD.Print("Getting code for ip: " + ip);
        string json = "";
        string[] headers = new string[] { "Content-Type: application/json" };
        Request(APILINK + ip, headers, HttpClient.Method.Post, json);
    }

    void onCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        GD.Print("onCompleted");
        if (responseCode != 200)
        {
            GD.Print("Error: " + responseCode);
            return;
        }

        var json = Encoding.UTF8.GetString(body);

        GD.Print(json);
        EmitSignal(nameof(OnCodeReceived), json);
    }
}
