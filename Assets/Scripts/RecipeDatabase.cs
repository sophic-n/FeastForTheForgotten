using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Feast/Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    public List<RiddleRecipe> allRecipes;
}
