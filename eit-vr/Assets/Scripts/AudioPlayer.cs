using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public event Action OnAudioFinished; // Event to notify when audio finishes playing
    private bool deleteCachedFile = true;
    private bool isPlayingAudio = false;

    private void OnEnable()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    public void ProcessAudioBytes(byte[] audioData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "audio.mp3");
        File.WriteAllBytes(filePath, audioData);

        StartCoroutine(LoadAndPlayAudio(filePath));
    }

    private IEnumerator LoadAndPlayAudio(string filePath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = audioClip;
            audioSource.Play();
            isPlayingAudio = true;
            StartCoroutine(CheckAudioPlayback());
        }
        else
        {
            Debug.LogError("Audio file loading error: " + www.error);
            isPlayingAudio = false;
        }

        if (deleteCachedFile) File.Delete(filePath);
    }

    private IEnumerator CheckAudioPlayback()
    {
        // Wait until the audio has finished playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        isPlayingAudio = false;
        OnAudioFinished?.Invoke(); // Invoke the event
    }
}
