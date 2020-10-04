using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager GM => _instance;

    public SceneContainer SceneContainer { get; set; }
    public PlacementManager PlacementManager { get; set; }

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
