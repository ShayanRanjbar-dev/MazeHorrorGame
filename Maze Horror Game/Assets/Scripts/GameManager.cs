using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<int> OnScoreAdded;
    [SerializeField] private Collectible collectibleObject;
    [SerializeField] private int collectibleCount = 20;
    [SerializeField] private int minDistance = 8;

    private Vector3 lastRandomPosition;
    private Vector2Int Xpos = new(-124, 0);
    private Vector2Int Zpos = new(-114, 0);
    private List<Collectible> spawnedCollectibles = new();
    private int score = 0;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < collectibleCount; i++)
        {
            SpawnCollectible();
        }
    }
    private void Update()
    {
        CollectibleAnimation();
    }
    private void SpawnCollectible() 
    {
        Vector3 randomPos = GetRandomPosition();
        if (!Physics.CheckSphere(randomPos,0.8f) &&
            IsFarEnough(randomPos))
        {
            spawnedCollectibles.Add(Instantiate(collectibleObject, randomPos, Quaternion.identity));
        }
        else 
        {
            SpawnCollectible();
        }
    }
    public Vector3 GetRandomPosition() 
    {
        int spawnXpos, spawnZpos;
        spawnXpos = UnityEngine.Random.Range(Xpos.x, Xpos.y);
        spawnZpos = UnityEngine.Random.Range(Zpos.x, Zpos.y);
        Vector3 randomPos = new(spawnXpos, 2f, spawnZpos);
        if (!(spawnXpos % 2 == 0 && spawnZpos % 2 == 0) || IsNotColliding(randomPos) || randomPos == lastRandomPosition) 
        {
            randomPos = GetRandomPosition();
        }
        lastRandomPosition = randomPos;
        return randomPos;
    }
    private bool IsNotColliding(Vector3 randomPos) 
    {
        return !NavMesh.SamplePosition(randomPos, out _, 2f, NavMesh.AllAreas);
    }
    private bool IsFarEnough(Vector3 newPos)
    {
        foreach (Collectible spawnedColl in spawnedCollectibles)
        {
            if (Vector3.Distance(newPos, spawnedColl.transform.position) < minDistance)
                return false;
        }
        return true;
    }
    private void CollectibleAnimation() 
    {

        for (int i = 0; i < spawnedCollectibles.Count; ++i) 
        {
            spawnedCollectibles[i].PlayMoveAnimation();
        }
    }
    public void RemoveCollectible(Collectible collectible) 
    {
        spawnedCollectibles.Remove(collectible);
    }
    public void GameOver() 
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.buildIndex);
    }
    public void AddScore() 
    {
        score++;
        OnScoreAdded?.Invoke(score);
        if (score >= collectibleCount) 
        {
            Application.Quit();
        }
    }
}
