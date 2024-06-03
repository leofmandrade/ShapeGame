using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Vector3> spawnPositions;

    [HideInInspector]
    public int shapeID;
    [HideInInspector]
    public Score ProxScore;

    private bool jogoAcabado;

    private void Awake()
    {
        jogoAcabado = false;
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            // Debug.LogError("spawnPositions is null or empty");
            return;
        }

        if (GameManager.instance == null)
        {
            // Debug.LogError("GameManager.instance is null");
            return;
        }

        if (GameplayManager.instance.shapes == null || GameplayManager.instance.shapes.Count == 0)
        {
            // Debug.LogError("GameManager.instance.shapes is null or empty");
            return;
        }

        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
        transform.position = spawnPosition;
        // Debug.Log("Spawning at position: " + spawnPosition);
    }

    private void FixedUpdate()
    {
        if (jogoAcabado)
        {
            return;
        }
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Obstacle"))
        {
            GameplayManager.instance.GameOver();
        }
        if (collision.CompareTag("Block"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onGameEnd += GameEnd;
        }
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onGameEnd -= GameEnd;
        }
    }

    private void GameEnd()
    {
        jogoAcabado = true;
    }
}
