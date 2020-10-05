using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    private static GameFlow _instance = null;
    public static GameFlow GF => _instance;

    public LevelList GameProgression;
    private int _currentLevel = -1;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }

    public void StartGame()
    {
        _currentLevel = 0;
        SceneManager.LoadScene(GameProgression.Levels[_currentLevel].Name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(GameProgression.MainMenu.Name);
    }
    public void GoToNextLevel()
    {
        _currentLevel++;
        if (_currentLevel >= 0)
        {
            if (_currentLevel < GameProgression.Levels.Count)
                SceneManager.LoadScene(GameProgression.Levels[_currentLevel].Name);
            else
                SceneManager.LoadScene(GameProgression.Ending.Name);
        }
    }
}
