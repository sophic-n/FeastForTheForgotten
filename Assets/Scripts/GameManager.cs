using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Shade Settings")]
    public int totalShadesToServe = 15;
    public int maxFailedAttempts = 5;

    private int successfulServes = 0;
    private int failedAttempts = 0;

    [Header("Shade Spawning")]
    public ShadeManager shadeManager; // Reference to your existing ShadeManager
    public List<RiddleRecipe> allRecipes;
    private List<RiddleRecipe> remainingRecipes;

    [Header("UI")]
    public GameObject winScreen;
    public GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        remainingRecipes = new List<RiddleRecipe>(allRecipes);
        SpawnNextShade();
    }

    public void RegisterServeResult(bool wasCorrect)
    {
        if (wasCorrect)
            successfulServes++;
        else
            failedAttempts++;

        if (successfulServes >= totalShadesToServe)
        {
            WinGame();
        }
        else if (failedAttempts >= maxFailedAttempts)
        {
            LoseGame();
        }
        else
        {
            SpawnNextShade();
        }
    }

    private void SpawnNextShade()
    {
        if (remainingRecipes.Count == 0) return;

        int index = Random.Range(0, remainingRecipes.Count);
        RiddleRecipe recipe = remainingRecipes[index];
        remainingRecipes.RemoveAt(index);

        shadeManager.SpawnShadeWithRecipe(recipe);
    }

    private void WinGame()
    {
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LoseGame()
    {
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadTitleScreen()
{
    SceneManager.LoadScene("TitleScreen"); // Replace with your actual title screen scene
}
    public void QuitGame()
    {
        Application.Quit();
    }
}
