using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLevel : MonoBehaviour
{
    public GameObject LevelComplete;

    private void Awake()
    {
        GameEvents.LevelCompleted += OnLevelComplete;
    }

    private void Start()
    {
        if (LevelComplete)
            LevelComplete.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.LevelCompleted -= OnLevelComplete;
    }

    private void OnLevelComplete()
    {
        if (LevelComplete)
            LevelComplete.SetActive(true);
    }
}
