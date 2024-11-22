using UnityEngine;
using Unity.Barracuda;
using UnityEngine.UI;

public class RustScanner : MonoBehaviour
{
    public NNModel onnxModel;
    private IWorker worker;
    public Text resultText;
    public Button scanButton;
    public Camera arCamera;

    // Existing references for the tooltip button and info panel
    public Button tooltipButton;        // Reference to the Tooltip Button
    public GameObject infoPanel;        // Reference to the Info Panel
    public Text infoText;               // Reference to the Info Text inside the panel

    // Dashboard button and panels (but do not show if Rust)
    public Button dashboardButton;      // Reference to the Dashboard Button (for Healthy)
    public GameObject dbRustPanel;      // Reference to the DB Rust panel
    public GameObject dbHealthyPanel;   // Reference to the DB Healthy panel

    // Rust Level buttons and dashboards
    public Button rustLevel1Button;     // Reference to Rust Level 1 button
    public Button rustLevel2Button;     // Reference to Rust Level 2 button
    public Button rustLevel3Button;     // Reference to Rust Level 3 button
    public GameObject dbRust1;          // Dashboard for Rust Level 1
    public GameObject dbRust2;          // Dashboard for Rust Level 2
    public GameObject dbRust3;          // Dashboard for Rust Level 3

    // References for Rust percentage texts (legend)
    public GameObject rustText1;        // Reference for "1%-20%" text
    public GameObject rustText2;        // Reference for "21%-80%" text
    public GameObject rustText3;        // Reference for ">80%" text

    // Reference to txtPlsTap text
    public Text txtPlsTap;              // Add a reference for txtPlsTap

    private string scanResult;          // Store the result of the scan ("Rust" or "Healthy")
    private bool isHealthyDashboardVisible = false;  // Track the visibility of the Healthy dashboard

    void Start()
    {
        // Initialize the worker for the model
        worker = ModelLoader.Load(onnxModel).CreateWorker();

        // Add listener to the scan button
        scanButton.onClick.AddListener(ScanLeaf);

        // Initially hide the tooltip button, dashboard button, rust level buttons, texts, and all panels
        tooltipButton.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        dashboardButton.gameObject.SetActive(false);
        dbRustPanel.SetActive(false);
        dbHealthyPanel.SetActive(false);
        rustLevel1Button.gameObject.SetActive(false);
        rustLevel2Button.gameObject.SetActive(false);
        rustLevel3Button.gameObject.SetActive(false);
        dbRust1.SetActive(false);
        dbRust2.SetActive(false);
        dbRust3.SetActive(false);

        // Initially hide the rust text objects (1%-20%, 21%-80%, >80%)
        rustText1.SetActive(false);
        rustText2.SetActive(false);
        rustText3.SetActive(false);

        // Initially hide txtPlsTap
        txtPlsTap.gameObject.SetActive(false); // Make sure it is hidden at start

        // Add listener to the tooltip button to toggle the info panel
        tooltipButton.onClick.AddListener(ToggleInfoPanel);

        // Add listener to the dashboard button for Healthy result
        dashboardButton.onClick.AddListener(ToggleHealthyDashboard);

        // Add listeners to rust level buttons (with toggling functionality)
        rustLevel1Button.onClick.AddListener(ToggleRustLevel1Dashboard);
        rustLevel2Button.onClick.AddListener(ToggleRustLevel2Dashboard);
        rustLevel3Button.onClick.AddListener(ToggleRustLevel3Dashboard);
    }

