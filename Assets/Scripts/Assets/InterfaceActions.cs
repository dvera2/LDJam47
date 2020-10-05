using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Actions", menuName = "System/Create Interface Actions")]
public class InterfaceActions : ScriptableObject
{
    public void StartSimulation() => DoAction(GameFlow.GF.Cues.UiGoButton, GameManager.GM.StartSim);

    public void StopSimulation() => DoAction(GameFlow.GF.Cues.UiStopButton, GameManager.GM.StopSim);

    public void RestartLevel() => DoAction(GameFlow.GF.Cues.UiGoButton, GameFlow.GF.RestartLevel);

    public void NextLevel() => DoAction(GameFlow.GF.Cues.UiGoButton, GameFlow.GF.GoToNextLevel);

    public void MainMenu() => DoAction(GameFlow.GF.Cues.UiGoButton, GameFlow.GF.GoToMainMenu);

    public void StartNewGame() => DoAction(GameFlow.GF.Cues.UiGoButton, GameFlow.GF.StartGame);

    private void DoAction(AudioClip clip, Action callback, bool wait = false)
    {
        GameFlow.GF.StartCoroutine(ActionCoroutine(clip, callback, wait));
    }

    private IEnumerator ActionCoroutine(AudioClip clip, Action callback, bool wait)
    {
        if (clip)
        {
            clip.PlayUiSource();

            if (wait)
            {
                yield return new WaitUntil(() => !GameFlow.GF.UiAudio.isPlaying);
            }
        }

        callback?.Invoke();
        yield break;
    }
}
