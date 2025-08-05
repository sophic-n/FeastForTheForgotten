using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    public List<RiddleRecipe> allRecipes = new List<RiddleRecipe>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject GetDishPrefab(string recipeName)
    {
        foreach (var recipe in allRecipes)
        {
            if (recipe.recipeName == recipeName)
                return recipe.dishPrefab;
        }
        return null;
    }

    public RiddleRecipe GetRecipeByName(string name)
    {
        foreach (var recipe in allRecipes)
        {
            if (recipe.recipeName == name)
                return recipe;
        }
        return null;
    }
}
