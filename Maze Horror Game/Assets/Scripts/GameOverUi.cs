using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameOverUi : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI scoreUi;

    public void GameOverScore(int score) 
    {
        scoreUi.text = $"You Collected {score} of 20 Orbs!";
    }
    private void Start()
    {
        Debug.Log("click");
        button.onClick.AddListener(() => { GameManager.Instance.RestartGame(); });
    }
}
