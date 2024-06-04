using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    #region START

    private bool jogoAcabado;
    public static GameplayManager instance;
    public List<Sprite> shapes;
    public Canvas gameOverCanvas;
    private Button watchAdbutton;
    private bool usedExtraLife = false;
    private TMP_Text onemoretrytext;

    private List<GameObject> activeScores = new List<GameObject>();

    void Awake()
    {
        instance = this;    
        jogoAcabado = false;
        gameOverCanvas.gameObject.SetActive(false);
        GameManager.instance.isInitialized = true;
        watchAdbutton =  gameOverCanvas.transform.Find("BotaoPlayAd").GetComponent<Button>(); // Replace "BotaoPlayAd" with the actual name of your button
        onemoretrytext = gameOverCanvas.transform.Find("oneMoreTry").GetComponent<TMP_Text>();
        score = 0;
        activeScores.Clear();
        scoreText.text = score.ToString();
        StartCoroutine(Spawnscore());
        InterstitialAdExample.instance.LoadAd();
    }

    #endregion

    #region GAME_LOGIC

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !jogoAcabado)
        {
            HandleShapeClick();
        }
    }

    IEnumerator ScaleAndDestroy(GameObject objectToDestroy, float scaleFactor, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float progress = Mathf.InverseLerp(startTime, endTime, Time.time);
            objectToDestroy.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scaleFactor, progress);
            objectToDestroy.transform.Rotate(Vector3.forward, 180 * progress);
            yield return null;
        }

        Destroy(objectToDestroy);
    }

    //shape click
    IEnumerator ShapeClick(GameObject clicked, float scaleFactor, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            //decrease and increase size

            float progress = Mathf.InverseLerp(startTime, endTime, Time.time);
            clicked.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scaleFactor, progress);


            yield return null;
        }
        clicked.transform.localScale = Vector3.one;

    }

    private void HandleShapeClick()
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

        if (hit.collider == null || !hit.collider.CompareTag("Block"))
        {
            Debug.Log("No block clicked.");
            return;
        }

        Debug.Log("Shape ID: " + scoreAtual.shapeID);
        Debug.Log("Esta na regiao: " + scoreAtual.estaNaRegiao);

        if (!scoreAtual.estaNaRegiao)
        {
            Debug.Log("Shape is not in the region. Ending game.");
            GameOver();
            return;
        }
        
        int currentScoreId = scoreAtual.shapeID;
        int clickedScoreId = hit.collider.GetComponent<Player>().shapeID;
        //animate the block being clicked
        StartCoroutine(ShapeClick(hit.collider.gameObject, 0.8f, 0.1f));

        Debug.Log("currentScoreId: " + currentScoreId);
        Debug.Log("clickedScoreId: " + clickedScoreId);
        Debug.Log("--------------------");
        if (currentScoreId != clickedScoreId)
        {
            Debug.Log("Shape IDs do not match. Ending game.");
            GameOver();
            return;
        }

        var tempScore = scoreAtual;
        scoreAtual = scoreAtual.ProxScore; // Update scoreAtual before destroying the current score
        //change scale before destroy
        // tempScore.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        // Destroy(tempScore.gameObject);
        StartCoroutine(ScaleAndDestroy(tempScore.gameObject, 0.1f, 0.1f));
        UpdateScore();

        // Check if there are more shapes to prevent null reference
        if (scoreAtual == null)
        {
            if (activeScores.Count > 0)
            {
                scoreAtual = activeScores[0].GetComponent<Score>();
            }
            else
            {
                Debug.Log("No more scores. Ending game.");
                GameOver();
                return;
            }
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
        IncreaseDifficulty();
    }

    [SerializeField] private float spawntime;

    [SerializeField] public List<Score> scorePrefabs;
    private Score scoreAtual;
    private bool isSpawning = false;


    private IEnumerator Spawnscore()
    {
        isSpawning = true;
        Score ScoreAnt = null;

        while (!jogoAcabado)
        {   
            int random = Random.Range(0, scorePrefabs.Count);
            Score tempScore = Instantiate(scorePrefabs[random]);
            tempScore.shapeID = random; // Ensure correct shapeID assignment
            Debug.Log("Spawning shapeID: " + tempScore.shapeID);
            activeScores.Add(tempScore.gameObject);

            if (ScoreAnt == null)
            {
                scoreAtual = tempScore;
            }
            else
            {
                ScoreAnt.ProxScore = tempScore;
            }

            ScoreAnt = tempScore;

            yield return new WaitForSeconds(spawntime);
        }

        isSpawning = false;
    }

    private void IncreaseDifficulty()
    {
        if (score % 5 == 0 && score < 142) // Increase difficulty every 5 points
        {
            spawntime = Mathf.Max(0.5f, spawntime - 0.1f); // Decrease spawn time but not below 0.5 seconds

            foreach (var scoreObj in activeScores)
            {
                var scoreScript = scoreObj.GetComponent<Score>();
                if (scoreScript != null)
                {
                    scoreScript.IncreaseSpeed();
                }
            }
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
        GameManager.instance.currentScore = score;
        SoundManager.instance.PlaySound(gameOverSound);

        foreach (GameObject score in activeScores)
        {
            Destroy(score);
        }
        activeScores.Clear();
        scoreAtual = null;

        gameOverCanvas.gameObject.SetActive(true);
        if (usedExtraLife)
        {
            watchAdbutton.gameObject.SetActive(false);
            onemoretrytext.gameObject.SetActive(false);
        }
        else
        {
            watchAdbutton.gameObject.SetActive(true);
            onemoretrytext.gameObject.SetActive(true);
        }
        PauseGame();
    }

    private IEnumerator GameOverRoutine()
    {

        
        
        yield return new WaitForSeconds(0f);
        GameManager.instance.goToMainMenu();
        jogoAcabado = false;
        gameOverCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        usedExtraLife = false;
    }

    #endregion

    #region PAUSE_RESUME

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        jogoAcabado = false;
        gameOverCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        usedExtraLife = true;
        Debug.Log("USED EXTRA LIFE:");

        if (!isSpawning)
        {
            StartCoroutine(Spawnscore());
        }
    }

    public void OnWatchAdButtonClicked()
    {

        InterstitialAdExample.instance.ShowAd();
    }

    public void OnMainMenuButtonClicked()
    {
        StartCoroutine(GameOverRoutine());
    }

    #endregion
}

