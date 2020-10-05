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

    public void StartSim()
    {
        if(PlacementManager.Validate())
        {
            PlacementManager.GenerateItems(SceneContainer);

            GameEvents.TriggerLevelSimStarted();
        }
    }

    public void StopSim()
    {
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
        if (GameFlow.GF.Cues.EndDiddies.Length > 0)
            diddy = Random.Range(0, GameFlow.GF.Cues.EndDiddies.Length);

        if (diddy >= 0)
            GameFlow.GF.Cues.EndDiddies[diddy].PlayUiSource();


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
