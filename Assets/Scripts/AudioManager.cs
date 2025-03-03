using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Dictionary<string, AudioSource> activeSounds = new Dictionary<string, AudioSource>();
    [Header("Sound Clips")]
    public List<SoundClip> soundClips = new List<SoundClip>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string clipName, bool loop = false)
    {
        SoundClip found = soundClips.Find(s => s.name == clipName);
        if (found != null && found.clip != null)
        {
            if (activeSounds.ContainsKey(clipName)) return; // Prevent duplicate sounds

            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = found.clip;
            newSource.volume = found.volume;
            newSource.loop = loop;
            newSource.Play();

            activeSounds[clipName] = newSource;
        }
        else
        {
            Debug.LogWarning($"Sound '{clipName}' not found!");
        }
    }

    public void StopSound(string clipName)
    {
        if (activeSounds.TryGetValue(clipName, out AudioSource source))
        {
            source.Stop();
            Destroy(source);
            activeSounds.Remove(clipName);
        }
    }
}
