using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CookbookUI : MonoBehaviour
{
    public GameObject cookbookPanel;
    public GameObject recipeEntryPrefab;
    public Transform contentContainer;

    public List<RiddleRecipe> allRecipes;

    private bool isOpen = false;

    void Start()
    {
        cookbookPanel.SetActive(false);
        PopulateCookbook();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCookbook();
        }
    }

    void ToggleCookbook()
    {
        isOpen = !isOpen;
        cookbookPanel.SetActive(isOpen);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    void PopulateCookbook()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject); // Clean up
        }

        foreach (RiddleRecipe recipe in allRecipes)
        {
            GameObject entry = Instantiate(recipeEntryPrefab, contentContainer);
            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var text in texts)
            {
                if (text.gameObject.name == "RecipeName")
                    text.text = recipe.recipeName;

                else if (text.gameObject.name == "Ingredients")
                    text.text = "Ingredients:\n" + string.Join("\n", recipe.requiredIngredients);
            }
        }
    }
}

