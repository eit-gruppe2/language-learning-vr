using OpenAI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;


namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private InputActionReference primaryButtonActionReference;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private string systemMessage;

        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi("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");

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        private float height;
        private List<ChatMessage> messages = new List<ChatMessage>();

        [SerializeField] private TTSManager ttsManager;
        [SerializeField] private ObjectivesNFeedback objectivesNFeedback;
        [SerializeField] private responsiveUI responsiveUI;

        private void Start()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
#endif

            messages.Add(new ChatMessage() { 
                Role = "system",
                Content = systemMessage
            });
            Console.Write(systemMessage);
        }

        private void OnEnable()
        {
            // Register the input action event for the primary button
            primaryButtonActionReference.action.started += OnPrimaryButtonPressed;
        }

        private void OnDisable()
        {
            // Unregister the input action event to avoid memory leaks
            primaryButtonActionReference.action.started -= OnPrimaryButtonPressed;
        }

        private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
        {
            if (isRecording)
            {
                EndRecording();
            }
            else
            {
                StartRecording();
            }
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private void StartRecording()
        {
            if (isRecording)
            {
                EndRecording();
                return;
            }

            isRecording = true;
            responsiveUI.UpdateRecordIcons();
            // change text of button
            recordButton.GetComponentInChildren<Text>().text = "Stop recording";

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            
            #if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            #endif
        }

        private async void EndRecording()
        {            
            isRecording = false;
            responsiveUI.UpdateRecordIcons();
            recordButton.GetComponentInChildren<Text>().text = "Record Audio";

            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = LanguageSettings.whisperLanguageTag
            };
            var res = await openai.CreateAudioTranscription(req);

            SendReply(res.Text);
            recordButton.enabled = true;
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply(string txt)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = txt
            };

            AppendMessage(newMessage);

            messages.Add(newMessage);

            recordButton.enabled = false;

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);

                // TTS
                if (ttsManager) ttsManager.SynthesizeAndPlay(message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            recordButton.enabled = true;
            objectivesNFeedback.CheckForCompletedObjectives(messages);
        }

    }
}
