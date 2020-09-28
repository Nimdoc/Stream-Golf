using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float rotateSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed += Random.Range(-10.0f, 10.0f);
    }

    // FixedUpdate runs once before each tick of the physics system. 
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
    }
}
