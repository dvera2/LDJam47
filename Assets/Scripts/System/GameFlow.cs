using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private static GameFlow _instance = null;
    public static GameFlow GF => _instance;

    public AudioSource UiAudio;
    public AudioSource MusicAudio;
    public AudioCues Cues;

    [Header("Fade Settings")]
    public CanvasGroup Fade;
    public int FadeSteps = 8;
    public float Duration = 0.5f;
    
    public LevelList GameProgression;
    private int _currentLevel = -1;

    // --------------------------------------------------------------------------
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    // --------------------------------------------------------------------------
    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }

    // --------------------------------------------------------------------------
    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }


    // --------------------------------------------------------------------------
    public void StartGame()
    {
        _currentLevel = 0;
        StartCoroutine(SwitchToLevel(GameProgression.Levels[_currentLevel].Name));
    }

    // --------------------------------------------------------------------------
    public void GoToMainMenu()
    {
        StartCoroutine(SwitchToLevel(GameProgression.MainMenu.Name));
    }

    // --------------------------------------------------------------------------
    public void GoToNextLevel()
    {
        _currentLevel++;
        if (_currentLevel >= 0)
        {
            if (_currentLevel < GameProgression.Levels.Count)
                StartCoroutine(SwitchToLevel(GameProgression.Levels[_currentLevel].Name));
            else
                StartCoroutine(SwitchToLevel(GameProgression.Ending.Name));
        }
    }

    // --------------------------------------------------------------------------
    public void RestartLevel()
    {
        StartCoroutine(SwitchToLevel(GameProgression.Levels[_currentLevel].Name));
    }

    private IEnumerator SwitchToLevel( string levelName )
    {
        for(int i = 0; i < FadeSteps; i++)
        {
            Fade.alpha = Mathf.Lerp(0, 1, (float)i / (FadeSteps - 1));
            yield return new WaitForSeconds(Duration / FadeSteps);
        }

        SceneManager.LoadScene(levelName);

        for (int i = 0; i < FadeSteps; i++)
        {
            Fade.alpha = Mathf.Lerp(1, 0, (float)i / (FadeSteps - 1));
            yield return new WaitForSeconds(Duration / FadeSteps);
        }
    }
}
