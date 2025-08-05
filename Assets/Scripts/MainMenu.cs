using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject instructionsScreen;
    public GameObject playerController; // (Optional) If player starts disabled

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optionally disable player movement/input here
        if (playerController != null)
            playerController.SetActive(false);
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        titleScreen.SetActive(false); // Hide the title menu
        if (playerController != null)
            playerController.SetActive(true); // Enable player movement if needed
    }

    public void ShowInstructions()
    {
        titleScreen.SetActive(false);
        instructionsScreen.SetActive(true);
    }

    public void BackToTitle()
    {
        titleScreen.SetActive(true);
        instructionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

