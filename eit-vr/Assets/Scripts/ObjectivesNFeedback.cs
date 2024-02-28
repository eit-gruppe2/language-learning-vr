using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesNFeedback : MonoBehaviour
{

    [SerializeField] private responsiveUI uiScript;
    public List<Objective> objectives = new List<Objective>
    {
        new Objective("Order a hamburger"),
        new Objective("Order a milkshake"),
        new Objective("Tip appropriately")
    };
    private int currentObjective = 0;
    private OpenAIApi openai = new OpenAIApi("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");

    public async void CheckForCompletedObjectives(List<ChatMessage> chats)
    {
        Debug.Log("Checking for completed objectives");
        // Add the new system message
        List<ChatMessage> chatMessages = new List<ChatMessage>(chats);
        chatMessages.RemoveAt(0);
        chatMessages.Insert(0, new ChatMessage()
        {
            Role = "system",
            Content = "Go through the chat log between the customer and the cashier at a fast food restaurant. If the current objective is met, respond with the word true only. If not, respond with the word false."
            + "\nThe current objective is: " + objectives[currentObjective].TaskDescription
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
            Content = @"Give feedback to the user acting as the customer in this conversation between a customer and a cashier in a fast food restaurant. Give a text feedback (textFeedback) with a rating from 1 to 5 on the following parameters grammar, pronunciation, expressiveness. You could assume the pronunciation is bad if there is a  lot of words that are transcribed that doesn't fit in the context. Expressiveness is how easy it is to understand and the ability of the customer to express themselves.

Example:

grammar: 2/5
pronunciation: 3/5
expressiveness: 2/5
textFeedback: Overall, the conversation was somewhat understandable despite some grammatical errors. The pronunciation was decent, but there were a few areas where it could be improved for better clarity. In terms of expressiveness, the responses were simple with room for more engagement and energy in the interaction. Keep practicing to refine these aspects!"
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
            Debug.Log(feedback);
        }
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
