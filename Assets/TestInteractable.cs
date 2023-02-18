using UnityEngine;

public class TestInteractable : Interactable
{
    [SerializeField] private HealthController healthController = null;
    [SerializeField] private float healValue = 30.0f;

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
    }
    public override void OnInteract()
    {
        print("Healed with " + gameObject.name);
        healthController.currentPlayerHealth += healValue;
        healthController.Heal(healValue);
    }
    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
    }
}
