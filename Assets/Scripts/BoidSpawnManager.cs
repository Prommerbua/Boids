using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawnManager : MonoBehaviour
{
    public GameObject boidPrefab;
    public float numberToSpawn = 10;
    public int Xmin = -25, Xmax = 25, Ymin = -10, Ymax = 10, Zmin = -25, Zmax = 25;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 position = new Vector3(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax), Random.Range(Zmin, Zmax));
            Instantiate(boidPrefab, position, Random.rotation);
        }
    }
}
