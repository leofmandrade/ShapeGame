using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    #region START


    private bool jogoAcabado;
    public static GameplayManager instance;
    public List<Sprite> shapes;


    void Awake()
    {
        instance = this;    
        jogoAcabado = false;

        GameManager.instance.isInitialized = true;

        score = 0;
        scoreText.text = score.ToString();
        StartCoroutine(Spawnscore());
    }

    #endregion


    #region GAME_LOGIC
    public void FixedUpdate(){
        if (Input.GetMouseButtonDown(0) && !jogoAcabado)
        {
            if (scoreAtual == null)
            {
                Debug.Log("scoreAtual is null");
                GameOver();
                return;
            }

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousPos2D = new Vector2(pos.x, pos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousPos2D, Vector2.zero);

            if (!hit.collider || !hit.collider.CompareTag("Block"))
            {
                Debug.Log("hit.collider is null or not a block");
                GameOver();
                return;
            }


            int currentScoreId = scoreAtual.shapeID;
            int clickedScoreId = hit.collider.GetComponent<Player>().shapeID;

            Debug.Log("currentScoreId: " + currentScoreId);
            Debug.Log("clickedScoreId: " + clickedScoreId);
            Debug.Log("--------------------");
            if (currentScoreId != clickedScoreId)
            {
                Debug.Log("AQUIIIIIIIIIII");
                GameOver();
                return;
            }


            var tempScore = scoreAtual;
            if (scoreAtual.ProxScore != null)
            {
                scoreAtual = scoreAtual.ProxScore;
            }
            Destroy(tempScore.gameObject);
        }
    }

    #endregion

    #region SCORE
    private int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AudioClip scoreSound;

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
        Debug.Log("Score: " + score);
        SoundManager.instance.PlaySound(scoreSound);
    }

    [SerializeField] private float spawntime;

    [SerializeField] public List<Score> scorePrefabs;
    private Score scoreAtual;

    private IEnumerator Spawnscore()
    {
        Score ScoreAnt = null;

        while (!jogoAcabado)
        {
            // // Instantiate a random prefab from the list
            // Score tempScore = Instantiate(scorePrefabs[Random.Range(0, scorePrefabs.Count)]);
            // if (tempScore == null)
            // {
            //     yield break;
            // }

            // if (ScoreAnt != null)
            // {
                
            //     scoreAtual = tempScore;
            //     ScoreAnt = tempScore;
            // }
            // else
            // {
            //     if (ScoreAnt != null)
            //     {
            //         ScoreAnt.ProxScore = tempScore;
            //     }
            //     ScoreAnt = tempScore;
            // }


            Score tempScore = Instantiate(scorePrefabs[Random.Range(0, scorePrefabs.Count)]);

            if (tempScore == null)
            {
                ScoreAnt = tempScore;
                scoreAtual = ScoreAnt;
            }
            else
            {
                ScoreAnt.ProxScore = tempScore;
                ScoreAnt = tempScore;
            }

            yield return new WaitForSeconds(spawntime);
        }
    }

    #endregion

    #region GAMEOVER

    public UnityAction OnGameOver;
    [SerializeField] private AudioClip gameOverSound;

    public void GameOver()
    {
        jogoAcabado = true;
        OnGameOver?.Invoke();
        SoundManager.instance.PlaySound(gameOverSound);
        GameManager.instance.currentScore = score;
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(0f);
        GameManager.instance.goToMainMenu();
    }

    #endregion
}
