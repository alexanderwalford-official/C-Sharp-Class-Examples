using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGeneratorSimple : MonoBehaviour
{
    public GameObject top;
    public GameObject filler;
    public bool TopLayerDone = false;
    public bool LockOn = false;
    public int MaxRows;
    public int MaxColls;
    public int MaxDepth;
    public int Rows = 0;
    public int Colls = 0;
    public int Depth = 0;
    float X = 0;
    float Z = 0;
    float Y = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateToplayer(0.535f);
    }

    public void GenerateToplayer (float Y)
    {
        if (Colls < MaxColls)
        {
            Vector3 CurrentBlockSpawnLoc1 = new Vector3(X, Y, Z - Rows / 20);
            if (!TopLayerDone)
            {
                var a = Instantiate(top, CurrentBlockSpawnLoc1, Quaternion.identity);
                a.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc2 = new Vector3(-X, Y, -Z - Rows / 20);
                var b = Instantiate(top, CurrentBlockSpawnLoc2, Quaternion.identity);
                b.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc3 = new Vector3(-X, Y, Z - Rows / 20);
                var c = Instantiate(top, CurrentBlockSpawnLoc3, Quaternion.identity);
                c.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc4 = new Vector3(X - Rows / 20, Y, -Z);
                var d = Instantiate(top, CurrentBlockSpawnLoc4, Quaternion.identity);
                d.transform.parent = this.transform;
            }
            else
            {
                var a = Instantiate(filler, CurrentBlockSpawnLoc1, Quaternion.identity);
                a.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc2 = new Vector3(-X, Y, -Z - Rows / 20);
                var b = Instantiate(filler, CurrentBlockSpawnLoc2, Quaternion.identity);
                b.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc3 = new Vector3(-X, Y, Z - Rows / 20);
                var c = Instantiate(filler, CurrentBlockSpawnLoc3, Quaternion.identity);
                c.transform.parent = this.transform;

                Vector3 CurrentBlockSpawnLoc4 = new Vector3(X - Rows / 20, Y, -Z);
                var d = Instantiate(filler, CurrentBlockSpawnLoc4, Quaternion.identity);
                d.transform.parent = this.transform;
            }

            Colls++;
            X = X + 0.05f;
            GenerateToplayer(Y);
        }
        else
        {
            if (Rows < MaxRows)
            {
                X = 0;
                Z = Z + 0.05f;
                
                Colls = 0;
                Rows++;
                GenerateToplayer(Y);
            }
            else
            {
                StartCoroutine(Wait());
                UnityEngine.Debug.Log("Top layer done.");
                TopLayerDone = true;
                if (Depth < MaxDepth)
                {
                    if (!!LockOn)
                    {
                        X = 0;
                        Z = 0;
                        Y = Y - 0.05f;
                        Rows = 0;
                        Colls = 0;
                        Depth++;
                        GenerateToplayer(Y);
                    }
                    else
                    {
                        GenerateToplayer(Y);
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Filler layer done.");
                }   
            }
        }
    }

    IEnumerator Wait()
    {
        LockOn = true;
        yield return new WaitForSeconds(1);
        LockOn = false;
    }
}