    public void ScanLeaf()
    {
        // Capture the camera image
        RenderTexture renderTexture = new RenderTexture(256, 256, 24);
        arCamera.targetTexture = renderTexture;
        arCamera.Render();

        Texture2D texture = new Texture2D(256, 256, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        texture.Apply();

        arCamera.targetTexture = null;
        RenderTexture.active = null;

        // Preprocess the image for the model
        Tensor input = new Tensor(texture, channels: 3);

        // Execute the model
        worker.Execute(input);

        // Get the result from the model
        Tensor output = worker.PeekOutput();
        float rustProbability = output[0];
        float healthyProbability = output[1];

        // Set a confidence threshold
        float confidenceThreshold = 0.7f; // Adjust based on testing
        string result;

        if (rustProbability > healthyProbability && rustProbability > confidenceThreshold)
        {
            result = "Rust";
            scanResult = "Rust";  // Store the result for the dashboard and tooltip

            // Show rust level buttons (1, 2, 3) and hide the dashboard button
            rustLevel1Button.gameObject.SetActive(true);
            rustLevel2Button.gameObject.SetActive(true);
            rustLevel3Button.gameObject.SetActive(true);
            dashboardButton.gameObject.SetActive(false);  // No dashboard button for Rust

            // Show rust percentage texts (1%-20%, 21%-80%, >80%)
            rustText1.SetActive(true);
            rustText2.SetActive(true);
            rustText3.SetActive(true);

            // Hide healthy dashboard
            dbHealthyPanel.SetActive(false);

            // Show txtPlsTap when Rust is detected
            txtPlsTap.gameObject.SetActive(true); // Make the text visible when Rust is detected
        }
        else if (healthyProbability > rustProbability && healthyProbability > confidenceThreshold)
        {
            result = "Healthy";
            scanResult = "Healthy";  // Store the result for the dashboard and tooltip

            // Hide the healthy panel initially, it will show when the dashboard button is clicked
            dbHealthyPanel.SetActive(false);

            // Show the healthy dashboard button and hide rust level buttons and rust texts
            dashboardButton.gameObject.SetActive(true);
            rustLevel1Button.gameObject.SetActive(false);
            rustLevel2Button.gameObject.SetActive(false);
            rustLevel3Button.gameObject.SetActive(false);

            rustText1.SetActive(false);
            rustText2.SetActive(false);
            rustText3.SetActive(false);

            isHealthyDashboardVisible = false;  // Reset the state

            // Hide txtPlsTap if the leaf is Healthy
            txtPlsTap.gameObject.SetActive(false); // Hide the text when the result is Healthy
        }
        else
        {
            result = "Uncertain";
            scanResult = "Uncertain";  // Store the result for the dashboard and tooltip

            // Hide everything in case of uncertain result
            dbHealthyPanel.SetActive(false);
            rustLevel1Button.gameObject.SetActive(false);
            rustLevel2Button.gameObject.SetActive(false);
            rustLevel3Button.gameObject.SetActive(false);
            dashboardButton.gameObject.SetActive(false);

            rustText1.SetActive(false);
            rustText2.SetActive(false);
            rustText3.SetActive(false);

            // Hide txtPlsTap in case of Uncertain result
            txtPlsTap.gameObject.SetActive(false); // Ensure text is hidden for uncertain result
        }

        // Update the result text in the UI
        resultText.text = "Leaf Status: " + result;

        // Clean up resources
        input.Dispose();
        output.Dispose();
        Destroy(texture);
    }

    // This method toggles the visibility of the info panel when the tooltip button is clicked
    public void ToggleInfoPanel()
    {
        bool isActive = infoPanel.activeSelf;  // Check if the panel is active
        infoPanel.SetActive(!isActive);        // Toggle its state (show if hidden, hide if shown)

        // If showing the panel, update the text based on the scan result
        if (!isActive)
        {
            if (scanResult == "Rust")
            {
                infoText.text = "Coffee Rust Detected!\n\n" +
                    "Coffee rust is a fungal disease caused by Hemileia vastatrix. " +
                    "It weakens coffee plants by infecting the leaves, reducing their ability to produce beans. Immediate action is required if rust is detected.\n\n" +
                    "Suggested Actions:\n" +
                    "- Apply a strong fungicide\n" +
                    "- Prune infected leaves\n" +
                    "- Increase monitoring of nearby plants to prevent the spread of rust.";
            }
            else if (scanResult == "Healthy")
            {
                infoText.text = "Your Leaf is Healthy!\n\n" +
                    "Your plant is healthy and free of rust infection. Keep up the good work by following proper coffee plant maintenance to ensure continued health.\n\n" +
                    "Preventative Actions:\n" +
                    "- Regularly inspect plants for early signs of rust, especially during rainy seasons\n" +
                    "- Prune plants to allow better airflow\n" +
                    "- Consider using a preventive fungicide if rust is common in your region.";
            }
            else
            {
                infoText.text = "Scan result is uncertain. Please try scanning again or inspect the leaf visually for any signs of rust.";
            }
        }
    }

    // This method toggles the healthy dashboard visibility when the dashboard button is clicked
    public void ToggleHealthyDashboard()
    {
        isHealthyDashboardVisible = !isHealthyDashboardVisible;
        dbHealthyPanel.SetActive(isHealthyDashboardVisible);
    }

    // Toggle Rust Level 1 Dashboard
    public void ToggleRustLevel1Dashboard()
    {
        bool isActive = dbRust1.activeSelf;
        HideAllRustDashboards();
        dbRust1.SetActive(!isActive);  // Toggle the panel based on current state
    }

    // Toggle Rust Level 2 Dashboard
    public void ToggleRustLevel2Dashboard()
    {
        bool isActive = dbRust2.activeSelf;
        HideAllRustDashboards();
        dbRust2.SetActive(!isActive);  // Toggle the panel based on current state
    }

    // Toggle Rust Level 3 Dashboard
    public void ToggleRustLevel3Dashboard()
    {
        bool isActive = dbRust3.activeSelf;
        HideAllRustDashboards();
        dbRust3.SetActive(!isActive);  // Toggle the panel based on current state
    }

    // Hide all rust dashboards
    private void HideAllRustDashboards()
    {
        dbRust1.SetActive(false);
        dbRust2.SetActive(false);
        dbRust3.SetActive(false);
    }

    private void OnDestroy()
    {
        worker.Dispose();
    }
}