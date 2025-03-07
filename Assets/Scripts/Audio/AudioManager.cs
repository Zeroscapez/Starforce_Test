using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;                  // Unique identifier
    public AudioClip clip;               // Audio clip asset
    [Range(0f, 1f)]
    public float volume = 1f;            // Individual volume
    public bool loop = false;            // Whether the sound should loop
    public AudioMixerGroup mixerGroup;   // Assign to a mixer group (e.g., BGM or SFX) via the Inspector
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Settings")]
    public List<Sound> bgmSounds = new List<Sound>(); // Configure background music sounds

    [Header("SFX Settings")]
    public List<Sound> sfxSounds = new List<Sound>(); // Configure sound effects

    [Header("Master Settings")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;  // Master volume slider for all sounds

    private AudioSource bgmSource;   // Dedicated AudioSource for background music
    private AudioSource sfxSource;   // Dedicated AudioSource for sound effects

    private void Awake()
    {
        // Singleton pattern: ensure only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            
            // Create dedicated AudioSources for BGM and SFX
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Plays background music by name. If the same track is already playing, it won't restart.
    /// </summary>
    public void PlayBGM(string name)
    {
        Sound found = bgmSounds.Find(s => s.name == name);
        if (found != null && found.clip != null)
        {
            if (bgmSource.clip == found.clip && bgmSource.isPlaying)
                return; // Avoid restarting the same song

            bgmSource.clip = found.clip;
            bgmSource.volume = found.volume * masterVolume;
            bgmSource.loop = found.loop;
            if (found.mixerGroup != null)
                bgmSource.outputAudioMixerGroup = found.mixerGroup;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found: " + name);
        }
    }

    /// <summary>
    /// Stops the currently playing background music.
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void PlaySongSequence(string track1, string track2)
    {
        StopBGM();
        StartCoroutine(SequenceCoroutine(track1, track2));
    }
    private IEnumerator SequenceCoroutine(string track1, string track2)
    {
        // Play the start clip.
        PlayBGM(track1);

        // Find the start sound in our list.
        Sound startSound = bgmSounds.Find(s => s.name == track1);
        if (startSound != null && startSound.clip != null)
        {
            // Wait for the length of the start clip.
            yield return new WaitForSeconds(startSound.clip.length);
        }
        else
        {
            Debug.LogWarning("Start sound not found or has no clip: " + track1);
        }

        // Now play the looping clip.
        Sound endSound = bgmSounds.Find(s => s.name == track2);
        if (endSound != null && endSound.clip != null)
        {
            // Ensure loop is enabled for the loop clip.
            endSound.loop = true;
            PlayBGM(track2);
        }
        else
        {
            Debug.LogWarning("Loop sound not found or has no clip: " + track2);
        }
    }

    /// <summary>
    /// Plays a sound effect by name using PlayOneShot.
    /// </summary>
    public void PlaySFX(string name)
    {
        Sound found = sfxSounds.Find(s => s.name == name);
        if (found != null && found.clip != null)
        {
            if (found.mixerGroup != null)
                sfxSource.outputAudioMixerGroup = found.mixerGroup;
            sfxSource.PlayOneShot(found.clip, found.volume * masterVolume);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }
}
