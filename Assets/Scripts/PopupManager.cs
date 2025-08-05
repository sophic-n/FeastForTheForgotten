using System.Collections;
using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public float displayTime = 2.5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowPopup(string message)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayPopup(message));
    }

    private IEnumerator DisplayPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        popupPanel.SetActive(false);
    }
}
