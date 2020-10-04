using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCueDefinition", menuName = "Audio/Cues")]
public class AudioCues : ScriptableObject
{
    public AudioClip UiHover;
    public AudioClip UiPlaceItem;
    public AudioClip UiSelectItem;
    public AudioClip UiGoButton;
    public AudioClip UiStopButton;
    public AudioClip[] EndDiddies;
}
