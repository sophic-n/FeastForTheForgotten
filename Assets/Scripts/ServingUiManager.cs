using TMPro;
using UnityEngine;

public class ServingUIManager : MonoBehaviour
{
    public static ServingUIManager Instance;

    public TextMeshProUGUI servingText;

    private void Start()
{
    ClearServing(); // Ensures it's never blank
}


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentServing(string shadeName)
    {
        servingText.text = $"Current Serving: {shadeName}";
    }

    public void ClearServing()
    {
        servingText.text = "Current Serving: None";
    }
}
