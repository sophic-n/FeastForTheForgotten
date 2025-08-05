using UnityEngine;
using TMPro; 

public class ShadeDialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;  // Drag in the UI element
    private static ShadeDialogueUI instance;

    private void Awake()
    {
        instance = this;
        dialogueText.gameObject.SetActive(false);
    }

    public static void ShowDialogue(string message)
    {
        if (instance == null) return;
        instance.dialogueText.text = message;
        instance.dialogueText.gameObject.SetActive(true);
    }

    public static void HideDialogue()
    {
        if (instance == null) return;
        instance.dialogueText.text = "";
        instance.dialogueText.gameObject.SetActive(false);
    }
}

