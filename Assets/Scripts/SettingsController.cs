using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public InputField channelUsername;
    public InputField botUsername;
    public InputField oauthToken;

	private Client _client;

    // Start is called before the first frame update
    void Start()
    {
        channelUsername.text = PlayerPrefs.GetString("channelUsername", "");
        botUsername.text = PlayerPrefs.GetString("botUsername", "");
        oauthToken.text = PlayerPrefs.GetString("oauthToken", "");
    }

    public void saveChannelUsername()
    {
        PlayerPrefs.SetString("channelUsername", channelUsername.text);
        PlayerPrefs.Save();
    }

    public void saveBotUsername()
    {
        PlayerPrefs.SetString("botUsername", botUsername.text);
        PlayerPrefs.Save();
    }

    public void saveOauthToken()
    {
        PlayerPrefs.SetString("oauthToken", oauthToken.text);
        PlayerPrefs.Save();
    }
    
    public void testConnection()
    {
        // To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
		// Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
		// This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
		// Application.runInBackground = true;

		//Create Credentials instance
		ConnectionCredentials credentials = new ConnectionCredentials(PlayerPrefs.GetString("botUsername", ""), PlayerPrefs.GetString("oauthToken", ""));

		// Create new instance of Chat Client
		_client = new Client();

		// Initialize the client with the credentials instance, and setting a default channel to connect to.
		_client.Initialize(credentials, PlayerPrefs.GetString("channelUsername", ""));

		_client.OnConnected += onConnected;
		_client.OnJoinedChannel += onJoinedChannel;
        _client.OnConnectionError += error;

		// Connect
		_client.Connect();
    }

    private void onConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
    {
        Debug.Log("Connected");
    }

    private void onJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
		_client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
	}

    private void error(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
    {
        Debug.Log("Not Connected");
    }
}
