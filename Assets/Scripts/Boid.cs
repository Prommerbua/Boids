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
    public float maxSpeed = 10.0f;
    private Vector3 _velocity;
    public float minimumDistance = 1.0f;
    [Range(0.0f, 1.0f)] public float seperation = 1.0f;
    [Range(0.0f, 1.0f)] public float alignement = 1.0f;
    [Range(0.0f, 1.0f)] public float cohesion = 1.0f;

    private Collider[] _nearbyBoids;

    private int Xmin = -25, Xmax = 25, Ymin = 0, Ymax = 20, Zmin = -25, Zmax = 25;


    void Start()
    {
        _velocity = new Vector3(Random.Range(speed, maxSpeed), Random.Range(speed, maxSpeed), Random.Range(speed, maxSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        _nearbyBoids = Physics.OverlapSphere(transform.position, detectionRadius);

        var s = SeperationRule() * seperation;
        var v = CheckBoundaries();

        _velocity += s;
        _velocity += v;

        if (_velocity.magnitude > maxSpeed)
        {
            _velocity = (_velocity / _velocity.magnitude) * maxSpeed;
        }
        //Apply Orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_velocity), 0.15f);
        transform.Translate(_velocity * (speed * Time.deltaTime), Space.World);
    }

    private Vector3 CheckBoundaries()
    {
        Vector3 v = Vector3.zero;
        if (transform.position.x < Xmin) v.x = 10;
        else if (transform.position.x > Xmax) v.x = -10;
        if (transform.position.y < Ymin) v.y = 10;
        else if (transform.position.y > Ymax) v.y = -10;
        if (transform.position.z < Zmin) v.z = 10;
        else if (transform.position.z > Zmax) v.z = -10;
        return v;
    }

    private Vector3 SeperationRule()
    {
        Vector3 c = Vector3.zero;
        foreach (var boid in _nearbyBoids)
        {
            if (boid.transform.root != transform)
            {
                {
                    if (Vector3.Distance(boid.gameObject.transform.position, transform.position) < minimumDistance)
                    {
                        c = c - (boid.gameObject.transform.position - transform.position);
                    }
                }
            }
        }

        return c;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
