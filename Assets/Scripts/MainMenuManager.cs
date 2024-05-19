using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text newBestText;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve speedCurve;


    private void Awake()
    {
        highScoreText.text = GameManager.instance.highScore.ToString();

        if (!GameManager.instance.isInitialized)
        {
            Debug.Log("Game Manager is not initialized");
            scoreText.gameObject.SetActive(false);
            newBestText.gameObject.SetActive(false);
        }
        else{
            Debug.Log("Game Manager is initialized");
            StartCoroutine(ShowScore());
        }
    }

    private IEnumerator ShowScore()
    {
        int temporaryScore = 0;
        scoreText.text = temporaryScore.ToString();

        int highScore = GameManager.instance.highScore;
        int currentScore = GameManager.instance.currentScore;

        if (currentScore > highScore)
        {
            newBestText.gameObject.SetActive(true);
            GameManager.instance.highScore = currentScore;
        }
        else
        {
            newBestText.gameObject.SetActive(false);
        }
        highScoreText.text = GameManager.instance.highScore.ToString();

        float speed = 1f / animationTime;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            temporaryScore = (int)(speedCurve.Evaluate(elapsedTime) * currentScore);
            scoreText.text = temporaryScore.ToString();
            yield return null;
        }

        temporaryScore = currentScore;
        scoreText.text = temporaryScore.ToString();
    }

    public void ClickedSound()
    {
        Debug.Log("Clicked");
        GameManager.instance.goToGame();
        // SoundManager.instance.PlaySound(clickSound);
    }
}
