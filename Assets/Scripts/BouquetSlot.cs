// Ethan Le (2/24/2026):
using UnityEngine;
using UnityEngine.UI; 

/** 
 * Script to have the 5 slots in your bouquet act as buttons (needed for adding and removing flowers individually).
**/
public class BouquetSlot : MonoBehaviour 
{
    public int slotIndex; 
    public BouquetManager bouquet; 

    public Image glow; // Assign glow image in the Inspector. 
    public Image flowerImage; // Main image displaying the flower at that slot. 

    /**
     * Function to initialize each bouquet slot's glow to false when starting the game. 
    **/
    void Awake()
    {
        SetGlow(false); 
    }

    /**
     * Function to set a slot as clicked in the backend. 
    **/ 
    public void OnClickSlot()
    {
        bouquet.SelectSlot(slotIndex); // If a slot has been clicked, make note of it as clicked in the BouquetManager.cs script. 
    }

    /**
     * Function to set the flower's sprite at a slot when the flower is added. 
    **/ 
    public void SetFlower(FlowerData flower) 
    {
        if (flower != null) // If a flower exists at this slot in the backend, 
        {
            // then update the frontend (visual) of the slot. 
            flowerImage.sprite = flower.sprite; // Assign the Flower Object's image as the slot's image. 
            flowerImage.color = Color.white; // Ensure the slot is visible. 
        }
        else // Otherwise, visually show the slot (frontend) as empty, 
        {
            flowerImage.sprite = null; // No sprite assigned at this slot. 
            flowerImage.color = Color.white; // Blank white slot. 
        }
    }

    public void SetGlow(bool active)
    {
        glow.enabled = active; // Set the glow to either true or false to activate it. 
    }
}