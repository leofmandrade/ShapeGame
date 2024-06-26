// ----------------------------------------------------------------------------------------------------------------------------
// DISCLAIMER
// Algumas partes deste código foram inspiradas no vídeo(https://www.youtube.com/watch?v=3i3HUkvRz9I). Agradecimentos ao autor por compartilhar conhecimentos valiosos que ajudaram no desenvolvimento deste jogo.
// ----------------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isInitialized { get; set;}
    public int currentScore { get; set; }
    private const string highScoreKey = "HighScore";
    public int highScore {
        get {
            return PlayerPrefs.GetInt(highScoreKey, 0);
        }
        set {
            PlayerPrefs.SetInt(highScoreKey, value);
        }
    }

    public UnityAction onGameEnd; // Define onGameEnd

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        currentScore = 0;
        isInitialized = false;
        // Debug.Log("Game Manager Initialized");
    }

    public void goToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void goToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }
}
