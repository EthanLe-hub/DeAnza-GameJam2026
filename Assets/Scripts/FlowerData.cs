// Ethan Le (2/24/2026):
using UnityEngine; 

/** 
 * This script sets up the Data Assets (think of Objects in OOP) for multiple flowers. 
**/
[CreateAssetMenu(fileName = "New Flower", menuName = "Flower Shop/Flower")]
// Right-click in Project tab, find "Flower Shop", then click "Flower". 
// A new ScriptableObject called "New Flower" will be created. 
public class FlowerData : ScriptableObject 
{
    // Variables for each object (each flower): 
    public string flowerName; 
    public string color;
    public Sprite sprite; 
    public int cost; 
    public string meaning; 
}