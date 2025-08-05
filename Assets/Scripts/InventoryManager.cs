using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> collectedIngredients = new List<string>();
    private List<string> cookedDishes = new List<string>();

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // INGREDIENTS ------------
    public void AddIngredient(string ingredient)
    {
        collectedIngredients.Add(ingredient);
        Debug.Log($"Collected: {ingredient}");
        OnInventoryUpdated?.Invoke();
    }

    public List<string> GetIngredients()
    {
        return collectedIngredients;
    }

    public List<string> GetAllIngredients()
    {
        return new List<string>(collectedIngredients); // copy
    }

    public void RemoveIngredients(List<string> usedIngredients)
    {
        foreach (var ingredient in usedIngredients)
        {
            collectedIngredients.Remove(ingredient);
        }
        Debug.Log("Used ingredients: " + string.Join(", ", usedIngredients));
        OnInventoryUpdated?.Invoke();
    }

    // COOKED DISHES ------------
    public void AddDish(string dishName)
    {
        cookedDishes.Add(dishName);
        Debug.Log($"Cooked Dish: {dishName}");
        OnInventoryUpdated?.Invoke();
    }

    public bool HasDish(string dishName)
    {
        return cookedDishes.Contains(dishName);
    }

    public void RemoveDish(string dishName)
    {
        cookedDishes.Remove(dishName);
        Debug.Log($"Removed Dish: {dishName}");
        OnInventoryUpdated?.Invoke();
    }

    public List<string> GetCookedDishes()
    {
        return new List<string>(cookedDishes);
    }

    public void ClearInventory()
    {
        collectedIngredients.Clear();
        cookedDishes.Clear();
        OnInventoryUpdated?.Invoke();
    }
    // Add at bottom of InventoryManager.cs:
public bool HasAnyDish()
{
    return cookedDishes.Count > 0;
}

public string GetFirstDish()
{
    return cookedDishes.Count > 0 ? cookedDishes[0] : null;
}

}

