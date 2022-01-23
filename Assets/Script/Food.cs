using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{

    void Start()
    {
        transform.Rotate(Vector3.forward, Random.Range(0, 180));
    }

   
}
