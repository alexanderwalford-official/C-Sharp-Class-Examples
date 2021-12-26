using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelloader : MonoBehaviour
{
    public string levelname;

    void Start()
    {
        SceneManager.LoadScene(levelname);
    }
}
