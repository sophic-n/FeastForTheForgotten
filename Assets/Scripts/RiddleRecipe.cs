using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FeastForTheForgotten/Riddle Recipe Entry")]
public class RiddleRecipe : ScriptableObject
{
    [Header("Identity")]
    public string shadeName; // e.g., "Shade of the Poet"

    [Header("Puzzle")]
    [TextArea]
    public string poeticRiddle;
    public string recipeName;

    [Header("Ingredients Required")]
    public List<string> requiredIngredients; // names of ingredient GameObjects

    [Header("Dialogue")]
    [TextArea]
    public string dialogueCorrect;   // when correct dish is served
    [TextArea]
    public string dialogueIncorrect; // when incorrect dish is served
    [Header("Visuals")]
    public GameObject dishPrefab;

}

