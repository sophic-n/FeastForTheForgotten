using UnityEngine;
using TMPro;

public class InventoryHUD : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;

    void Start()
    {
        InventoryManager.Instance.OnInventoryUpdated += UpdateHUD;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        var ingredients = InventoryManager.Instance.GetIngredients();
        var dishes = InventoryManager.Instance.GetCookedDishes();

        inventoryText.text = 
            "🌿 Ingredients:\n" + string.Join("\n", ingredients) +
            "\n\n🍲 Cooked Dishes:\n" + string.Join("\n", dishes);
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryUpdated -= UpdateHUD;
    }
}
