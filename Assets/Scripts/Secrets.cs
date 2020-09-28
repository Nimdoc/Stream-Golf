using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secrets : MonoBehaviour
{
    public static string clientId = "sedng16v647fczar1jxilurs6icjsm";
    public static string clientSecret = "27uzlrk3v2x5qz52lbu6hs2n8oo1ey";
    public static string botAccessToken = "ezg0rnujoq599t67kfnxddo1eu508z";
    public static string botRefreshToken = "hqseg7lsdo35hcxss1cpibcsb1e6y9yj919e2q1xqwqgvpwez9";

    public const string CLIENT_ID = "CLIENT_ID"; //Your application's client ID, register one at https://dev.twitch.tv/dashboard
	public const string OAUTH_TOKEN = "ezg0rnujoq599t67kfnxddo1eu508z"; //A Twitch OAuth token which can be used to connect to the chat
	public const string USERNAME_FROM_OAUTH_TOKEN = "nimdoc_bot"; //The username which was used to generate the OAuth token
	public const string CHANNEL_ID_FROM_OAUTH_TOKEN = "ezg0rnujoq599t67kfnxddo1eu508z"; //The channel Id from the account which was used to generate the OAuth token
}
