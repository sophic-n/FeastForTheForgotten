using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactLayer;
    public TextMeshProUGUI interactionPrompt;

    private GameObject currentTarget;

    void Update()
    {
        CheckForInteractable();

        if (currentTarget && Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget.CompareTag("Ingredient"))
            {
                currentTarget.GetComponent<Ingredient>().Harvest();
                HidePrompt();
            }
            else if (currentTarget.CompareTag("CookingPot"))
            {
                currentTarget.GetComponent<CookingPot>().AttemptCook();
                HidePrompt();
            }
            else if (currentTarget.CompareTag("Counter"))
            {
                TryServeDish();
                HidePrompt();
            }
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.CompareTag("Ingredient"))
            {
                currentTarget = hit.collider.gameObject;
                interactionPrompt.text = "Press E to Harvest";
                interactionPrompt.gameObject.SetActive(true);
                return;
            }
            else if (hit.collider.CompareTag("CookingPot"))
            {
                currentTarget = hit.collider.gameObject;
                interactionPrompt.text = "Press E to Cook";
                interactionPrompt.gameObject.SetActive(true);
                return;
            }
            else if (hit.collider.CompareTag("Counter"))
            {
                currentTarget = hit.collider.gameObject;
                interactionPrompt.text = "Press E to Serve";
                interactionPrompt.gameObject.SetActive(true);
                return;
            }
        }

        currentTarget = null;
        HidePrompt();
    }

    public void HidePrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }

    public void ShowPrompt(string message)
    {
        interactionPrompt.text = message;
        interactionPrompt.gameObject.SetActive(true);
    }

    void TryServeDish()
    {
        if (!InventoryManager.Instance.HasAnyDish())
        {
            PopupManager.Instance.ShowPopup("You have no dishes to serve.");
            return;
        }

        string dish = InventoryManager.Instance.GetFirstDish();
        ShadeManager.Instance.TryServeDish(dish);
        InventoryManager.Instance.RemoveDish(dish);
    }
}


