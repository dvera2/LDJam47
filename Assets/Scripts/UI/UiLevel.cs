using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLevel : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject LevelComplete;
    public GameObject PostLevelUserControls;
    public GameObject UiItems;
    public Transform ItemGrid;

    [Header("Prefab")]
    public UiItemButton ItemButtonPrefab;

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
        var levelConstraints = GameObject.FindObjectOfType<LevelConstraints>();
        SetupConstraints(levelConstraints);

        EnableObj(LevelComplete, false);
        EnableObj(StartButton, true);
        EnableObj(StopButton, false);
        EnableObj(UiItems, true);
        EnableObj(PostLevelUserControls, false);
    }

    private void SetupConstraints(LevelConstraints levelConstraints)
    {
        if (!levelConstraints)
            return;

        if (!ItemGrid)
            return;

        ItemGrid.transform.ClearChildren();

        // Repopulate with constraint items 
        foreach(var c in levelConstraints.Items)
        {
            var instance = GameObject.Instantiate<UiItemButton>(ItemButtonPrefab, ItemGrid, false);
            instance.SetItem(c.Item);
            instance.SetCount(c.Count);
        }
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
        EnableObj(StopButton, false);
        EnableObj(UiItems, false);
        EnableObj(PostLevelUserControls, true);
    }

    private void EnableObj(GameObject obj, bool enable)
    {
        if (obj) obj.SetActive(enable);
    }
}
