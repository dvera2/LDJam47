using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Actions", menuName = "System/Create Interface Actions")]
public class InterfaceActions : ScriptableObject
{
    public void StartSimulation() => GameManager.GM.StartSim();

    public void StopSimulation() => GameManager.GM.StopSim();

    public void RestartLevel() => GameManager.GM.RestartLevel();

    public void NextLevel() => GameManager.GM.GoToNextLevel();

    public void MainMenu() => GameManager.GM.GoToMainMenu();
}
