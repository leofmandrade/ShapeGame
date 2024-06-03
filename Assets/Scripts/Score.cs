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
            return;
        }

        if (GameManager.instance == null)
        {
            return;
        }

        if (GameplayManager.instance.shapes == null || GameplayManager.instance.shapes.Count == 0)
        {
            return;
        }

        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
        transform.position = spawnPosition;
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

    public void IncreaseSpeed()
    {
        speed += 0.5f; // Increase speed by 0.5 units
        Debug.Log("Increased speed to: " + speed);
    }
}
