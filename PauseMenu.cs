using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject playerCam;
    public GameObject PauseCam;
    public KeyCode key;
    bool ispaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Cursor.lockState = CursorLockMode.None;
            playerCam.SetActive(false);
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            PauseCam.SetActive(true);
            ispaused = true;
        }
    }
}
