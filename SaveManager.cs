using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;

public class SaveManager : MonoBehaviour
{
    string SavesLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Renovate Software LTD/Shrieking Darkness/saves/";
    public Sprite ButtonSprite;
    public GameObject InventoryManagerObj;
    public GameObject CheckpointManagerObj;
    public GameObject BackButton;
    public GameObject NextButton;
    public GameObject SaveButton;
    public GameObject Player;
    public string GameVersion = "0.1";
    public Text ResponseText;
    public string apiKey = "550039706949";
    string authfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Renovate Software LTD/Shrieking Darkness/auth.dat";
    int startingpos = 100;
    int SeletedSavePage = 0;
    bool loaded = false;
    bool NotLoading = true;
    bool HasChangedVal = false;
    public bool FoundPlayer = false;
    public bool IsTitleScreen = false;

    private static readonly HttpClient client = new HttpClient(); // the HTTP client object

    private void OnLevelWasLoaded(int level)
    {
        loaded = true;
    }

    public void Update()
    {
        // check for scene loader data object
        if (GameObject.FindGameObjectWithTag("SCENE LOADER"))
        {
            // the relevant save needs to be loaded
            GameObject dataobject = GameObject.FindGameObjectWithTag("SCENE LOADER");
            string savename = dataobject.name.ToString();

            // load the save into the save manager
            LoadGame(savename);
        }
    }

    public void Start()
    {
        if (IsTitleScreen)
        {
            SaveButton.SetActive(false);
        }

        int counter = 0;

        // reset the menu
        GameObject[] targets = GameObject.FindGameObjectsWithTag("UI-ELEMENT");
        foreach (GameObject g in targets)
        {
            GameObject.Destroy(g);
        }

        // check if saves directory exists
        if (Directory.Exists(SavesLocation))
        {
            // render all the saves located in the directory

            List<string> saves = new List<string>();

            DirectoryInfo d = new DirectoryInfo(SavesLocation);

            FileInfo[] Files = d.GetFiles("*.save");

            foreach (FileInfo file in Files)
            {
                saves.Add(file.Name.ToString());
            }

            foreach (string s in saves)
            {
                // create a background for each save
                GameObject bgnewobj = new GameObject();
                bgnewobj.tag = "UI-ELEMENT";
                bgnewobj.name = "SAVE_BG";
                bgnewobj.transform.SetParent(this.transform);
                bgnewobj.AddComponent<RawImage>();
                var rawimg = bgnewobj.GetComponent<RawImage>();
                rawimg.color = new Color(0, 0, 0, 0.3f); // set bg colour
                rawimg.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(1200, 150);
                rawimg.GetComponent<RawImage>().rectTransform.localPosition = new Vector3(0, -counter * 80 - 10 + startingpos, 0);

                // print the name of the text file
                GameObject newobj = new GameObject();
                newobj.tag = "UI-ELEMENT";
                newobj.AddComponent<Text>().text = s;
                newobj.name = "TXT-SAVE-" + s;
                newobj.transform.SetParent(this.transform);
                newobj.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                newobj.GetComponent<Text>().resizeTextForBestFit = true;
                newobj.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(300, 100);
                newobj.GetComponent<Text>().rectTransform.localPosition = new Vector3(-100, -counter * 80 - 25 + startingpos, 0);

                // button 1 - load save

                // create a button to load it
                GameObject newobjbtn = new GameObject();
                newobjbtn.tag = "UI-ELEMENT";
                newobjbtn.AddComponent<Button>();
                newobjbtn.GetComponent<Button>().onClick.AddListener(delegate { LoadGame(s); });
                newobjbtn.AddComponent<Image>();
                newobjbtn.GetComponent<Image>().sprite = ButtonSprite;
                newobjbtn.name = "BTN-SAVE-" + s;
                newobjbtn.transform.SetParent(this.transform);
                newobjbtn.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(150, 100);
                newobjbtn.GetComponent<Image>().rectTransform.localPosition = new Vector3(200, -counter * 80 - 10 + startingpos, 0);

                // add text to button
                GameObject newobjbtntext = new GameObject();
                newobjbtntext.tag = "UI-ELEMENT";
                newobjbtntext.AddComponent<Text>().text = "LOAD";
                newobjbtntext.name = "text";
                newobjbtntext.transform.SetParent(newobjbtn.transform);
                newobjbtntext.GetComponent<Text>().resizeTextForBestFit = true;
                newobjbtntext.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(100, 50);
                newobjbtntext.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, 0, 0);
                newobjbtntext.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                newobjbtntext.GetComponent<Text>().color = new Color(0, 0, 0);


                // button 2 - delete save

                // create a button to load it
                GameObject newobjbtn2 = new GameObject();
                newobjbtn2.tag = "UI-ELEMENT";
                newobjbtn2.AddComponent<Button>();
                newobjbtn2.GetComponent<Button>().onClick.AddListener(delegate { DeleteSave(s, newobjbtntext, newobjbtn, newobjbtn2); });
                newobjbtn2.AddComponent<Image>();
                newobjbtn2.GetComponent<Image>().sprite = ButtonSprite;
                newobjbtn2.name = "BTN-DELETE-" + s;
                newobjbtn2.transform.SetParent(this.transform);
                newobjbtn2.GetComponent<Image>().color = new Color(255, 0, 0);
                newobjbtn2.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                newobjbtn2.GetComponent<Image>().rectTransform.localPosition = new Vector3(100, -counter * 80 - 10 + startingpos, 0);

                // add text to button
                GameObject newobjbtntext2 = new GameObject();
                newobjbtntext2.tag = "UI-ELEMENT";
                newobjbtntext2.AddComponent<Text>().text = "DEL";
                newobjbtntext2.name = "text";
                newobjbtntext2.transform.SetParent(newobjbtn2.transform);
                newobjbtntext2.GetComponent<Text>().color = new Color(255, 255, 255);
                newobjbtntext2.GetComponent<Text>().resizeTextForBestFit = true;
                newobjbtntext2.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(50, 30);
                newobjbtntext2.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, 0, 0);
                newobjbtntext2.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

                counter++;
            }

