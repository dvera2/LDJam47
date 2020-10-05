using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevel : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject LevelComplete;
    public GameObject PostLevelUserControls;
    public GameObject UiItems;
    public Transform ItemGrid;
    public Image PreviewImage; 

    [Header("Prefab")]
    public UiItemButton ItemButtonPrefab;

    private void Awake()
    {
        GameEvents.LevelSimStarted += OnLevelSimStarted;
        GameEvents.LevelSimEnded += OnLevelSimEnded;
        GameEvents.LevelCompleted += OnLevelComplete;
        GameEvents.ItemPreviewUpdated += OnItemPreviewUpdated;
    }


    private void OnDestroy()
    {
        GameEvents.ItemPreviewUpdated -= OnItemPreviewUpdated;
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

    private void OnItemPreviewUpdated(PreviewUpdateArgs obj)
    {
        if (PreviewImage)
        {
            PreviewImage.enabled = obj.Enabled;
            if (obj.Enabled)
            {
                Color c = Color.white;
                c.a = 0.5f;

                PreviewImage.sprite = obj.Sprite;
                PreviewImage.color = obj.ValidSpot ? c : Color.red;
                PreviewImage.enabled = true;
                PreviewImage.rectTransform.rotation = Quaternion.Euler(0, 0, obj.Angle);

                var vp = Camera.main.WorldToViewportPoint(obj.WorldPosition);
                PreviewImage.rectTransform.anchorMin = vp;
                PreviewImage.rectTransform.anchorMax = vp;
            }
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
        EnableObj(StopButton, false);
        EnableObj(UiItems, false);
        EnableObj(PostLevelUserControls, true);

        StartCoroutine(ShowPanel());
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(2.5f);
        EnableObj(LevelComplete, true);
    }

    private void EnableObj(GameObject obj, bool enable)
    {
        if (obj) obj.SetActive(enable);
    }
}
