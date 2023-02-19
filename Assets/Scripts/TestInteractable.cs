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

        if (healthController.currentPlayerHealth + healValue <= 100)
        {
            healthController.currentPlayerHealth += healValue;
            healthController.Heal(healValue);
        }
        else
        {
            healthController.currentPlayerHealth += 100 - healthController.currentPlayerHealth;
            healthController.Heal(100 - healthController.currentPlayerHealth);
        }
    }
    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
    }
}
