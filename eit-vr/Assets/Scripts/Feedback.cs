using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Feedback
{
    public int grammar { get; set; }
    public int pronunciation { get; set; }
    public int expressiveness { get; set; }
    public string generalFeedback { get; set; }

    public Feedback(int grammar, int pronunciation, int expressiveness, string generalFeedback)
    {
        this.grammar = grammar;
        this.pronunciation = pronunciation;
        this.expressiveness = expressiveness;
        this.generalFeedback = generalFeedback;
    }

    public Feedback(string chatGptResponse)
    {
        // Regular expression patterns for extracting scores and feedback
        string grammarPattern = @"grammar:\s*(\d)/5";
        string pronunciationPattern = @"pronunciation:\s*(\d)/5";
        string expressivenessPattern = @"expressiveness:\s*(\d)/5";
        string textFeedbackPattern = @"textFeedback:\s*(.*)";  // Capture everything after 'textFeedback:'

        // Extract the scores
        Match grammarMatch = Regex.Match(chatGptResponse, grammarPattern);
        Match pronunciationMatch = Regex.Match(chatGptResponse, pronunciationPattern);
        Match expressivenessMatch = Regex.Match(chatGptResponse, expressivenessPattern);
        Match textFeedbackMatch = Regex.Match(chatGptResponse, textFeedbackPattern);

        // Parse the scores (handle potential failures)
        if (grammarMatch.Success)
        {
            int tempGrammar;
            if (int.TryParse(grammarMatch.Groups[1].Value, out tempGrammar))
            {
                this.grammar = tempGrammar;
            }
        }

        if (pronunciationMatch.Success)
        {
            int tempPronunciation;
            if (int.TryParse(pronunciationMatch.Groups[1].Value, out tempPronunciation))
            {
                this.pronunciation = tempPronunciation;
            }
        }

        if (expressivenessMatch.Success)
        {
            int tempExpressiveness;
            if (int.TryParse(expressivenessMatch.Groups[1].Value, out tempExpressiveness))
            {
                this.expressiveness = tempExpressiveness;
            }
        }

        // Store the general feedback
        if (textFeedbackMatch.Success)
        {
            this.generalFeedback = textFeedbackMatch.Groups[1].Value;
        }
    }

    public override string ToString()
    {
        // Building the string representation
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Grammar Score: {grammar}/5");
        sb.AppendLine($"Pronunciation Score: {pronunciation}/5");
        sb.AppendLine($"Expressiveness Score: {expressiveness}/5");
        sb.AppendLine($"General Feedback: {generalFeedback}");

        return sb.ToString();
    }



}
