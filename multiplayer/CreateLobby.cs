using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    public InputField lobbyid;
    public Toggle IsPrivate;
    public InputField password;
    public string apiKey = "550039706949";
    public string gameId = "SD";

    private static readonly HttpClient client = new HttpClient(); // the HTTP client object

    public async void SubmitData()
    {
        var values = new Dictionary<string, string> {
            { "gameid", "SD" }, 
            { "apikey", "550039706949" },
            { "title",  lobbyid.text},
            { "owner", "DEVELOPER" },
            { "isprivate", IsPrivate.isOn.ToString() },
            { "password", password.text}
        };

        var content = new FormUrlEncodedContent(values); // set the POST content from our variables
        var response = await client.PostAsync("https://renovatesoftware.com/API/createlobby/ ", content); // send the post asyncronously

        string responseString = response.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

        if (!responseString.Contains("error"))
        {
            // it worked!
            UnityEngine.Debug.Log("New lobby " + lobbyid.text + " created!");
            
            // load into game mode (change scene)
        }
        else
        {
            // error! inform the player
        }
    }

}
