using Godot;
using System;
using System.Text;

public partial class JoinHelper : HttpRequest
{
    const string APILINK = "webserver-zgik.onrender.com";

    [Signal]
    public delegate void OnIpRecievedEventHandler(string ip);

    public override void _Ready()
    {
        RequestCompleted += onCompleted;

        // base._Ready();
    }

    void checkCode(String code)
    {
        GD.Print("Checking code: " + code);
        Request(APILINK + "/code/" + code);
    }

    void onCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        if (responseCode != 200)
        {
            GD.Print("Error: " + responseCode);
            return;
        }

        Godot.Collections.Dictionary json = Json.ParseString(Encoding.UTF8.GetString(body))
            .AsGodotDictionary();
        GD.Print(json);
        // EmitSignal(nameof(OnIpRecieved), );
    }
}
