using SO.Variables;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class HighScoreList
{
    public List<HighScoreEntry> highScores;
}


public class UIController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI score;
    [SerializeField] private IntVariable currentScore;

    [SerializeField] private GameObject putScore;
    [SerializeField] private GameObject dontPutScore;

    // Start is called before the first frame update
    private void OnEnable()
    {
        currentScore.OnValueChanged += UpdateScore;
    }

    private void OnDisable()
    {
        currentScore.OnValueChanged -= UpdateScore;
    }

    private void Start()
    {
        currentScore.Value = 0;
    }

    private void UpdateScore()
    {
        score.text = currentScore.Value.ToString();
    }


    public List<HighScoreEntry> highScores = new List<HighScoreEntry>();

    private const string HighScoresKey = "HighScores";
    private const int MaxHighScores = 5;


    public void AddHighScore(TMP_InputField playerName)
    {
        HighScoreEntry newEntry = new HighScoreEntry { playerName = playerName.text, score = currentScore.Value };

        if (highScores.Count < MaxHighScores || currentScore.Value > highScores[highScores.Count - 1].score)
        {
            highScores.Add(newEntry);
            highScores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order
            if (highScores.Count > MaxHighScores)
            {
                highScores.RemoveAt(MaxHighScores);
            }
            SaveHighScores();
        }
    }

    private void SaveHighScores()
    {
        string highScoreString = JsonUtility.ToJson(new HighScoreList { highScores = highScores });
        PlayerPrefs.SetString(HighScoresKey, highScoreString);
        PlayerPrefs.Save();
    }

    public void LoadHighScores()
    {
        if (PlayerPrefs.HasKey(HighScoresKey))
        {
            string highScoreString = PlayerPrefs.GetString(HighScoresKey);
            HighScoreList highScoreList = JsonUtility.FromJson<HighScoreList>(highScoreString);
            highScores = highScoreList.highScores;
        }
    }

    public void ShowRecordInput()
    {
        LoadHighScores();

        if(highScores.Count > 0)
        {
            if (currentScore.Value > highScores[highScores.Count - 1].score)
                putScore.SetActive(true);
            else
                dontPutScore.SetActive(true);
        }
        else
            putScore.SetActive(true);
    }

}
