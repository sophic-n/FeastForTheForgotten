using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeManager : MonoBehaviour
{
    [Header("Shade Setup")]
    public List<GameObject> shadePrefabs;
    public Transform spawnPoint;

    private ShadeBehavior activeShade;

    [Header("Positions")]
    public List<Transform> waitingSpots;
    public List<Transform> orderingSpots;

    private int nextWaitingIndex = 0;
    private int nextOrderingIndex = 0;

    [Header("Riddle + Recipe Data")]
    public List<RiddleRecipe> riddlePool;

    private List<GameObject> activeShades = new List<GameObject>();
    private List<RiddleRecipe> usedRiddles = new List<RiddleRecipe>();

    public Transform dishSpawnPoint;
    private GameObject currentDishVisual;

    public int totalShadesToSpawn = 15;
    private int spawnedCount = 0;

    public static ShadeManager Instance;
    private ShadeBehavior currentActiveShade;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnNextShade();
    }

    public void SpawnNextShade()
    {
        if (spawnedCount >= totalShadesToSpawn)
        {
            CheckForWinCondition();
            return;
        }

        if (shadePrefabs.Count == 0 || riddlePool.Count == 0)
        {
            Debug.LogWarning("ShadeManager: Missing shade prefabs or riddle data.");
            return;
        }

        RiddleRecipe chosen;
        do
        {
            chosen = riddlePool[Random.Range(0, riddlePool.Count)];
        } while (usedRiddles.Contains(chosen) && usedRiddles.Count < riddlePool.Count);

        usedRiddles.Add(chosen);

        GameObject visualPrefab = shadePrefabs[Random.Range(0, shadePrefabs.Count)];
        GameObject shade = Instantiate(visualPrefab, spawnPoint.position, Quaternion.identity);

        Transform waitingSpot = waitingSpots[nextWaitingIndex % waitingSpots.Count];
        Transform orderingSpot = orderingSpots[nextOrderingIndex % orderingSpots.Count];

        nextWaitingIndex++;
        nextOrderingIndex++;
        spawnedCount++;

        ShadeBehavior behavior = shade.GetComponent<ShadeBehavior>();
        behavior.Initialize(chosen, waitingSpot, orderingSpot);
        behavior.SpeakRiddle();
        SetActiveShade(behavior);

        activeShades.Add(shade);
    }
    public void SpawnShadeWithRecipe(RiddleRecipe recipe)
    {
        int randomIndex = Random.Range(0, shadePrefabs.Count);
        GameObject newShade = Instantiate(shadePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        Transform waitingSpot = waitingSpots[nextWaitingIndex % waitingSpots.Count];
        Transform orderingSpot = orderingSpots[nextOrderingIndex % orderingSpots.Count];

        nextWaitingIndex++;
        nextOrderingIndex++;
        spawnedCount++;

        ShadeBehavior behavior = newShade.GetComponent<ShadeBehavior>();
        behavior.Initialize(recipe, waitingSpot, orderingSpot);
        behavior.SpeakRiddle();

        activeShades.Add(newShade);
    }



public void CallShadeToCounter()
{
    foreach (GameObject shadeObj in activeShades)
    {
        ShadeBehavior shade = shadeObj.GetComponent<ShadeBehavior>();
        if (shade.currentState == ShadeBehavior.ShadeState.Waiting && !shade.IsSatisfied())
        {
            shade.GoToCounter();
            SetActiveShade(shade); // ✅ Highlight
            break;
        }
    }
}


    public void TryServeDish(string dishName)
    {
        foreach (GameObject shadeObj in activeShades)
        {
            ShadeBehavior shade = shadeObj.GetComponent<ShadeBehavior>();
            if (shade.currentState == ShadeBehavior.ShadeState.Ordering || shade.currentState == ShadeBehavior.ShadeState.Returning)
            {
                ShowDishVisual(dishName);

                bool correct = shade.CheckDish(dishName);
                shade.ReturnToCounter();

                if (correct)
                {
                    shade.SpeakCorrectDialogue();
                    GameManager.Instance.RegisterServeResult(true);
                    StartCoroutine(FadeOutAndRemoveWithDish(shadeObj));
                }
                else
                {
                    shade.SpeakIncorrectDialogue();
                    GameManager.Instance.RegisterServeResult(false);
                    StartCoroutine(ClearDishAfterDelay(2f));
                    shade.ReturnToWaiting();
                }

                return;
            }
        }

        PopupManager.Instance.ShowPopup("No shade is ready to receive a dish.");
    }

    void ShowDishVisual(string recipeName)
    {
        if (currentDishVisual) Destroy(currentDishVisual);

        GameObject prefab = RecipeManager.Instance.GetDishPrefab(recipeName);
        if (prefab && dishSpawnPoint)
        {
            currentDishVisual = Instantiate(prefab, dishSpawnPoint.position, dishSpawnPoint.rotation);
        }
    }

    IEnumerator ClearDishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentDishVisual)
        {
            Destroy(currentDishVisual);
            currentDishVisual = null;
        }
    }

    private IEnumerator FadeOutAndRemoveWithDish(GameObject shadeObj)
    {
        yield return new WaitForSeconds(2f);

        if (currentDishVisual)
        {
            Destroy(currentDishVisual);
            currentDishVisual = null;
        }

        Renderer[] renderers = shadeObj.GetComponentsInChildren<Renderer>();
        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            foreach (Renderer r in renderers)
            {
                if (r.material.HasProperty("_Color"))
                {
                    Color c = r.material.color;
                    c.a = Mathf.Lerp(1f, 0f, timer / duration);
                    r.material.color = c;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        activeShades.Remove(shadeObj);
        Destroy(shadeObj);

        ServingUIManager.Instance.ClearServing();

        yield return new WaitForSeconds(1f);
        SpawnNextShade();
    }

    void CheckForWinCondition()
    {
        if (activeShades.Count == 0)
        {
            GameManager.Instance.RegisterServeResult(true);
        }
    }

    public void SetActiveShade(ShadeBehavior newShade)
{
    // Remove highlight from previous
    if (currentActiveShade != null)
        currentActiveShade.SetHighlighted(false);

    currentActiveShade = newShade;

    if (currentActiveShade != null)
    {
        currentActiveShade.SetHighlighted(true);
        ServingUIManager.Instance.SetCurrentServing(currentActiveShade.assignedRecipe.shadeName); // ✅ Corrected line
    }
}

}

