using TMPro;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        GameManager.Instance.OnScoreAdded += OnScoreAdded; 
    }

    private void OnScoreAdded(int obj)
    {
        scoreText.text = obj.ToString() + "/20";
    }
}
