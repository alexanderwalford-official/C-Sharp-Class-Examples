using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public string SavesLocation = @"C:\\ProgramData\RenovateSoftware\SD\saves\";
    public Sprite ButtonSprite;

    public void Update()
    {
        // check for scene loader data object
        if (GameObject.FindGameObjectWithTag("SCENE LOADER"))
        {
            // the relevant save needs to be loaded
            GameObject dataobject = GameObject.FindGameObjectWithTag("SCENE LOADER");
            string savename = dataobject.name;

            // load the save into the save manager
            LoadGame(savename);
        }
    }


    public void Start()
    {
        int counter = 0;

        // reset the menu
        GameObject[] targets = GameObject.FindGameObjectsWithTag("UI-ELEMENT");
        foreach(GameObject g in targets)
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
                // print the name of the text file
                GameObject newobj = new GameObject();
                newobj.tag = "UI-ELEMENT";
                newobj.AddComponent<Text>().text = s;
                newobj.name = "TXT-SAVE-" + s;
                newobj.transform.SetParent(this.transform);
                newobj.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                newobj.GetComponent<Text>().resizeTextForBestFit = true;
                newobj.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(300, 100);
                newobj.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, counter * 100, 0);

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
                newobjbtn.GetComponent<Image>().rectTransform.localPosition = new Vector3(200, counter * 100 + 10, 0);

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
                newobjbtn2.GetComponent<Image>().color = new Color(255,0,0);
                newobjbtn2.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                newobjbtn2.GetComponent<Image>().rectTransform.localPosition = new Vector3(100, counter * 100 + 10, 0);

                // add text to button
                GameObject newobjbtntext2 = new GameObject();
                newobjbtntext2.tag = "UI-ELEMENT";
                newobjbtntext2.AddComponent<Text>().text = "DEL";
                newobjbtntext2.name = "text";
                newobjbtntext2.transform.SetParent(newobjbtn2.transform);
                newobjbtntext2.GetComponent<Text>().color = new Color(255, 255, 255);
                newobjbtntext2.GetComponent<Text>().resizeTextForBestFit = true;
                newobjbtntext2.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(50, 50);
                newobjbtntext2.GetComponent<Text>().rectTransform.localPosition = new Vector3(0, 0, 0);
                newobjbtntext2.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

                counter++;
            }
        }
        else
        {
            // create the saves directory
            Directory.CreateDirectory(SavesLocation);
        }


    }

    public void SaveGame()
    {
        if (!Directory.Exists(SavesLocation))
        {
            Directory.CreateDirectory(SavesLocation);
        }

        // save the relevent gameobject data

        // select the needed components from the relevant game objects
        var CheckPointManager = GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>();
        var InventoryManager = GameObject.Find("InventoryMenu").GetComponent<PlayerInventory>();
        
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

        // save a new file, containing the gameobject data
        File.WriteAllText(SavesLocation + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "--" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".save", levelname + "\n/" + checkpointcount.ToString() + "\n/" + RecompiledItemNames + "\n/" + RecompiledItemDescriptions + "\n/" + RecompiledItemCounts);

        // re-render the saves
        Start();
    }

    public void LoadGame(string save)
    {
        // get the data and load it into a string
        string SaveData = File.ReadAllText(SavesLocation + save);

        // split the relevant parts of the string
        string[] data = SaveData.Split("/");

        // seperate the array into seperate strings
        string level = data[0].Replace("\n", "");
        string checkpointcount = data[1].Replace("\n", "");
        string[] ItemNames = data[2].Replace("\n", "").Split(" | ");
        string[] ItemDescriptions = data[3].Replace("\n", "").Split("|");
        string[] ItemCounts = data[4].Replace("\n", "").Split("|");

        // now do the relevant work

        // select the needed components from the relevant game objects
        var CheckPointManager = GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>();
        var InventoryManager = GameObject.Find("InventoryMenu").GetComponent<PlayerInventory>();

        // load data into the checkpoint manager
        
        CheckPointManager.Levelname = level;

        // change the level if not as the same stored
        if (level != SceneManager.GetActiveScene().name)
        {
            // not the same, load it and pass the arguments
            GameObject SceneLoadData = new GameObject();
            SceneLoadData.AddComponent<NoDestoryOnLoad>(); // make it invincible
            SceneLoadData.name = save; // name it the relevant file name
            SceneLoadData.tag = "SCENE LOADER";
            SceneManager.LoadSceneAsync(level);
        }
        else
        {
            CheckPointManager.CheckPointCounter = int.Parse(checkpointcount);

            // load data into the inventory manager

            // decompile the data
            foreach (string s in ItemNames)
            {
                InventoryManager.items.Add(s);
            }
            foreach (string s in ItemDescriptions)
            {
                InventoryManager.itemsDescription.Add(s);
            }
            foreach (string s in ItemCounts)
            {
                InventoryManager.itemsCount.Add(int.Parse(s));
            }
        } 
    }

    public void DeleteSave(string save, GameObject newobjbtntext, GameObject newobjbtn, GameObject newobjbtn2)
    {
        // delete the save
        File.Delete(SavesLocation + save);

        // re-render the saves
        Start();
    }

}
