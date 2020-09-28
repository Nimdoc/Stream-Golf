using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    GameObject generator;

    void Start()
    {
        generator = GameObject.FindWithTag("Generator");
    }

    private void OnTriggerEnter(Collider other)
    {
        generator.GetComponent<CourseGenerator>().regenerateGame();
    }
}
