// using UnityEngine;
// using UnityEngine.UI;
// using Unity.Barracuda;

// public class NeuralNetworkInference : MonoBehaviour
// {
//     public NNModel modelFile; // Reference to the ONNX model asset
//     private IWorker worker; // Barracuda worker for inference

//     // Example: Reference to UI text to display inference result
//     public Text inferenceResultText;

//     void Start()
//     {
//         // Load the model from the Resources folder
//         string path = "model"; // No need to include folder name in path
//         Debug.Log("Loading model from: " + path);
//         modelFile = (NNModel)Resources.Load(path, typeof(NNModel));

//         if (modelFile == null)
//         {
//             Debug.LogError("Failed to load NNModel from Resources. Make sure the path is correct.");
//             return;
//         }

//         // Create Barracuda worker for inference
//         var model = ModelLoader.Load(modelFile);
//         worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
//     }

//     // Method for performing inference (to be triggered by UI interaction)
//     public void PerformInference()
//     {
//         // Example: Prepare input tensor (replace with your actual input data)
//         // Tensor inputTensor = new Tensor( /* Your input tensor data */ );

//         // Execute inference
//         // worker.Execute(inputTensor);

//         // Example: Simulate inference result (replace with actual inference logic)
//         float prediction = Random.Range(0f, 1f); // Placeholder for demonstration

//         if (prediction > 0.5f)
//         {
//             inferenceResultText.text = "Rust";
//         }
//         else
//         {
//             inferenceResultText.text = "Healthy";
//         }

//         // Cleanup tensors (uncomment if using actual input tensor)
//         // inputTensor.Dispose();
//     }

//     void OnDestroy()
//     {
//         // Cleanup Barracuda worker
//         if (worker != null)
//         {
//             worker.Dispose();
//         }
//     }
// }