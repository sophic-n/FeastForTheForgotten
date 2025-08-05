using System.Collections.Generic;
using UnityEngine;

public class CookingPot : MonoBehaviour
{
    public float interactRange = 3f;
    public List<string> currentIngredients = new List<string>();
    public GameObject player;

    [Header("Reference to the Recipe Database")]
    public RecipeDatabase recipeDatabase; // Add this line


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
            playerInteraction.ShowPrompt("Press E to Cook");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
            playerInteraction.HidePrompt();
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < interactRange &&
            Input.GetKeyDown(KeyCode.E))
        {
            AttemptCook();
        }
    }

    public void AttemptCook()
{
    List<string> playerIngredients = InventoryManager.Instance.GetAllIngredients();

    foreach (RiddleRecipe recipe in recipeDatabase.allRecipes)
    {
        if (MatchRecipe(recipe, playerIngredients))
        {
            InventoryManager.Instance.RemoveIngredients(recipe.requiredIngredients);
            InventoryManager.Instance.AddDish(recipe.recipeName);

            PopupManager.Instance.ShowPopup($"You cooked {recipe.recipeName}!");
            ShadeManager.Instance.CallShadeToCounter();
            currentIngredients.Clear();
            return;
        }
    }

    PopupManager.Instance.ShowPopup("No matching recipe found.");
}


    private bool MatchRecipe(RiddleRecipe recipe, List<string> inventoryIngredients)
    {
        foreach (string ingredient in recipe.requiredIngredients)
        {
            if (!inventoryIngredients.Contains(ingredient))
                return false;
        }
        return true;
    }
}
