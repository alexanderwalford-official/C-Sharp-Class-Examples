using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

public class TranslatePosition : MonoBehaviour
{
    public bool b_GetPosition;
    public bool b_SendPosition;
    public string apiKey = "550039706949";
    public string gameID = "SD";
    public string lobbyID = "lobby1234";
    public int RefreshInterval = 500;

    private static readonly HttpClient client = new HttpClient(); // the HTTP client object

    public async void SendPosition()
    {
        string s_pos = "x:" + this.gameObject.transform.position.x + ",y:" + this.gameObject.transform.position.y + ",z:" + this.gameObject.transform.position.z;

        // send object position to server
        var values = new Dictionary<string, string> {
            { "objectid", this.gameObject.name }, // object ID
            { "lobbyid", lobbyID }, // lobby ID
            { "params", s_pos }, // example value, in this example we're using a boolean
            { "apikey", apiKey } // the API key needs to be provided here
        };

        var content = new FormUrlEncodedContent(values); // set the POST content from our variables
        var response = await client.PostAsync("https://renovatesoftware.com/API/setgameobjectdata/ ", content); // send the post asyncronously

        string responseString = response.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

        if (responseString.Contains("saved"))
        {
            // OK!
        }
        else
        {
            // error!
        }
    }

    public async void GetPosition ()
    {
        // get object position from server

        // send object position to server
        var values = new Dictionary<string, string> {
            { "objectid", this.gameObject.name }, // object ID
            { "lobbyid", lobbyID }, // lobby ID
            { "apikey", apiKey } // the API key needs to be provided here
        };

        var content = new FormUrlEncodedContent(values); // set the POST content from our variables
        var response = await client.PostAsync("https://renovatesoftware.com/API/getgameobjectdata/ ", content); // send the post asyncronously

        string responseString = response.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

        if (responseString.Contains("went wrong"))
        {
            // error!
        }
        else
        {
            // - LOBBY_ID|OBJECT_ID|OBJECT_PARAMS
            List<string> objectdata = responseString.Split("|").ToList();

            // objectdata[2] = x:2324,y:324,z:23452
            List<string> translatedata = objectdata[2].Split(",").ToList();

            // translatedata[0] = x:2324, translatedata[1] = y:324, translatedata[2] = z:23452

            int x = int.Parse(translatedata[0].Replace("x:", ""));
            int y = int.Parse(translatedata[0].Replace("y:", ""));
            int z = int.Parse(translatedata[0].Replace("z:", ""));

            Vector3 newpos = new Vector3(x, y, z);

            this.transform.position = transform.position + newpos;
        }  
    }

    // Start is called before the first frame update
    void Start()
    {
        if (b_SendPosition)
        {
            SendPosition();
        }
        if (b_GetPosition)
        {
            GetPosition();
        }
    }
}
