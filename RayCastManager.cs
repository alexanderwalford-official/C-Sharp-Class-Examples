using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastManager : MonoBehaviour
{
    // this script will be used to check if a player wants to learn more about an object (right click)
    // or wants to interact with it by pickig it up (left click)

    public Text inspectiontext;
    bool rpressed = false;
    bool lpressed = false;

    // Update is called once per frame
    void Update()
    {
            // left click (pick up)
            if (Input.GetMouseButton(0) && lpressed == false)
            {
                lpressed = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.gameObject.GetComponent<itemdata>().itemtype == "inspect")
                {
                }
                else
                {
                    pickup(hit.collider.gameObject);
                }
            }
            // right click (learn)
            if (Input.GetMouseButton(1) && rpressed == false)
            {
                rpressed = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                inspect(hit.collider.gameObject);
                if (hit.collider.gameObject.GetComponent<itemdata>().itemtype == "pickup")
                {
                }
                else
                {
                    inspect(hit.collider.gameObject);
                }
            }
        
    }

    void pickup (GameObject item)
    {
        if (item.gameObject.GetComponent<itemdata>().itemtype == "pickup")
        {           
            inspectiontext.text = item.gameObject.GetComponent<itemdata>().inspectiontext;
            StartCoroutine(ClearText());
            item.gameObject.SetActive(false);
        }
    }

    void inspect (GameObject item)
    {
        if (item.gameObject.GetComponent<itemdata>().itemtype == "inspect")
        {
            inspectiontext.text = item.gameObject.GetComponent<itemdata>().inspectiontext;
            StartCoroutine(ClearText());
        }
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSecondsRealtime(5);
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(ClearText());
        }
        else if (Input.GetMouseButton(1))
        {
            StartCoroutine(ClearText());
        }
        else
        {
            inspectiontext.text = "";
            rpressed = false;
            lpressed = false;
        }
    }

}
