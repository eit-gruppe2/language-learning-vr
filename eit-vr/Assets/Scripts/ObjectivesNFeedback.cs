using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesNFeedback : MonoBehaviour
{

    [SerializeField] private responsiveUI uiScript;
    public List<Objective> objectives = new List<Objective>
    {
        new Objective(LanguageSettings.orderABurger.ToString()),
        new Objective(LanguageSettings.orderAMilkshake.ToString()),
        new Objective(LanguageSettings.tipAppropriately.ToString())
    };
    private int currentObjective = 0;
    private OpenAIApi openai = new OpenAIApi("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");
    public bool finished = false;

    public void Start(){
       uiScript.InitObjectives(objectives[0]);
    }
    
    public async void CheckForCompletedObjectives(List<ChatMessage> chats)
    {
        Debug.Log("Checking for completed objectives");
        // Add the new system message
        List<ChatMessage> chatMessages = new List<ChatMessage>(chats);
        chatMessages.RemoveAt(0);
        chatMessages.Insert(0, new ChatMessage()
        {
            Role = "system",
            Content = LanguageSettings.systemMessage.ToString()
            + "\n" + LanguageSettings.currentObjectiveIs.ToString() + objectives[currentObjective].TaskDescription
        });
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0613",
            Messages = chatMessages
        });
        if (completionResponse.Choices != null)
        {
            var response = completionResponse.Choices[0].Message.Content.Trim().ToLower();
            if (response == "true")
            {
                objectives[currentObjective].IsCompleted = true;
                currentObjective++;

                uiScript.ReceiveUpdatedObjectives(objectives);

                if (currentObjective >= objectives.Count)
                {
                    GiveFeedBack(chatMessages);
                }
            }
            Debug.Log(LogObjectives());
        }
    }

    private async void GiveFeedBack(List<ChatMessage> chats)
    {
        // Add the new system message
        List<ChatMessage> chatMessages = new List<ChatMessage>(chats);
        chatMessages.RemoveAt(0);
        chatMessages.Insert(0, new ChatMessage()
        {
            Role = "system",
            Content = LanguageSettings.feedbackSystemMessage.ToString()
        });
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0613",
            Messages = chatMessages
        });
        if (completionResponse.Choices != null)
        {
            var response = completionResponse.Choices[0].Message.Content;
            Feedback feedback = new Feedback(response);
            uiScript.ReceiveFeedback(feedback);
            Debug.Log("Feedback: " +feedback);
        }
        finished = true;
    }

    private string LogObjectives()
    {
        string o = "";
        foreach (Objective objective in objectives)
        {
            o += objective.ToString() + "\n";
        }
        return o;
    }
}
