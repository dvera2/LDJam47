using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEvents
{
    public static void TriggerLevelComplete() => LevelCompleted?.Invoke();
    public static event Action LevelCompleted;

    public static void TriggerLevelSimStarted() => LevelSimStarted?.Invoke();
    public static event Action LevelSimStarted;

    public static void TriggerLevelSimEnded() => LevelSimEnded?.Invoke();
    public static event Action LevelSimEnded;
    
    public static void TriggerItemButtonPointer(UiItemButton button, PointerEventData data) => ItemButtonClicked?.Invoke(button, data);
    public static event Action<UiItemButton, PointerEventData> ItemButtonClicked;


    public static void TriggerPreviewUpdated(PreviewUpdateArgs args) => ItemPreviewUpdated?.Invoke(args);
    public static event Action<PreviewUpdateArgs> ItemPreviewUpdated;

}
