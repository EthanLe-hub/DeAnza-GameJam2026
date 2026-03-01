// Ethan Le (2/24/2026):
using UnityEngine; 
using UnityEngine.UI;

/** 
 * This script controls the UI of the Play screen.
**/
public class PlayScreenUI : MonoBehaviour 
{
    [Header("UI References:")]
    [SerializeField] private GameObject playPanel; // Assign GameObject component in Inspector. 
    [SerializeField] private GameObject mainScreenPanel; // Assign GameObject component in Inspector. 
    [SerializeField] private Button closeButton; // Assign Button component in Inspector. 

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel); // Attach listener to close panel when button is clicked. 
        }
    }

    private void HidePanel()
    {
        if (playPanel != null)
        { 
            playPanel.SetActive(false); // Turn off the Play panel when Close button is clicked. 
            mainScreenPanel.SetActive(false); // Turn on main screen.
        }
    }
}