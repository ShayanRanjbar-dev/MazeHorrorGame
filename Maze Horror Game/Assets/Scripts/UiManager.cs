using UnityEngine;

public class UiManager : MonoBehaviour
{
    void Start()
    {
        Player.Instance.OnPlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        gameObject.SetActive(false);
    }
}
