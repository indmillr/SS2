using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameManager _gameManager;
    public int bestScore;
    [SerializeField]
    private Text _bestScoreText;

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _scoreText.text = "Score: " + 0;
        bestScore = PlayerPrefs.GetInt("HighScore", 0);


        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
        _bestScoreText.text = "Best: " + bestScore.ToString();
    }

    public void CheckForBestScore(int playerScore)
    {
        if(playerScore > bestScore)
        {
            bestScore = playerScore;
            PlayerPrefs.SetInt("HighScore", bestScore);
            _bestScoreText.text = "Best: " + bestScore;
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Resume Play and Hide Panel
    public void ResumePlay()
    {
        GameManager gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gm.ResumeGame();
    }

    // Back to Main Menu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }    
}
