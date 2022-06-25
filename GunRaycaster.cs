using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRaycaster : MonoBehaviour
{
    // implement raycasting system for when holding gun
    public AudioSource shotsoundfx;
    public ParticleSystem MuzzleFlash;
    public GameObject PlayerInventory;
    public GameObject MeshObject;
    int ammo = 0;

    void Update()
    {
        // get the string arrays for the player's inventory
        List<string> items = PlayerInventory.GetComponent<PlayerInventory>().items;
        List<int> itemcounts = PlayerInventory.GetComponent<PlayerInventory>().itemsCount;

        if (ammo == 0)
        {
            // localised counter for indexing array position
            int counter = 0;

            // look for mag in string
            foreach (string s in items)
            {
                if (s.Contains("Mag"))
                {
                    // can't be negative at the pointer
                    if (itemcounts[counter] > 0)
                    {
                        // 1 magazine contains 10 bullets, multiply it by 10
                        ammo = ammo + itemcounts[counter] * 10;
                    }

                }
                counter++;
            }
        }
        

        // set the mesh renderer status the same as the boolean that informs us if we are holding the gun
        MeshObject.GetComponent<SkinnedMeshRenderer>().enabled = gameObject.GetComponentInParent<ChangeAnimState>().isAiming;

        if (Input.GetKeyDown(KeyCode.Mouse0) && ammo !=0 && gameObject.GetComponentInParent<ChangeAnimState>().isAiming)
        {
            // ammo
            ammo = ammo - 1;

            // shoot gun

            // every 10 ammo used, remove a mag from the inventory
            if (ammo.ToString().EndsWith("0"))
            { 
                // localised counter for indexing array position
                int counter2 = 0;

                // look for mag in string
                foreach (string s in items)
                {
                    if (s.Contains("Mag"))
                    {
                        // remove 1 mag
                        PlayerInventory.GetComponent<PlayerInventory>().itemsCount.Insert(counter2, PlayerInventory.GetComponent<PlayerInventory>().itemsCount[counter2] - 1);
                    }
                    counter2++;
                }
            }

            // play sound effect
            shotsoundfx.Play();

            // muzzle flash
            MuzzleFlash.Play();

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            // show raycast prompt system
            if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
            {
                // deal damage to enemy
                hit.collider.gameObject.GetComponentInParent<EnemyAI>().Health = hit.collider.gameObject.GetComponentInParent<EnemyAI>().Health - 10;
                // animate hit
                hit.collider.gameObject.GetComponentInParent<EnemyAI>().TakeHit();
            }
            else
            {
                // impact on unflitered mesh
                // implment environment desctruction or bullet decals?
            }
        }

    }
}
