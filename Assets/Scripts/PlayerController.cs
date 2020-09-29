using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] //[SerializeField] Allows the private field to show up in Unity's inspector. Way better than just making it public
	private string _channelToConnectTo = "nimdoc";
	private Client _client;

    public float hitForceMultiplier = 100.0f;
    public float Speed = 100.0f;
	private Rigidbody body;

    GameObject generator;
    GameObject gui;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindWithTag("Generator");
        gui = GameObject.FindWithTag("GUI");
        body = GetComponent<Rigidbody>();

		// To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
		// Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
		// This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
		// Application.runInBackground = true;

		//Create Credentials instance
		ConnectionCredentials credentials = new ConnectionCredentials(Secrets.USERNAME_FROM_OAUTH_TOKEN, Secrets.OAUTH_TOKEN);

		// Create new instance of Chat Client
		_client = new Client();

		// Initialize the client with the credentials instance, and setting a default channel to connect to.
		_client.Initialize(credentials, _channelToConnectTo);

		// Bind callbacks to events
		_client.OnConnected += OnConnected;
		_client.OnJoinedChannel += OnJoinedChannel;
		_client.OnMessageReceived += OnMessageReceived;
		_client.OnChatCommandReceived += OnChatCommandReceived;

		// Connect
		_client.Connect();
    }

    // FixedUpdate runs once before each tick of the physics system. 
    void FixedUpdate()
    {
        float horizontalForce = Input.GetAxis("Horizontal") * Speed;
        float verticalForce = Input.GetAxis("Vertical") * Speed;

		body.AddForce(new Vector3(horizontalForce, 0, verticalForce));

        if(this.transform.position.y < -10) {
            generator.GetComponent<CourseGenerator>().resetPlayer();
        }

        // if(body.velocity.magnitude < 2.5f && Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Vertical") < 0.1f) {
        //     body.velocity = new Vector3(0,0,0);
        // }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			_client.SendMessage(_channelToConnectTo, "I pressed the space key within Unity.");
		}
    }
    
    private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");

		if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
			Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
	}

	private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
		_client.SendMessage(e.Channel, "I just joined the channel! PogChamp");
	}

	private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
	{
		Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
	}

	private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
	{
		switch (e.Command.CommandText)
		{
			case "hello":
				_client.SendMessage(e.Command.ChatMessage.Channel, $"Hello {e.Command.ChatMessage.DisplayName}!");
				break;
			case "about":
				_client.SendMessage(e.Command.ChatMessage.Channel, "I am a Twitch bot running on TwitchLib!");
				break;
            case "hit":

                if(e.Command.ArgumentsAsList.Count == 2) {
                    int angle, force;
                    bool angleValid = int.TryParse(e.Command.ArgumentsAsList[0], out angle);
                    bool forceValid = int.TryParse(e.Command.ArgumentsAsList[1], out force);

                    if(angleValid && forceValid && Mathf.Abs(force) <= 10) {

                        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * hitForceMultiplier * force;
                        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * hitForceMultiplier * force;

                        body.AddForce(ycomponent, 0, xcomponent, ForceMode.VelocityChange);

                        gui.GetComponent<GuiController>().setLatestHit(e.Command.ChatMessage.Username + ": " + angle + " degrees, " + force + " power");
                        gui.GetComponent<GuiController>().addHitCount();
                    }
                }
                break;
			default:
				_client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
				break;
		}
	}
}
