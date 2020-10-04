using System;
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

    public void GoToMainMenu()
    {
        throw new NotImplementedException();
    }

    public void GoToNextLevel()
    {
        throw new NotImplementedException();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}
