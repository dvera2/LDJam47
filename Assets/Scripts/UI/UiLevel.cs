using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLevel : MonoBehaviour
{
    public GameObject EditModeControls;
    public GameObject PlayModeControls;
    public GameObject LevelComplete;
    public GameObject PostLevelUserControls;
    public GameObject UiItems;
    public GameObject RulesPanel;
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
        EnableObj(EditModeControls, true);
        EnableObj(PlayModeControls, false);
        EnableObj(UiItems, true);
        EnableObj(PostLevelUserControls, false);
        EnableObj(RulesPanel, false);

        if( GameFlow.GF )
        {
            bool isFirstLevel = GameFlow.GF.GameProgression.Levels.FindIndex((x) => x.Name == SceneManager.GetActiveScene().name) == 0;
            EnableObj(RulesPanel, isFirstLevel);
        }
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
        EnableObj(PlayModeControls, false);
        EnableObj(EditModeControls, true);
        EnableObj(UiItems, true);
    }

    private void OnLevelSimStarted()
    {
        EnableObj(PlayModeControls, true);
        EnableObj(EditModeControls, false);
        EnableObj(UiItems, false);
    }

    private void OnLevelComplete()
    {
        EnableObj(PlayModeControls, false);
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
