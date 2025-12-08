using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float RestartDelay = 5f;

    // Existing
    public GameObject CompleteLevelUI;

    // New: Game over UI
    [Header("Game Over UI")]
    public GameObject gameOverPanel;         // assign GameOverPanel (disabled by default)
    public TextMeshProUGUI finalScoreText;   // assign FinalScoreText (TMP)
    public Transform player;                 // optional: used to compute score

    [Header("Options")]
    public bool autoRestart = false;         // if true, automatically restart after RestartDelay

    public void CompleteLevel()
    {
        Debug.Log("CompleteLevel called");
        CompleteLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");

            // Show the game over UI and pause
            ShowGameOver();

            // If you still want the old behaviour of auto restart after delay,
            // set autoRestart = true in Inspector.
            if (autoRestart)
            {
                Invoke(nameof(Restart), RestartDelay);
            }
        }
    }

    // Show game over UI and pause the game
    void ShowGameOver()
    {
        // compute score (example: player's z position)
        int score = 0;
        if (player != null)
        {
            score = Mathf.FloorToInt(player.position.z);
        }

        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        FindObjectOfType<MusicFader>().FadeOut();

        // pause the game
        Time.timeScale = 0f;
    }

    // Called by Restart button or by Invoke if autoRestart is true
    public void RestartLevel()
    {
        // unpause before loading
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // kept for backward compatibility with your earlier Invoke("Restart", RestartDelay);
    // but making it private to avoid inspector clutter
    void Restart()
    {
        RestartLevel();
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");   // will show in editor
        Application.Quit();       // works in build
    }

}
