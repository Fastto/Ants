using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Marker : MonoBehaviour
{
    [SerializeField] private float LifeTimeLimit = 1f;

    public float Intensivity { get; set; } = 1;

    [SerializeField] public float BurningRatePerSec = .9f;

    [SerializeField] public float IntensivityLimit = .05f;


    private float LifeCounter = 0f;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        LifeCounter += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LifeCounter > 1f)
        {
            LifeCounter = 0;
            Refresh();
        }

        if (Intensivity < IntensivityLimit)
        {
            Destroy(this.gameObject);
        }
    }

    public void Refresh()
    {
        Intensivity *= BurningRatePerSec;
        Color old = _spriteRenderer.color;
        _spriteRenderer.color = new Color(old.r, old.g, old.b, Intensivity);
    }
}