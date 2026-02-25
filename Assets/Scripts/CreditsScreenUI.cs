// Ethan Le (2/24/2026):
using UnityEngine; 
using UnityEngine.UI;

/** 
 * This script controls the UI of the Credits screen.
**/
public class CreditsScreenUI : MonoBehaviour 
{
    [Header("UI References:")]
    [SerializeField] private GameObject creditsPanel; // Assign GameObject component in Inspector. 
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
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false); // Turn off the Credits panel when Close button is clicked. 
        }
    }
}