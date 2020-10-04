using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Surface Type", menuName = "Collision/Surface Type Asset")]
public class SurfaceType : ScriptableObject
{
    [Header("Info")]
    public string Name;

    [Header("Audio")]
    public AudioClip ImpactAudio;
    public AudioClip RollAudio;

    [Header("DEBUG")]
    public Color DebugSurfaceColor = Color.yellow;
}
