using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class language_selector : MonoBehaviour
{
    // UI Buttons for languages
    public Button englishButton;
    public Button japaneseButton;
    public Button startButton;

    // Default language
    private string selectedLanguage = "English";

    void Start()
    {
        // Add listeners for button clicks
        englishButton.onClick.AddListener(() => SelectLanguage("English"));
        japaneseButton.onClick.AddListener(() => SelectLanguage("Japanese"));
        startButton.onClick.AddListener(StartLanguageSpecificContent);

        // Highlight the English button by default
        HighlightButton(englishButton);
    }

    void SelectLanguage(string language)
    {
        selectedLanguage = language;

        // Highlight the selected button and unhighlight the other
        if (language == "English")
        {
            HighlightButton(englishButton);
            UnhighlightButton(japaneseButton);
        }
        else if (language == "Japanese")
        {
            HighlightButton(japaneseButton);
            UnhighlightButton(englishButton);
        }
    }

    void StartLanguageSpecificContent()
    {
        // Start the language-specific content
        Debug.Log("Language selected: " + selectedLanguage);
        // Here you can load scenes or content specific to the selected language
        LanguageSettings.SelectedLanguage = selectedLanguage;
        SceneManager.LoadScene("Restaurant Scene");
    }

    void HighlightButton(Button button)
    {
        // Set the button as visually highlighted
        // This can be a color change, scale change, or any visual cue
        button.GetComponent<Image>().color = Color.blue; // Example color change
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    void UnhighlightButton(Button button)
    {
        // Set the button as not highlighted
        button.GetComponent<Image>().color = Color.white; // Revert color change
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
}
