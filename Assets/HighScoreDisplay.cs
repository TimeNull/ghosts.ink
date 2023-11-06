using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public UIController ui;

    private void Awake()
    {
        ui.LoadHighScores();
    }

    private void OnEnable()
    {
        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        string text = "High Scores:\n";

        for (int i = 0; i < ui.highScores.Count; i++)
        {
            text += $"{i + 1}. {ui.highScores[i].playerName}: {ui.highScores[i].score}\n";
        }

        highScoreText.text = text;
    }
}