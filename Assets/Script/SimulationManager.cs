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
        AntsNumOnStart = (int)AntsAmountSlider.value;
        OnAntsAmountChanged();

        Ants = new List<GameObject>();
        Food = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (AntsNumOnStart > 0 && Random.Range(0, 101) > 70)
        {
            GameObject Ant = Instantiate(AntPrefab, HomeObj.transform.position, Quaternion.Euler(0,0, Random.Range(0,360)));
            Ants.Add(Ant);
            AntsNumOnStart--;
        }
    }

    public void OnRestartClick()
    {
        AntsNumOnStart = 0;
        TrackAntToggle.isOn = false;
        
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
        
        AntsNumOnStart = (int)AntsAmountSlider.value;
    }

    public void OnAntsAmountChanged()
    {
        AntsAmountText.text = AntsAmountSlider.value.ToString();
    }

    public GameObject GetRandomAnt()
    {
        return Ants[Random.Range(0, Ants.Count - 1)];
    }
}