using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Oculus.Voice;
using Meta.WitAi.CallbackHandlers;
using UnityEngine.Events;

public class VoiceManager : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private WitResponseMatcher responseMatcher;

    [SerializeField] private UnityEvent openStatusDetected;
    [SerializeField] private UnityEvent<string> completeTranscription;

    private bool _voiceCommandReady;

    private void Awake()
    {
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);

        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if(eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.AddListener(OpenStatusDetected);
        }
        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);

        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.RemoveListener(OpenStatusDetected);
        }
    }
    private void ReactivateVoice() => appVoiceExperience.Activate();

    private void OpenStatusDetected(string[] arg0)
    {
        _voiceCommandReady = true;
        openStatusDetected?.Invoke();
    }

    private void OnPartialTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        Debug.Log(transcription);
    }

    private void OnFullTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        _voiceCommandReady = false;
        completeTranscription?.Invoke(transcription);
    }
}
