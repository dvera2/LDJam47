using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public static void TriggerLevelComplete() => LevelCompleted?.Invoke();
    public static event Action LevelCompleted;

    public static void TriggerLevelSimStarted() => LevelSimStarted?.Invoke();
    public static event Action LevelSimStarted;

    public static void TriggerLevelSimEnded() => LevelSimEnded?.Invoke();
    public static event Action LevelSimEnded;
    
    public static void TriggerItemButtonClicked(UiItemButton button) => ItemButtonClicked?.Invoke(button);
    public static event Action<UiItemButton> ItemButtonClicked; 

}