            if (counter == 0)
            {
                NoSaves();
            }
            else if (counter == 4)
            {
                NextButton.SetActive(true);
            }
        }
        else
        {
            // create the saves directory
            Directory.CreateDirectory(SavesLocation);
            NoSaves();
        }
    }

    public void NoSaves()
    {
        // display text
        GameObject newobj = new GameObject();
        newobj.tag = "UI-ELEMENT";
        newobj.AddComponent<Text>().text = "We couldn't find any saves on your system.";
        newobj.name = "TXT-NOSAVES";
        newobj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        newobj.transform.SetParent(this.transform);
        newobj.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        newobj.GetComponent<Text>().resizeTextForBestFit = true;
        newobj.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(1000, 100);
        newobj.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, 0, 0);
    }

    public void SaveGame()
    {
        // get player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float playerxpos = player.transform.position.x;
        float playerypos = player.transform.position.y;
        float playerzpos = player.transform.position.z;
        string PlayerPosString = playerxpos.ToString() + "|" + playerypos.ToString() + "|" + playerzpos.ToString();

        if (!Directory.Exists(SavesLocation))
        {
            Directory.CreateDirectory(SavesLocation);
        }

        // save the relevent gameobject data

        // select the needed components from the relevant game objects
        var CheckPointManager = CheckpointManagerObj.GetComponent<CheckPointManager>();
        var InventoryManager = InventoryManagerObj.GetComponent<PlayerInventory>();

        // save the data as local variables
        int checkpointcount = CheckPointManager.CheckPointCounter;
        string levelname = CheckPointManager.Levelname;
        List<string> ItemNames = InventoryManager.items;
        List<string> ItemDescriptions = InventoryManager.itemsDescription;
        List<int> ItemCounts = InventoryManager.itemsCount;

        // recompile the data
        string RecompiledItemNames = "";
        string RecompiledItemDescriptions = "";
        string RecompiledItemCounts = "";

        foreach (string s in ItemNames)
        {
            RecompiledItemNames = RecompiledItemNames + s + "|";
        }
        foreach (string s in ItemDescriptions)
        {
            RecompiledItemDescriptions = RecompiledItemDescriptions + s + "|";
        }
        foreach (int i in ItemCounts)
        {
            RecompiledItemCounts = RecompiledItemCounts + i.ToString() + "|";
        }

        string file_name = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "--" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".save";
        string file_data = levelname + "\n/" + checkpointcount.ToString() + "\n/" + RecompiledItemNames + "\n/" + RecompiledItemDescriptions + "\n/" + RecompiledItemCounts + "\n/" + PlayerPosString + "\n/" + GameVersion;

        // save a new file, containing the gameobject data
        File.WriteAllText(SavesLocation + file_name, file_data);

        // re-render the saves
        Start();

        ResponseText.text = "Your game was saved.";

        // check if the auth file exists
        if (File.Exists(authfile))
        {
            // upload the file to the cloud
            UploadSave(file_data, file_name);
        }
    }

    public void LoadGame(string save)
    {
        if (NotLoading)
        {
            if (IsTitleScreen)
            {
                Destroy(GameObject.Find("Music"));
            }

            try
            {
                UnityEngine.Debug.Log("Loading.. " + SavesLocation + save);

                // get the data and load it into a string
                string SaveData = File.ReadAllText(SavesLocation + save);

                // split the relevant parts of the string
                string[] data = SaveData.Split("/");

                // seperate the array into seperate strings
                string level = data[0].Replace("\n", "");
                string checkpointcount = data[1].Replace("\n", "");
                string[] ItemNames = data[2].Replace("\n", "").Split("|");
                string[] ItemDescriptions = data[3].Replace("\n", "").Split("|");
                string[] ItemCounts = data[4].Replace("\n", "").Split("|");
                string[] PlayerPosString = data[5].Replace("\n", "").Split("|");
                string LGameVersion = data[6].Replace("\n", "");

                float PlayerPosX = float.Parse(PlayerPosString[0]);
                float PlayerPosY = float.Parse(PlayerPosString[1]);
                float PlayerPosZ = float.Parse(PlayerPosString[2]);

                Vector3 LoadedPlayerPos = new Vector3(PlayerPosX, PlayerPosY, PlayerPosZ);

                UnityEngine.Debug.Log("Level: " + level);
                UnityEngine.Debug.Log("Checkpoint Count: " + checkpointcount);
                UnityEngine.Debug.Log("Game Version: " + LGameVersion);

                if (LGameVersion != GameVersion)
                {
                    // version mismatch!
                    ResponseText.text = "Game version mismatch! Will attempt to load anyway..";
                }
                else
                {
                    ResponseText.text = "Same game version..";
                }

                // now do the relevant work

                // select the needed components from the relevant game objects
                var CheckPointManager = CheckpointManagerObj.GetComponent<CheckPointManager>();
                var InventoryManager = InventoryManagerObj.GetComponent<PlayerInventory>();

                // load data into the checkpoint manager
                CheckPointManager.Levelname = level;

                try
                {
                    // change the level if not as the same stored
                    if (level != SceneManager.GetActiveScene().name.ToString())
                    {
                        if (NotLoading)
                        {
                            NotLoading = false; // stop this block from repeating itself
                            this.gameObject.name = "LoadDataParser";
                            //this.gameObject.AddComponent<NoDestoryOnLoad>();
                            // not the same, load it and pass the arguments
                            GameObject SceneLoadData = new GameObject();
                            SceneLoadData.AddComponent<NoDestoryOnLoad>(); // make it invincible
                            SceneLoadData.name = save; // name it the relevant file name
                            SceneLoadData.tag = "SCENE LOADER";
                            ResponseText.text = "Loading..";
                            StartCoroutine(LoadYourAsyncScene(level, LoadedPlayerPos, checkpointcount, ItemNames, ItemDescriptions, ItemCounts));
                        }
                    }
                    else
                    {
                        try
                        {
                            CheckPointManager.CheckPointCounter = int.Parse(checkpointcount);
                        }
                        catch
                        {
                            ResponseText.text = "Could not parse string!";
                        }

                        try
                        {
                            // first, remove all items from the inventory
                            foreach (string s in InventoryManager.items)
                            {
                                if (s != "Torch")
                                {
                                    InventoryManager.items.Remove(s);
                                }
                            }
                            foreach (string s in InventoryManager.itemsDescription)
                            {
                                InventoryManager.itemsDescription.Remove(s);
                            }
                            foreach (int s in InventoryManager.itemsCount)
                            {
                                InventoryManager.itemsCount.Remove(s);
                            }

                            // load data into the inventory manager
                            // decompile the data
                            foreach (string s in ItemNames)
                            {
                                try
                                {
                                    InventoryManager.items.Add(s);
                                }
                                catch
                                {
                                    InventoryManager.items.Add("Error");
                                }
                            }
                            foreach (string s in ItemDescriptions)
                            {
                                try
                                {
                                    InventoryManager.itemsDescription.Add(s);
                                }
                                catch
                                {
                                    InventoryManager.itemsDescription.Add("Error");
                                }
                            }
                            foreach (string s in ItemCounts)
                            {
                                try
                                {
                                    InventoryManager.itemsCount.Add(int.Parse(s));
                                }
                                catch
                                {
                                    InventoryManager.itemsCount.Add(1);
                                }
                            }
                            ResponseText.text = "Your game was loaded.";
                            StartCoroutine(HideResponseText());
                        }
                        catch
                        {
                            ResponseText.text = "Some of your items may be missing.";
                            StartCoroutine(HideResponseText());
                        }
                        finally
                        {
                            Player.transform.position = LoadedPlayerPos;
                        }
                    }
                }
                catch
                {
                    UnityEngine.Debug.LogError("Error SV2!");
                    ResponseText.text = "Programming error! Please contact developer.";
                }

            }
            catch (Exception e)
            {
                // error
                UnityEngine.Debug.Log("Failed to load: " + e);
                ResponseText.text = "Failed to load the save.";

                StartCoroutine(HideResponseText());
            }
        }
    }

    public void DeleteSave(string save, GameObject newobjbtntext, GameObject newobjbtn, GameObject newobjbtn2)
    {
        // delete the save locally
        File.Delete(SavesLocation + save);

        if (File.Exists(authfile))
        {
            // delete the save from the cloud
            DeleteCloudSave(save);
        }

        // re-render the saves
        Start();
    }

    public void NextPage()
    {
        SeletedSavePage = SeletedSavePage + 1;
        // implement array position selection
    }

    public void BackPage()
    {
        SeletedSavePage = SeletedSavePage - 1;
    }

    IEnumerator LoadYourAsyncScene(string level, Vector3 PlayerPos, string checkpointcount, string[] ItemNames, string[] ItemDescriptions, string[] ItemCounts)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(level));

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);

        // try to change the gameobject data
        StartCoroutine(PassData(level, PlayerPos, checkpointcount, ItemNames, ItemDescriptions, ItemCounts));
    }

    IEnumerator PassData(string level, Vector3 PlayerPos, string checkpointcount, string[] ItemNames, string[] ItemDescriptions, string[] ItemCounts)
    {
        yield return new WaitForSecondsRealtime(1);
        if (!HasChangedVal)
        {
            try
            {
                // set player position after loading scene
                GameObject CheckPointManager_new = GameObject.Find("CheckPointManager");
                CheckPointManager_new.GetComponent<CheckPointManager>().PlayerPos = PlayerPos;
                CheckPointManager_new.GetComponent<CheckPointManager>().ParseDataFromLoad = true;
                CheckPointManager_new.GetComponent<CheckPointManager>().ItemNames = ItemNames;
                CheckPointManager_new.GetComponent<CheckPointManager>().ItemDescriptions = ItemDescriptions;
                CheckPointManager_new.GetComponent<CheckPointManager>().ItemCounts = ItemCounts;

                // set the inventory
                // select the needed components from the relevant game objects
                var CheckPointManager = GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>();

                try
                {
                    CheckPointManager.CheckPointCounter = int.Parse(checkpointcount);
                }
                catch
                {
                    ResponseText.text = "Could not parse string!";
                }

                // save loaded 
                ResponseText.text = "Save loaded.";
                UnityEngine.Debug.Log("Scene was loaded.");
                HasChangedVal = true;
                Destroy(this.gameObject); // now we can destory this gameobject
            }
            catch
            {
                // failed to find gameobject, not yet ready!
                HasChangedVal = false;
            }
        }
        else
        {
            StartCoroutine(PassData(level, PlayerPos, checkpointcount, ItemNames, ItemDescriptions, ItemCounts));
        }
    }

    public async void UploadSave(string file_contents, string file_name)
    {
        // get the key
        string key = Decrypt(File.ReadAllText(authfile));

        // first, check if folder exists in cloud
        var values = new Dictionary<string, string> {
            { "apikey", apiKey },
            { "key", key },
            { "name", "_SD-SAVES" }
        };

        var content = new FormUrlEncodedContent(values); // set the POST content from our variables
        var response = await client.PostAsync("https://renovatesoftware.com/API/check_folder_exists/", content); // send the post asyncronously
        var responseString = response.Content.ReadAsStringAsync().Result.ToString();// get the server's response

        if (responseString == "Does not exist.")
        {
            ResponseText.text = "Cloud folder does not exist, creating..";
            // folder does not exist, create it
            // first, check if folder exists in cloud
            var values2 = new Dictionary<string, string> {
                { "apikey", apiKey },
                { "key", key },
                { "name", "_SD-SAVES" }
            };

            var content2 = new FormUrlEncodedContent(values2); // set the POST content from our variables
            var response2 = await client.PostAsync("https://renovatesoftware.com/API/create_folder/", content2); // send the post asyncronously
            var responseString2 = response2.Content.ReadAsStringAsync().Result.ToString();// get the server's response
            ResponseText.text = "Cloud folder created.";
        }

        ResponseText.text = "Uploading save file to cloud..";
        // upload the file
        // first, check if folder exists in cloud
        var values3 = new Dictionary<string, string> {
            { "apikey", apiKey },
            { "key", key },
            { "file_name", file_name },
            { "file_content", file_contents },
            { "folder", "_SD-SAVES" },
            { "notes", "Shrieking Darkness save file, automatically generated from the game." }
        };

        var content3 = new FormUrlEncodedContent(values3); // set the POST content from our variables
        var response3 = await client.PostAsync("https://renovatesoftware.com/API/create_file/ ", content3); // send the post asyncronously
        var responseString3 = response3.Content.ReadAsStringAsync().Result.ToString(); // get the server's response

        if (responseString3 == "OK")
        {
            ResponseText.text = "File saved to cloud.";
        }
        else
        {
            ResponseText.text = "Could not save to cloud.";
        }

        StartCoroutine(HideResponseText());
    }

    public async void DeleteCloudSave(string file_name)
    {
        // get the key
        string key = Decrypt(File.ReadAllText(authfile));

        // first, check if folder exists in cloud
        var values = new Dictionary<string, string> {
            { "apikey", apiKey },
            { "key", key },
            { "file_name", file_name },
            { "folder", "_SD-SAVES" }
        };

        var content = new FormUrlEncodedContent(values); // set the POST content from our variables
        var response = await client.PostAsync("https://renovatesoftware.com/API/delete_string_file/", content); // send the post asyncronously
        var responseString = response.Content.ReadAsStringAsync().Result.ToString();// get the server's response

    }

    static public string Decrypt(string data)
    {
        string passPhrase = "gddfasefdf";        // can be any string
        string saltValue = "awdsfgga";        // can be any string
        string hashAlgorithm = "SHA1";             // can be "MD5"
        int passwordIterations = 7;                  // can be any number
        string initVector = "~1B2c3D4e5F6g7H8"; // must be 16 bytes
        int keySize = 256;

        byte[] bytes = Encoding.ASCII.GetBytes(initVector);
        byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
        byte[] buffer = Convert.FromBase64String(data);
        byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
        RijndaelManaged managed = new RijndaelManaged();
        managed.Mode = CipherMode.CBC;
        ICryptoTransform transform = managed.CreateDecryptor(rgbKey, bytes);
        MemoryStream stream = new MemoryStream(buffer);
        CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
        byte[] buffer5 = new byte[buffer.Length];
        int count = stream2.Read(buffer5, 0, buffer5.Length);
        stream.Close();
        stream2.Close();
        return Encoding.UTF8.GetString(buffer5, 0, count);
    }

    IEnumerator HideResponseText()
    {
        yield return new WaitForSecondsRealtime(3);
        ResponseText.text = "";
    }
}
