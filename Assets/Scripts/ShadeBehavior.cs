using UnityEngine;
using UnityEngine.AI;

public class ShadeBehavior : MonoBehaviour
{
    public enum ShadeState { Ordering, Waiting, Returning }
    public ShadeState currentState = ShadeState.Ordering;

    public RiddleRecipe assignedRecipe;

    private bool hasBeenServed = false;
    private Transform waitingSpot;
    private Transform orderingSpot;
    private NavMeshAgent agent;

    public float triggerRadius = 3f;
    private Transform player;
    private bool playerInRange = false;

    public Quaternion orderingRotation = Quaternion.Euler(0, 180f, 0);
    public Quaternion waitingRotation = Quaternion.Euler(0, 90f, 0);
    public Quaternion returningRotation = Quaternion.Euler(0, 180f, 0);

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindWithTag("Player")?.transform;
        if (orderingSpot == null) orderingSpot = transform;
    }

    public void Initialize(RiddleRecipe recipe, Transform waitingLocation, Transform orderingLocation)
    {
        assignedRecipe = recipe;
        waitingSpot = waitingLocation;
        orderingSpot = orderingLocation;
        currentState = ShadeState.Ordering;
        MoveTo(orderingSpot.position, orderingRotation);
    }

    private void Update()
    {
        if (player == null || assignedRecipe == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool nowInRange = distance <= triggerRadius;

        if (nowInRange && !playerInRange)
        {
            playerInRange = true;

            if (currentState == ShadeState.Ordering || currentState == ShadeState.Waiting)
            {
                SpeakRiddle();

                if (currentState == ShadeState.Ordering)
                    OnPlayerHeardRiddle();
            }
            else if (currentState == ShadeState.Returning)
            {
                ShadeDialogueUI.ShowDialogue($"{assignedRecipe.shadeName} says:\n\"{GetResultDialogue()}\"");
            }
        }
        else if (!nowInRange && playerInRange)
        {
            playerInRange = false;
            ShadeDialogueUI.HideDialogue();
        }
    }

    public void SpeakRiddle()
    {
        ShadeDialogueUI.ShowDialogue($"{assignedRecipe.shadeName} says:\n\"{assignedRecipe.poeticRiddle}\"");
    }

    public void SpeakCorrectDialogue()
    {
        PopupManager.Instance.ShowPopup($"{assignedRecipe.shadeName} says:\n\"{assignedRecipe.dialogueCorrect}\"");
    }

    public void SpeakIncorrectDialogue()
    {
        PopupManager.Instance.ShowPopup($"{assignedRecipe.shadeName} says:\n\"{assignedRecipe.dialogueIncorrect}\"");
    }

    public void MoveTo(Vector3 position, Quaternion targetRotation)
    {
        if (agent != null)
        {
            agent.SetDestination(position);
        }
        else
        {
            transform.position = position;
        }

        transform.rotation = targetRotation;
    }

    public void OnPlayerHeardRiddle()
    {
        if (currentState == ShadeState.Ordering)
        {
            currentState = ShadeState.Waiting;
            MoveTo(waitingSpot.position, waitingRotation);
        }
    }

    public void ReturnToWaiting()
    {
        currentState = ShadeState.Waiting;
        MoveTo(waitingSpot.position, waitingRotation);
    }

    public void ReturnToCounter()
    {
        currentState = ShadeState.Returning;
        MoveTo(orderingSpot.position, returningRotation);
    }

    public bool CheckDish(string servedDish)
    {
        if (assignedRecipe != null && servedDish == assignedRecipe.recipeName)
        {
            hasBeenServed = true;
            return true;
        }
        return false;
    }

    public string GetResultDialogue()
    {
        if (assignedRecipe == null) return "";
        return hasBeenServed ? assignedRecipe.dialogueCorrect : assignedRecipe.dialogueIncorrect;
    }

    public bool IsSatisfied()
    {
        return hasBeenServed;
    }

    public void GoToCounter()
    {
        currentState = ShadeState.Returning;
        MoveTo(orderingSpot.position, returningRotation);
    }

public GameObject highlightEffect; // Assign an outline object or glow child

public void SetHighlighted(bool highlighted)
{
    if (highlightEffect != null)
        highlightEffect.SetActive(highlighted);
}


}

