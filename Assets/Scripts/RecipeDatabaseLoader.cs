using UnityEngine;

public class RecipeDatabaseLoader : MonoBehaviour
{
    public static RecipeDatabase Instance;
    public RecipeDatabase recipeData;

    void Awake()
    {
        Instance = recipeData;
    }
}

