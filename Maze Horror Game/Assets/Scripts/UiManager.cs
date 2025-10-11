using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] RectTransform inGameUi;
    [SerializeField] RectTransform gameOverUi;
    void Start()
    {
        Player.Instance.OnPlayerDied += OnPlayerDied;
        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(int score)
    {
        gameOverUi.gameObject.SetActive(true);
        gameOverUi.GetComponent<GameOverUi>().GameOverScore(score);
    }
   

    private void OnPlayerDied()
    {
        inGameUi.gameObject.SetActive(false);
    }
}
