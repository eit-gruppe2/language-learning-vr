using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageSettings
{
    public static string SelectedLanguage { get; set; } = "English";

    public static string whisperLanguageTag { get { return SelectedLanguage == "English" ? "en" : "de";}}

    public static LocalizedString orderABurger = new LocalizedString("Order a hamburger", "einen Hamburger bestellen");
    public static LocalizedString orderAMilkshake = new LocalizedString("Order a milkshake", "einen Milchshake bestellen");
    public static LocalizedString tipAppropriately = new LocalizedString("Tip appropriately", "angemessen Trinkgeld geben");
    
    public static LocalizedString systemMessage = new LocalizedString("Go through the chat log between the customer and the cashier at a fast food restaurant. If the current objective is met, respond with the word true only. If not, respond with the word false.", "Gehen Sie durch das Chat-Protokoll zwischen dem Kunden und dem Kassierer in einem Schnellrestaurant. Wenn das aktuelle Ziel erreicht ist, antworten Sie nur mit dem Wort true. Andernfalls antworten Sie mit dem Wort false.");
    public static LocalizedString currentObjectiveIs = new LocalizedString("The current objective is: ", "Das aktuelle Ziel ist: ");

    public static LocalizedString feedbackSystemMessage = new LocalizedString(@"Give feedback to the user acting as the customer in this conversation between a customer and a cashier in a fast food restaurant. Give a text feedback (textFeedback) with a rating from 1 to 5 on the following parameters grammar, pronunciation, expressiveness. You could assume the pronunciation is bad if there is a  lot of words that are transcribed that doesn't fit in the context. Expressiveness is how easy it is to understand and the ability of the customer to express themselves.

Example:

grammar: 2/5
pronunciation: 3/5
expressiveness: 2/5
textFeedback: Overall, the conversation was somewhat understandable despite some grammatical errors. The pronunciation was decent, but there were a few areas where it could be improved for better clarity. In terms of expressiveness, the responses were simple with room for more engagement and energy in the interaction. Keep practicing to refine these aspects!",
@"Geben Sie dem Benutzer, der als Kunde in diesem Gespräch zwischen einem Kunden und einem Kassierer in einem Schnellrestaurant auftritt, Feedback. Geben Sie ein Textfeedback (textFeedback) mit einer Bewertung von 1 bis 5 für die folgenden Parameter Grammatik, Aussprache, Ausdrucksfähigkeit. Sie könnten annehmen, dass die Aussprache schlecht ist, wenn viele Wörter transkribiert werden, die nicht in den Kontext passen. Ausdrucksfähigkeit ist, wie einfach es ist zu verstehen und die Fähigkeit des Kunden, sich auszudrücken.");

}

public class LocalizedString
{
    public string language1 { get; set; }
    public string language2 { get; set; }

    public LocalizedString(string language1, string language2)
    {
        this.language1 = language1;
        this.language2 = language2;
    }

    override
        public string ToString()
    {
        if (LanguageSettings.SelectedLanguage == "English")
        {
            return this.language1;
        }
        return this.language2;
    }
}

