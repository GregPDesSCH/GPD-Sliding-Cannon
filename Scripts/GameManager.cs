using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Singleton Instance
    public static GameManager gameManager;

    // UI Elements
    public Text scoreText;
    public Slider healthBar;
    public Image healthBarBackground;
    public Text statusText;

    public GameObject playAgainButtonGroup;
    public Button playAgainButton;
    public Button quitButton;


    // Game Properties and Components
    public Cannon playerCannon;
    private int playerScore;
    private Color playerHealthInitialColor = new Color(0.0f, 180.0f / 255f, 0.0f);
    public GameObject enemyGroup;
    private int numberOfEnemies = 0;
    

	// Use this for initialization
	void Start ()
    {
        // Singleton Instance assignment
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);

        // Event Listener Assignment
        playAgainButton.onClick.AddListener(() => PlayAgain());
        quitButton.onClick.AddListener(() => Application.Quit());

        // Game Initialization
        statusText.text = "";
        playerScore = 0;
        numberOfEnemies = enemyGroup.transform.childCount;
    }

    // Adds some points to the player's current score.
    public void addScore(int score)
    {
        playerScore += score;
    }

    // Keeping track of the number of enemies we have left.s
    public void DeductEnemy()
    {
        numberOfEnemies--;

        if (NoMoreEnemies())
            DisableGame();
    }

    // Update the score text on the screen.
    public void UpdateScoreText()
    {
        scoreText.text = playerScore + "";
    }

    // Update the health bar on the screen.
    public void UpdateHealthBar()
    {
        healthBar.value = ((float)playerCannon.numberOfHitpointsLeft()) / playerCannon.maximumNumberOfHitpoints();
        healthBarBackground.color = Color.Lerp(Color.red, playerHealthInitialColor, healthBar.value);
    }

    // Check to see if all the enemies are gone.
    public bool NoMoreEnemies()
    {
        return numberOfEnemies == 0;
    }

    IEnumerator addExtraPoints()
    {
        while (playerCannon.numberOfHitpointsLeft() != 0)
        {
            yield return new WaitForSeconds(0.1f);
            playerCannon.addingBonusPoints();
            playerScore += 100;
            UpdateHealthBar();
            UpdateScoreText();
        }
    }



    // Disables the game when either the cannon has no more hitpoints left, 
    // or there are no more enemies to hit.
    public void DisableGame()
    {
        if (playerCannon.isDestroyed())
        {
            statusText.text = "GAME OVER";
        }
        else if (NoMoreEnemies())
        {
            statusText.color = Color.green;
            statusText.text = "Game Clear!";
            StartCoroutine(addExtraPoints());
        }

        playAgainButtonGroup.SetActive(true);
    }

    // Reloads the scene and plays the game again.
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
