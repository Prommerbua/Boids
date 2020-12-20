using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    // Start is called before the first frame update
    public float detectionRadius = 5.0f;
    public float speed = 1.0f;
    public float minSpeed = 10.0f;
    public float maxSpeed = 10.0f;
    [HideInInspector] Vector3 velocity;
    public float minimumDistance = 1.0f;
    public float boundaryStrength = 50f;
    public Vector3 TendToPlace;
    [Range(-1, 1)] public float seperation = 1f;
    [Range(-1, 1)] public float alignement = 1f;
    [Range(-1, 1)] public float cohesion = 1f;

    private int boidLayer;

    private Collider[] _nearbyBoids;

    private int Xmin = -25, Xmax = 25, Ymin = -10, Ymax = 10, Zmin = -25, Zmax = 25;


    void Start()
    {
        velocity = new Vector3(Random.Range(speed, maxSpeed), Random.Range(speed, maxSpeed),
            Random.Range(speed, maxSpeed));
        boidLayer = 1 << LayerMask.NameToLayer("Boid");

        // speed = Random.Range(2.0f, 4.0f);
        // detectionRadius = Random.Range(2.0f, 5.0f);
        // minimumDistance = Random.Range(1.0f, 3.0f);
        // seperation = Random.Range(0.1f, 0.5f);
        // alignement = Random.Range(0.1f, 0.5f);
        // cohesion = Random.Range(0.5f, 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        _nearbyBoids = Physics.OverlapSphere(transform.position, detectionRadius, boidLayer);

        var v1 = SeperationRule() * seperation;
        var v2 = AlignmentRule() * alignement;
        var v3 = CohesionRule() * cohesion;
        var v4 = TendToPlace - transform.position / 100;

        var v5 = CheckBoundaries();


        velocity += v1 + v2 + v3 + v5;

        if (velocity.magnitude > maxSpeed)
        {
            velocity = (velocity / velocity.magnitude) * maxSpeed;
        }

        //Apply Orientation
        if (velocity.magnitude < minSpeed)
        {
            velocity = (velocity / velocity.magnitude) * minSpeed;
        }

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.15f);
            transform.Translate(velocity * (speed * Time.deltaTime), Space.World);
        }
    }

    private Vector3 AlignmentRule()
    {
        Vector3 v3 = Vector3.zero;
        foreach (var boid in _nearbyBoids)
        {
            if (boid.transform.root != transform)
            {
                Boid b = boid.gameObject.GetComponentInParent<Boid>();
                if (b != null)
                {
                    v3 += b.velocity;
                }
            }
        }

        if (_nearbyBoids.Length > 1)
        {
            v3 /= _nearbyBoids.Length - 1;
            return (v3 - velocity) / 16;
        }
        return Vector3.zero;
    }

    private Vector3 CohesionRule()
    {
        Vector3 pc = Vector3.zero;
        foreach (var boid in _nearbyBoids)
        {
            if (boid.transform.root != transform)
            {
                pc += boid.gameObject.transform.position;
            }
        }

        if (_nearbyBoids.Length > 1)
        {
            pc /= _nearbyBoids.Length - 1;
            pc = (pc - transform.position) / 100.0f;
            Debug.DrawLine(transform.position, transform.position + pc);
            return pc;
        }
        return Vector3.zero;
    }

    private Vector3 CheckBoundaries()
    {
        Vector3 v = Vector3.zero;
        if (transform.position.x < Xmin) v.x = boundaryStrength;
        else if (transform.position.x > Xmax) v.x = -boundaryStrength;
        if (transform.position.y < Ymin) v.y = boundaryStrength;
        else if (transform.position.y > Ymax) v.y = -boundaryStrength;
        if (transform.position.z < Zmin) v.z = boundaryStrength;
        else if (transform.position.z > Zmax) v.z = -boundaryStrength;
        return v;
    }

    private Vector3 SeperationRule()
    {
        Vector3 c = Vector3.zero;
        foreach (var boid in _nearbyBoids)
        {
            if (boid.transform.root != transform)
            {
                if (Vector3.Distance(boid.gameObject.transform.position, transform.position) < minimumDistance)
                {
                    c = c - (boid.gameObject.transform.position - transform.position);
                }
            }
        }
        return c / 100;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
