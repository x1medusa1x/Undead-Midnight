using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public InventoryManager inventoryManager = null;

    private int currentValue = 0;
    private int maxValue = 100;

    private bool isHolding = false;

    private bool isWaiting = false;
    public int parameterValue = 10; // Change the parameter value as desired

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isHolding = true;
            isWaiting = false;
            InvokeRepeating("Add", 0f, 0.1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isHolding = false;
            isWaiting = true;
            CancelInvoke();
            InvokeRepeating("Subtract", 0.5f, 0.05f);
        }
    }

    private float Normalise()
    {
        return (float)currentValue / maxValue;
    }

    public void Add()
    {
        currentValue += 10;

        if (currentValue > maxValue)
        {
            currentValue = maxValue;
            inventoryManager.crafted = true;
        }
        inventoryManager.progressBar.gameObject.GetComponent<Image>().fillAmount = Normalise();
    }

    public void Subtract()
    {
        if (!isHolding)
        {
            currentValue -= 10;

            if (currentValue <= 0)
            {
                currentValue = 0;
            }
            inventoryManager.progressBar.gameObject.GetComponent<Image>().fillAmount = Normalise();
        }
        isWaiting = false;

    }

}
