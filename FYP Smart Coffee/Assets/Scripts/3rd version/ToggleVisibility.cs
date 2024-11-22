using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{
    // Reference to the Button
    public Button toggleButton;

    // Array of GameObjects to toggle
    public GameObject[] objectsToToggle;

    void Start()
    {
        // Add a listener to the button to call the ToggleObjects method when clicked
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleObjects);
        }
    }

    void ToggleObjects()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            // Toggle the active state of each GameObject
            obj.SetActive(!obj.activeSelf);
        }
    }
}
