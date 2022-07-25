using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using System.Linq;

public class GetLobbies : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient(); // the HTTP client object

    public string apiKey = "550039706949";
    public string gameID = "SD";
    public Text LobbyTextList;
    public int RefreshInterval = 5000;

    void Start()
    {
        StartAsync();
    }

    // Start is called before the first frame update
    async void StartAsync()
    {
        // clear the list
        LobbyTextList.text = "";

        try
        {
            // get the lobby list

            var values = new Dictionary<string, string> {
                { "gameid", gameID }, // game ID
                { "apikey", apiKey } // the API key needs to be provided here
            };

            var content = new FormUrlEncodedContent(values); // set the POST content from our variables
            var response = await client.PostAsync("https://renovatesoftware.com/API/getlobbylist/ ", content); // send the post asyncronously
            
            string responseString = response.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

            List<string> lobbydata = responseString.Split("|").ToList();

            foreach (string s in lobbydata)
            {
                if (s != "")
                {
                    List<string> thisdata = s.Split("_").ToList();
                    // thisdata[0] = lobby1234
                    // thisdata[1] = PLAYERS:10
                    // thisdata[2] = IS-PRIVATE:1
                    // thisdata[3] = PASSWORD:mypassword123
                    // thisdata[4] = OWNER:player123
                    // thisdata[5] = GAME-MODE:TYPE

                    if (thisdata[2].Contains("True"))
                    {
                        // is private
                        LobbyTextList.text = LobbyTextList.text + "\nLOBBY ID:" + thisdata[0] + ", " + thisdata[1] + ", TYPE:PRIVATE, " + thisdata[4] + ", " + thisdata[5];
                    }
                    else
                    {
                        // is public
                        LobbyTextList.text = LobbyTextList.text + "\nLOBBY ID:" + thisdata[0] + ", " + thisdata[1] + ", TYPE:PUBLIC, " + thisdata[4] + ", " + thisdata[5];
                    }
                }
            }

        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
            LobbyTextList.text = "COULD NOT GET DATA, PLEASE TRY AGAIN LATER.";
        }

        await Task.Delay(RefreshInterval);
        StartAsync();
    }
}
