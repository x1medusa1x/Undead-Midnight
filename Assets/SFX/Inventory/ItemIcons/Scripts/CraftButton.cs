using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour, IPointerDownHandler
{

    public InventoryManager inventoryManager = null;

    private int currentValue = 0;
    private int maxValue = 100;

    private bool isCrafting = false;

    public bool hasCrafted = false;
    public int parameterValue = 10;

    private Coroutine craftingCoroutine;

    private void Update()
    {
        if (isCrafting)
        {
            if (currentValue < maxValue)
            {
                if (craftingCoroutine == null)
                {
                    craftingCoroutine = StartCoroutine(IncrementCurrentValue());
                }
            }
            else
            {
                isCrafting = false;
                hasCrafted = true;
                craftingCoroutine = null;
            }
        }
        else
        {
            if (currentValue > 0)
            {
                if (craftingCoroutine == null)
                {
                    craftingCoroutine = StartCoroutine(DecrementCurrentValue());

                }
            }
            else
            {
                hasCrafted = false;
                craftingCoroutine = null;
                if (inventoryManager.canBeCrafted == false)
                {
                    inventoryManager.craftButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private float Normalise()
    {
        return (float)currentValue / maxValue;
    }

    private IEnumerator IncrementCurrentValue()
    {
        yield return new WaitForSeconds(0.05f);
        currentValue += parameterValue;
        inventoryManager.progressBar.gameObject.GetComponent<Image>().fillAmount = Normalise();
        craftingCoroutine = null;
        if (currentValue == maxValue)
        {
            inventoryManager.HandleCraft();
            hasCrafted = true;
        }
    }

    private IEnumerator DecrementCurrentValue()
    {
        yield return new WaitForSeconds(0.02f);
        currentValue -= parameterValue;
        inventoryManager.progressBar.gameObject.GetComponent<Image>().fillAmount = Normalise();
        craftingCoroutine = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentValue == 0 && !hasCrafted)
        {
            isCrafting = true;
        }
        else if (currentValue == maxValue && hasCrafted)
        {
            currentValue = 0;
            hasCrafted = false;
        }
    }
}