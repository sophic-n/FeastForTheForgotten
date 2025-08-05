using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicPlayer : MonoBehaviour
{
    public List<AudioClip> musicTracks; // Drag your audio clips here in the inspector
    private AudioSource audioSource;

    private List<AudioClip> shuffledTracks;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // We want to handle track switching manually
        ShuffleTracks();
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void ShuffleTracks()
    {
        shuffledTracks = new List<AudioClip>(musicTracks);

        // Fisher-Yates shuffle
        for (int i = 0; i < shuffledTracks.Count; i++)
        {
            int j = Random.Range(i, shuffledTracks.Count);
            (shuffledTracks[i], shuffledTracks[j]) = (shuffledTracks[j], shuffledTracks[i]);
        }

        currentTrackIndex = 0;
    }

    void PlayNextTrack()
    {
        if (shuffledTracks == null || shuffledTracks.Count == 0)
            return;

        if (currentTrackIndex >= shuffledTracks.Count)
        {
            ShuffleTracks(); // Reshuffle when we've played all tracks
        }

        audioSource.clip = shuffledTracks[currentTrackIndex];
        audioSource.Play();
        currentTrackIndex++;
    }
}

