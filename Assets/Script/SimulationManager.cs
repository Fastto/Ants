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
    
    public GameObject FoodPrefab;
    public GameObject WallPrefab;
    public GameObject MazePrefab;
    public GameObject SubscribePrefab;

    public Dropdown SimulationTypeDropdown;
    public Slider AntsAmountSlider;
    public Text AntsAmountText;

    public Toggle TrackAntToggle;

    private List<GameObject> Ants;
    private List<GameObject> Food;
    private GameObject Maze;
    private GameObject Subscribe;
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
        foreach (var ant in Ants)
        {
            Destroy(ant);
        }

        Ants.Clear();

        foreach (var food in Food)
        {
            Destroy(food);
        }

        Food.Clear();

        GameObject[] toFoodGameObjects = GameObject.FindGameObjectsWithTag("ToFood");
        if (toFoodGameObjects.Length > 0)
        {
            foreach (var obj in toFoodGameObjects)
            {
                Destroy(obj);
            }
        }

        GameObject[] toHomeGameObjects = GameObject.FindGameObjectsWithTag("ToHome");
        if (toHomeGameObjects.Length > 0)
        {
            foreach (var obj in toHomeGameObjects)
            {
                Destroy(obj);
            }
        }

        if (Maze != null)
        {
            Destroy(Maze);
            Maze = null;
        }
        
        if (Subscribe != null)
        {
            Destroy(Subscribe);
            Subscribe = null;
        }

        //build stage
        switch (SimulationTypeDropdown.value)
        {
            case 1:
                Home.transform.position = Vector3.zero;
                GenerateFoodIsland(Home.transform.position, 50, 0, 20, 1, 5);
                break;
            case 2:
                Home.transform.position = Vector3.zero;
                GenerateFoodIsland(Home.transform.position, 50, 0, 20, 5, 5);
                break;
            case 3:
                Home.transform.position = Vector3.zero;
                GenerateFoodIsland(Home.transform.position, 60, 5, 20, 7, 5);
                break;
            case 4:
                //Food is placed far away from home
                Home.transform.position = new Vector3(-55f, 55f, 0);
                GenerateFoodIsland(new Vector3(55f, -55f, 0), 0, 0, 100, 1, 20);
                break;
            case 5:
                //Walls
                Home.transform.position = new Vector3(-35f, 35f, 0);
                GenerateFoodIsland(new Vector3(35f, -35f, 0), 0, 0, 100, 1, 20);
                Maze = Instantiate(WallPrefab);
                break;
            case 6:
                //Maze
                Home.transform.position = new Vector3(-35f, 35f, 0);
                GenerateFoodIsland(new Vector3(35f, -35f, 0), 0, 0, 20, 1, 3);
                Maze = Instantiate(MazePrefab);
                break;
            case 7:
                //Wealth
                Home.transform.position = Vector3.zero;
                GenerateFoodIsland(Home.transform.position, 60, 0, 20, 100, 5);
                break;
            case 8:
                //Subscribe
                Home.transform.position = new Vector3(-0f, 40f, 0);
                Subscribe = Instantiate(SubscribePrefab);
                break;
            case 0:
            default:
                Home.transform.position = Vector3.zero;
                break;
                
        }

        //start ants generation
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

    public void GenerateFoodIsland(Vector3 homePosition, float initialDistanceFromHome, float distanceStepPerCycle,
        int foodAmountPerCycle, int cyclesAmount, float foodIslandRadius)
    {
        for (int j = 0; j < cyclesAmount; j++)
        {
            Vector3 vec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            vec *= initialDistanceFromHome;
            vec += homePosition;
            for (int i = 0; i < foodAmountPerCycle; i++)
            {
                GameObject food = Instantiate(FoodPrefab, new Vector3(
                    vec.x + Random.Range(-foodIslandRadius, foodIslandRadius),
                    vec.y + Random.Range(-foodIslandRadius, foodIslandRadius),
                    0
                ), Quaternion.identity);
                Food.Add(food);
            }

            initialDistanceFromHome += distanceStepPerCycle;
        }
    }
}