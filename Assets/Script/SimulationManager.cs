using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private GameObject AntPrefab;

    [SerializeField] private GameObject HomeObj;

    private int AntsNumOnStart;

    public GameObject HomePrefab;
    public GameObject FoodPrefab;
    public GameObject WallPrefab;

    public Dropdown SimulationTypeDropdown;
    public Slider AntsAmountSlider;
    public Text AntsAmountText;

    public Toggle TrackAntToggle;

    private List<GameObject> Ants;
    private List<GameObject> Food;
    public GameObject Home;


    private void Start()
    {
        AntsNumOnStart = (int) AntsAmountSlider.value;
        OnAntsAmountChanged();

        Ants = new List<GameObject>();
        Food = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (AntsNumOnStart > 0 && Random.Range(0, 101) > 70)
        {
            GameObject Ant = Instantiate(AntPrefab, HomeObj.transform.position,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Ants.Add(Ant);
            AntsNumOnStart--;
        }
    }

    public void OnRestartClick()
    {
        //stops everything
        AntsNumOnStart = 0;
        TrackAntToggle.isOn = false;

        //clean everything
        foreach (var Ant in Ants)
        {
            Destroy(Ant);
        }

        Ants.Clear();

        foreach (var food in Food)
        {
            Destroy(food);
        }

        Food.Clear();

        GameObject[] ToFoodGameObjects = GameObject.FindGameObjectsWithTag("ToFood");
        if (ToFoodGameObjects.Length > 0)
        {
            foreach (var obj in ToFoodGameObjects)
            {
                Destroy(obj);
            }
        }

        GameObject[] ToHomeGameObjects = GameObject.FindGameObjectsWithTag("ToHome");
        if (ToHomeGameObjects.Length > 0)
        {
            foreach (var obj in ToHomeGameObjects)
            {
                Destroy(obj);
            }
        }

        //build stage

        //empty field, mo food
        if (SimulationTypeDropdown.value == 0)
        {
            Home.transform.position = Vector3.zero;
        }
        //fppd placed near the home
        else if (SimulationTypeDropdown.value == 1)
        {
            Home.transform.position = Vector3.zero;
            GenerateFoodIsland(Home.transform.position, 50, 0, 20, 1, 5);
        }
        else if (SimulationTypeDropdown.value == 2)
        {
            Home.transform.position = Vector3.zero;

            GenerateFoodIsland(Home.transform.position, 50, 0, 20, 5, 5);
        }
        else if (SimulationTypeDropdown.value == 3)
        {
            Home.transform.position = Vector3.zero;
            GenerateFoodIsland(Home.transform.position, 60, 5, 20, 7, 5);
        }

        //start ants
        AntsNumOnStart = (int) AntsAmountSlider.value;
    }

    public void OnAntsAmountChanged()
    {
        AntsAmountText.text = AntsAmountSlider.value.ToString();
    }

    public GameObject GetRandomAnt()
    {
        return Ants[Random.Range(0, Ants.Count - 1)];
    }

    public void GenerateFoodIsland(Vector3 HomePosition, float InitialDistanceFromHome, float DistanceStepPerCycle,
        int FoodAmounterCycle, int CyclesAmount, float FoodIslandRadius)
    {
        for (int j = 0; j < CyclesAmount; j++)
        {
            Vector3 vec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            vec *= InitialDistanceFromHome;
            vec += HomePosition;
            for (int i = 0; i < FoodAmounterCycle; i++)
            {
                GameObject food = Instantiate(FoodPrefab, new Vector3(
                    vec.x + Random.Range(-FoodIslandRadius, FoodIslandRadius),
                    vec.y + Random.Range(-FoodIslandRadius, FoodIslandRadius),
                    0
                ), Quaternion.identity);
                Food.Add(food);
            }

            InitialDistanceFromHome += DistanceStepPerCycle;
        }
    }
}