using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLevel : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject LevelComplete;
    public GameObject UiItems;


    private void Awake()
    {
        GameEvents.LevelSimStarted += OnLevelSimStarted;
        GameEvents.LevelSimEnded += OnLevelSimEnded;
        GameEvents.LevelCompleted += OnLevelComplete;
    }
    private void OnDestroy()
    {
        GameEvents.LevelSimStarted -= OnLevelSimStarted;
        GameEvents.LevelSimEnded -= OnLevelSimEnded;
        GameEvents.LevelCompleted -= OnLevelComplete;
    }

    private void Start()
    {
        EnableObj(LevelComplete, false);
        EnableObj(StartButton, true);
        EnableObj(StopButton, false);
        EnableObj(UiItems, true);
    }

    private void OnLevelSimEnded()
    {
        EnableObj(StopButton, false);
        EnableObj(StartButton, true);
        EnableObj(UiItems, true);
    }

    private void OnLevelSimStarted()
    {
        EnableObj(StopButton, true);
        EnableObj(StartButton, false);
        EnableObj(UiItems, false);
    }

    private void OnLevelComplete()
    {
        EnableObj(LevelComplete, true);
    }

    private void EnableObj(GameObject obj, bool enable)
    {
        if (obj) obj.SetActive(enable);
    }
}
