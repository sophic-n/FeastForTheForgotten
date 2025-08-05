using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;
    public float respawnTime = 10f;

    private Collider coll;
    private MeshRenderer rend;

    void Awake()
    {
        coll = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
    }

    public void Harvest()
    {
        // Add to inventory
        InventoryManager.Instance.AddIngredient(ingredientName);
        
        // Disable visuals and collider
        coll.enabled = false;
        rend.enabled = false;

        // Start respawn coroutine
        StartCoroutine(RespawnAfterDelay());
    }

    private System.Collections.IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);

        // Reactivate visuals and collider
        coll.enabled = true;
        rend.enabled = true;
    }
}


