using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI fpsText;
    private void Start()
    {
        GameManager.Instance.OnScoreAdded += OnScoreAdded;
    }
    private void Update()
    {
        fpsText.text = ((int)(1f / Time.deltaTime)).ToString(); 
    }
    private void OnScoreAdded(int obj)
    {
        scoreText.text = $"{obj}/20";
    }
}
