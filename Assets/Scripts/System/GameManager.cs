using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager GM => _instance;

    public SceneContainer SceneContainer { get; set; }

    public PlacementManager PlacementManager { get; set; }

    public AudioCues Cues;
    public AudioSource MainUiSource;
    public AudioSource MusicSource;

    public void StartSim()
    {
        if(PlacementManager.Validate())
        {
            PlacementManager.GenerateItems(SceneContainer);

            GameEvents.TriggerLevelSimStarted();

            Cues.UiGoButton.PlayUiSource();
        }
    }

    public void StopSim()
    {
        // 
        Cues.UiStopButton.PlayUiSource();

        // Call first for any required interception
        GameEvents.TriggerLevelSimEnded();

        // THEN Clean up player-made stuff
        SceneContainer.DestroyAll();

        // THEN Restore level state
        PlacementManager.RestoreState();
    }

    public void TriggerLevelComplete()
    {
        // Play victory cue
        int diddy = -1;
        if (Cues.EndDiddies.Length > 0)
            diddy = Random.Range(0, Cues.EndDiddies.Length);

        if (diddy >= 0)
            Cues.EndDiddies[diddy].PlayUiSource();


        // trigger completion.
        GameEvents.TriggerLevelComplete();
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        SceneContainer = GetComponent<SceneContainer>();
        PlacementManager = GetComponent<PlacementManager>();
    }

    private void OnDestroy()
    {
        if(this == _instance)
        {
            _instance = null;
        }
    }
}
