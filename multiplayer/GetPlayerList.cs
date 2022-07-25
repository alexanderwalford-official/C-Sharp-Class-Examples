using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerList : MonoBehaviour
{
    public Text PlayerLobbyList;
    public string apiKey = "550039706949";
    public string lobbyid = "lobby1234";
    public int RefreshInterval = 5000;

    private static readonly HttpClient client = new HttpClient(); // the HTTP client object

    // Update is called once per frame
    void Start()
    {
        // get the player list
        StartAsync();
    }

    async void StartAsync()
    {
        // clear the list
        PlayerLobbyList.text = "";

        try
        {
            // get the lobby list

            var values = new Dictionary<string, string> {
                { "lobbyid", lobbyid }, // lobby ID
                { "apikey", apiKey } // the API key needs to be provided here
            };

            var content = new FormUrlEncodedContent(values); // set the POST content from our variables
            var response = await client.PostAsync("https://renovatesoftware.com/API/getplayerlist/ ", content); // send the post asyncronously

            string responseString = response.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

            List<string> lobbydata = responseString.Split("|").ToList();
            
            foreach (string p in lobbydata)
            {
                PlayerLobbyList.text = PlayerLobbyList.text + "\n" + p;
            }
            
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
            PlayerLobbyList.text = "COULD NOT GET DATA, PLEASE TRY AGAIN LATER.";
        }

        await Task.Delay(RefreshInterval);
        StartAsync();
    }


}
