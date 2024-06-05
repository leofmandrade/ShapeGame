// ----------------------------------------------------------------------------------------------------------------------------
// DISCLAIMER
// Algumas partes deste código foram inspiradas no vídeo(https://www.youtube.com/watch?v=3i3HUkvRz9I). Agradecimentos ao autor por compartilhar conhecimentos valiosos que ajudaram no desenvolvimento deste jogo.
// ----------------------------------------------------------------------------------------------------------------------------

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

    public bool estaNaRegiao;
    private bool jogoAcabado;

    private void Awake()
    {
        jogoAcabado = false;
        estaNaRegiao = false;
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
        if (collision.CompareTag("Region"))
        {
            // Debug.Log("Shape ID: " + shapeID + " entrou na região");
            estaNaRegiao = true;
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Region"))
        {
            Debug.Log("Saiu da região");
            // atualiza a variavel do GameplayManager "estaNaRegiao" para false
            estaNaRegiao = false;
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
        float speedincrement = 0.5f;
        speed += speedincrement;
        speedincrement += 0.1f;
        Debug.Log("Increased speed to: " + speed);
    }
}
