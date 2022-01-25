using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{

    [SerializeField] private int amount = 10;
    
    void Start()
    {
        transform.Rotate(Vector3.forward, Random.Range(0, 180));
        transform.position = new Vector3(
        transform.position.x + Random.Range(-2f, 2f),
        transform.position.y + Random.Range(-2f, 2f),
        transform.position.z+ Random.Range(-2f, 2f)
            );
    }

    public void Bite()
    {
        amount--;

        if (amount < 1)
        {
            Destroy(gameObject);
        }
    }

   
}
