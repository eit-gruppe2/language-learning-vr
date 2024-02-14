using UnityEngine;

public class TTSManager : MonoBehaviour
{
    private OpenAIWrapper openAIWrapper;
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private TTSModel model = TTSModel.TTS_1;
    [SerializeField] private TTSVoice voice = TTSVoice.Alloy;
    [SerializeField, Range(0.25f, 4.0f)] private float speed = 1f;

    [SerializeField] private lewisAnimationStateController lewis;

    private void OnEnable()
    {
        if (!openAIWrapper) this.openAIWrapper = FindObjectOfType<OpenAIWrapper>();
        if (!audioPlayer) this.audioPlayer = GetComponentInChildren<AudioPlayer>();

        // Subscribe to the OnAudioFinished event
        audioPlayer.OnAudioFinished += HandleAudioFinished;
    }

    public async void SynthesizeAndPlay(string text)
    {
        Debug.Log("Trying to synthesize " + text);
        byte[] audioData = await openAIWrapper.RequestTextToSpeech(text, model, voice, speed);
        if (audioData != null)
        {
            Debug.Log("Playing audio.");
            audioPlayer.ProcessAudioBytes(audioData);
        }
        else
        {
            Debug.LogError("Failed to get audio data from OpenAI.");
        }
    }

    public async void SynthesizeAndPlay(string text, TTSModel model, TTSVoice voice, float speed)
    {
        this.model = model;
        this.voice = voice;
        this.speed = speed;
        this.lewis.startTalkingAnimation();
        SynthesizeAndPlay(text);
    }

    private void OnDisable()
    {
        // It's important to unsubscribe when the object is disabled/destroyed to prevent memory leaks
        audioPlayer.OnAudioFinished -= HandleAudioFinished;
    }

    private void HandleAudioFinished()
    {
        // Code to start the animation
        Debug.Log("Audio finished, starting animation.");
        this.lewis.stopTalkingAnimation();
    }
}