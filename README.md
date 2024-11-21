# ARCoffeeRust
Augmented Reality application to detect coffee rust disease in coffee plants

## Project Overview

This project leverages Augmented Reality (AR) and Artificial Intelligence (AI) to develop an application for detecting and managing coffee rust disease in coffee plants. By integrating AR visualization with a deep learning model, the application enables coffee farmers to perform real-time leaf health analysis and assess the severity of coffee rust through an AR overlay.

## Features
- Real-Time Leaf Health Scanning: Detect whether a coffee leaf is healthy or affected by rust.
- AR Overlay: Display rust-affected areas with intensity levels using a red marker.
- User-Friendly Interface: Navigation through Home, About, How to Use, Coffee Plant Care, and Scan pages.
- Actionable Insights: Provides rust severity levels to guide treatment and management.

## System Requirements
- Unity Version: Ensure Unity Editor 2021 or higher.
- Dependencies: Vuforia SDK for AR and Unity Barracuda for ONNX model integration.
- Android Device: Any Android devices

## How It Works
- Launch the application and navigate to the Scan page.
- Point the AR camera at a coffee leaf and press the Scan button.
- The app analyzes the leaf's health and displays the result:
- **Healthy:** No rust detected.
- **Rust:** Rust severity highlighted with red markers.

33 Key Technologies
- Augmented Reality: Real-time AR visualization using Unity and Vuforia.
- Deep Learning: Teachable Machine model converted to ONNX for Unity integration.
- C++: Backend scripting for app functionality.

## Limitations
- Requires digital images for validation due to limited access to fresh coffee leaves.
- Not optimized for large or mature coffee trees.

## Future Work
- Scalability to larger coffee plants.
- Enhanced image segmentation to detect and classify individual rust-affected areas on larger scales.

## Acknowledgements
This project was completed as part of the final year project at Universiti Teknologi PETRONAS (UTP) under the supervision of Dr. Maged M Saeed Nasser and Dr. Khairul Shafee Kalid. Special thanks to everyone who contributed to the development and refinement of this project.

