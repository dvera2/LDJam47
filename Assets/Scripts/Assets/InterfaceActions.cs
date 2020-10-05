using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Actions", menuName = "System/Create Interface Actions")]
public class InterfaceActions : ScriptableObject
{
    public void StartSimulation() => GameManager.GM.StartSim();

    public void StopSimulation() => GameManager.GM.StopSim();

    public void RestartLevel() => GameFlow.GF.RestartLevel();

    public void NextLevel() => GameFlow.GF.GoToNextLevel();

    public void MainMenu() => GameFlow.GF.GoToMainMenu();

    public void StartNewGame() => GameFlow.GF.StartGame();
}
