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

    private FlowerData[] currentBouquet; // List to hold the bouquet being built (contains Flower Objects that were instantiated based on FlowerData.cs).

    private int selectedSlotIndex = -1; // For tracking which slot has been selected. 

    void Start()
    {
        currentBouquet = new FlowerData[maxSlots]; // Initialize a fixed-size (static) array. 
    }

    /** 
     * Function to add a Flower Object instance by FlowerData.cs into the current bouquet. 
    **/ 
    public void AddFlower(FlowerData flower) 
    {
        // If a player selected a slot, place/replace flower directly in that slot:
        if (selectedSlotIndex != -1)
        {
            currentBouquet[selectedSlotIndex] = flower;

            selectedSlotIndex = -1; // Set selected slot index back to default (return it where it came from). 
            
            ClearAllGlows(); // Call function to clear all slot glows from selection. 
            UpdateSlotVisual(); // Update the bouquet visual to show updated slot after adding a flower.
            return; 
        }

        // Otherwise, find the first empty slot available in the current bouquet array. 
        for (int i = 0; i < currentBouquet.Length; i++)
        {
            if (currentBouquet[i] == null)
            {
                currentBouquet[i] = flower; // Add new flower to next empty slot. 
                UpdateSlotVisual(); // Update the bouquet visual to show updated slot after adding a flower.
                return; 
            }
        }
        
        Debug.Log("Bouquet is full! Remove one to add!"); 
    }

    /** 
     * Function to remove all Flower Object instances by FlowerData.cs from the current bouquet. 
    **/
    public void ClearBouquet()
    {
        for (int i = 0; i < currentBouquet.Length; i++)
        {
            currentBouquet[i] = null; // Clear the whole bouquet to start afresh. 
        }
        
        ClearAllGlows(); // Clear any glows that may be from any selected slots. 
        UpdateSlotVisual(); // Update the bouquet visual to show empty after clearing currentBouquet[] array. 
    }

    /**
     * Function to update each image slot of the visual bouquet with the respective added flower. 
    **/
    void UpdateSlotVisual()
    {
        for (int i = 0; i < bouquetSlots.Count; i++) // Loop through each of the 5 bouquet slots, 
        {
            if (i < currentBouquet.Length) // and add the visuals to the slots if the flower exists at that slot in the backend.
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
     * Function to clear all possible glows from any selected slots:
    **/ 
    void ClearAllGlows()
    {
        for (int i = 0; i < bouquetSlots.Count; i++)
        {
            bouquetSlots[i].SetGlow(false); 
        }
    }

    /**
     * Function to return the current bouquet. 
    **/
    public FlowerData[] GetBouquet()
    {
        return currentBouquet; 
    }
}