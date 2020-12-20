using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidSpawnManager : MonoBehaviour
{
    public GameObject boidPrefab;
    public int numberToSpawn = 10;
    public Transform Xmin, Xmax, Ymin, Ymax, Zmin, Zmax;
    private GameObject[] boids;

    public float detectionRadius = 5.0f;
    public float speedMultiplier = 1.0f;
    public float minSpeed = 10.0f;
    public float maxSpeed = 10.0f;
    public float minimumDistance = 1.0f;
    public float boundaryStrength = 50f;
    [Range(-1, 1)] public float seperation = 1f;
    [Range(-1, 1)] public float alignement = 1f;
    [Range(-1, 1)] public float cohesion = 1f;
    public Transform TendToPlace;



    // Start is called before the first frame update
    void Start()
    {
        boids = new GameObject[numberToSpawn];
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 position = new Vector3(Random.Range(Xmin.position.x, Xmax.position.x),
                Random.Range(Ymin.position.y, Ymax.position.y), Random.Range(Zmin.position.z, Zmax.position.z));
            var boid = Instantiate(boidPrefab, position, Random.rotation);
            boids[i] = boid;
        }
    }

    private void Update()
    {
        foreach (var boid in boids)
        {
            boid.GetComponent<Boid>().detectionRadius = detectionRadius;
            boid.GetComponent<Boid>().speed = speedMultiplier;
            boid.GetComponent<Boid>().minSpeed = minSpeed;
            boid.GetComponent<Boid>().maxSpeed = maxSpeed;
            boid.GetComponent<Boid>().minimumDistance = minimumDistance;
            boid.GetComponent<Boid>().boundaryStrength = boundaryStrength;
            boid.GetComponent<Boid>().TendToPlace = TendToPlace.position;
            boid.GetComponent<Boid>().seperation = seperation;
            boid.GetComponent<Boid>().alignement = alignement;
            boid.GetComponent<Boid>().cohesion = cohesion;

        }
    }
}
