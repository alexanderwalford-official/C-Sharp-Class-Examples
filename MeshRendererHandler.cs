using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MeshRendererHandler : MonoBehaviour
{
    GameObject player;
    string settingsprefile = @"C:\\ProgramData\RenovateSoftware\SD-settings.pref";
    public bool AllowMeshRendererUnloading;
    public int RenderDistance = 40;

    private void OnEnable()
    {
        Start();
    }

    void Start()
    {
        string data = File.ReadAllText(settingsprefile).ToString();
        if (data.Contains("AllowMeshRendererUnloading=true")) {
            AllowMeshRendererUnloading = true;

            // now get the render distance
            string[] split = data.Split("\n");
            foreach (string s in split)
            {
                if (s.Contains("RenderDistance="))
                {
                    RenderDistance = int.Parse(s.Replace("RenderDistance=", ""));
                }
            }
        }
        else
        {
            AllowMeshRendererUnloading = false;
        }
    }

    void Update()
    {
        try
        {
            player = GameObject.Find("Player");
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist > RenderDistance)
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        catch
        {
        }
    }
}
