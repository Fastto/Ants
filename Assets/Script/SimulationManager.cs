using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour
{

    [SerializeField] private GameObject AntPrefab;

    [SerializeField] private GameObject HomeObj;

    [SerializeField] private int AntsNumOnStart;

    private void Start()
    {
      
    }

    private void FixedUpdate()
    {
        if (AntsNumOnStart > 0 && Random.Range(0, 101) > 70)
        {
            Instantiate(AntPrefab, HomeObj.transform.position, Quaternion.Euler(0,0, Random.Range(0,360)));
            AntsNumOnStart--;
        }
    }
}