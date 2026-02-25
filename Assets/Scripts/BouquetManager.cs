// Ethan Le (2/24/2026):
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

/** 
 * This script manages the bouquet the player puts together when serving a customer.
**/
public class BouquetManager : MonoBehaviour
{
    public int maxSlots = 5; // 5 total slots on the bouquet.

    public List<BouquetSlot> bouquetSlots; // Assign 5 bouquet slots (Slot1, Slot2, etc.) in the Inspector. 

    private List<FlowerData> currentBouquet = new List<FlowerData>(); // List to hold the bouquet being built (contains Flower Objects that were instantiated based on FlowerData.cs).

    private int selectedSlotIndex = -1; // For tracking which slot has been selected. 

    /** 
     * Function to add a Flower Object instance by FlowerData.cs into the current bouquet. 
    **/ 
    public void AddFlower(FlowerData flower) 
    {
        // If a player selected a slot, replace it:
        if (selectedSlotIndex != -1)
        {
            // If the player is replacing a clicked EXISTING slot with a different added flower:
            if (selectedSlotIndex < currentBouquet.Count) 
            {
                currentBouquet[selectedSlotIndex] = flower; // then replace that existing slot. 
            }

            // Otherwise, if the player is adding a flower to a clicked EMPTY slot:
            else if (currentBouquet.Count < maxSlots)
            {
                currentBouquet.Add(flower); // then add it to the next empty slot. 
            }

            selectedSlotIndex = -1; // Set selected slot index back to default (return it where it came from). 
            
            for (int i = 0; i < bouquetSlots.Count; i++) // Deactivate any glows after adding a flower. 
            {
                bouquetSlots[i].SetGlow(false); 
            }
            
            UpdateSlotVisual(); 
            return; 
        }

        // Check if the bouquet is full (already has 5 flowers):
        if (currentBouquet.Count >= maxSlots) 
        {
            Debug.Log("Bouquet is full! Remove one to add!"); 
            return;
        }

        // Normal add behavior (if no slots were selected): 
        currentBouquet.Add(flower); // Add new flower if bouquet is not full. 
        UpdateSlotVisual(); // Update the bouquet visual to show new added flower. 
    }

    /** 
     * Function to remove all Flower Object instances by FlowerData.cs from the current bouquet. 
    **/
    public void ClearBouquet()
    {
        currentBouquet.Clear(); // Clear the whole bouquet to start afresh. 
        UpdateSlotVisual(); // Update the bouquet visual to show empty after clearing List<FlowerData>. 
    }

    /**
     * Function to update each image slot of the visual bouquet with the respective added flower. 
    **/
    void UpdateSlotVisual()
    {
        for (int i = 0; i < bouquetSlots.Count; i++) // Loop through each of the 5 bouquet slots, 
        {
            if (i < currentBouquet.Count) // and add the visuals to the slots if the flower exists at that slot in the backend.
            {
                bouquetSlots[i].SetFlower(currentBouquet[i]); 
            } 
            else
            {
                bouquetSlots[i].SetFlower(null); // Otherwise, show empty slot if backend does not have a flower at this slot. 
            }
        }
    }

    /** 
     * Function to set the index of the clicked slot. 
    **/ 
    public void SelectSlot(int index)
    {
        selectedSlotIndex = index; // Assign clicked slot's index as being selected. 

        // Loop through all slots to find the slot that was selected, then set its glow to true:
        for (int i = 0; i < bouquetSlots.Count; i++)
        {
            bouquetSlots[i].SetGlow(i==selectedSlotIndex); // Returns true if we get to the slot in the sequence that was clicked. 
        }
    }

    /**
     * Function to return the current bouquet. 
    **/
    public List<FlowerData> GetBouquet()
    {
        return currentBouquet; 
    }
}