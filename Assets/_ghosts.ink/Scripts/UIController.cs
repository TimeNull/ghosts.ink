using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI score;
    [SerializeField] private IntVariable currentScore;

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
}
