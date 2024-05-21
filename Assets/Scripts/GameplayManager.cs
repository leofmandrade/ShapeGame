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

        score = 0;
        scoreText.text = score.ToString();
        StartCoroutine(Spawnscore());
    }

    #endregion

    #region GAME_LOGIC

    #endregion

    #region SCORE
    private int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AudioClip scoreSound;

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
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
            // Instantiate a random prefab from the list
            Score tempScore = Instantiate(scorePrefabs[Random.Range(0, scorePrefabs.Count)]);
            if (tempScore == null)
            {
                Debug.LogError("tempScore is null");
                yield break;
            }

            if (ScoreAnt != null)
            {
                scoreAtual = tempScore;
                ScoreAnt = tempScore;
            }
            else
            {
                if (ScoreAnt != null)
                {
                    ScoreAnt.ProxScore = tempScore;
                }
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
        // SoundManager.instance.PlaySound(gameOverSound);
        GameManager.instance.currentScore = score;
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.goToMainMenu();
    }

    #endregion
}
