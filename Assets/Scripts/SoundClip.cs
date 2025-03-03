using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string name;  // A name to identify the clip in the Inspector
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;  // Individual volume slider for this clip
}
