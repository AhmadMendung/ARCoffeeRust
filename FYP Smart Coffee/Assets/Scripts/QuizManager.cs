using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public Text[] questionTexts;        // Array to store each question's text
    public Button[] answerButtons;      // Array of all answer buttons (A, B, C, D for all questions)
    public Text feedbackText;           // Text component for feedback after the quiz
    public Button submitButton;         // Button to submit the quiz
    public Button resetButton;          // Button to reset the quiz

    private string[] correctAnswers;    // Array to store the correct answers
    private string[] selectedAnswers;   // Array to store the selected answers for each question
    private int totalQuestions = 5;     // Total number of questions in the quiz

    private Color defaultColor;         // To store the default button color
    public Color selectedColor = Color.green;  // The color when the button is selected

    void Start()
    {
        LoadQuestions();
        feedbackText.gameObject.SetActive(false);  // Hide feedback initially
        selectedAnswers = new string[totalQuestions]; // Initialize the selected answers array

        // Get the default button color (all buttons should have the same default color)
        defaultColor = answerButtons[0].GetComponent<Image>().color;

        // Add listeners to each answer button
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Capture the loop index to avoid closure issue
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }

        // Add listener to the submit button to check all answers
        submitButton.onClick.AddListener(CheckAnswers);

        // Add listener to the reset button to reset the quiz
        resetButton.onClick.AddListener(ResetQuiz);
    }

    void LoadQuestions()
    {
        // Set up the correct answers
        correctAnswers = new string[] { "B", "B", "B", "C", "C" };  // Correct answers for the 5 questions
    }

    void SelectAnswer(int buttonIndex)
    {
        // Determine which question this button belongs to
        int questionIndex = buttonIndex / 4;  // Each question has 4 buttons (A, B, C, D)

        // Reset all buttons for this question to default color
        ResetButtonColorsForQuestion(questionIndex);

        // Change the clicked button's color to indicate selection
        answerButtons[buttonIndex].GetComponent<Image>().color = selectedColor;

        // Determine the answer letter based on the button index
        string answerLetter;
        switch (buttonIndex % 4)
        {
            case 0:
                answerLetter = "A";
                break;
            case 1:
                answerLetter = "B";
                break;
            case 2:
                answerLetter = "C";
                break;
            case 3:
                answerLetter = "D";
                break;
            default:
                answerLetter = "";
                break;
        }

        // Store the selected answer for the question
        selectedAnswers[questionIndex] = answerLetter;

        Debug.Log("Selected answer for question " + (questionIndex + 1) + ": " + answerLetter);
    }

    // Reset the button colors for a specific question (questionIndex)
    void ResetButtonColorsForQuestion(int questionIndex)
    {
        // Reset the colors for all answer buttons (A, B, C, D) for the specific question
        for (int i = questionIndex * 4; i < questionIndex * 4 + 4; i++)
        {
            answerButtons[i].GetComponent<Image>().color = defaultColor;
        }
    }

    void CheckAnswers()
    {
        int score = 0;  // Track the user's score

        // Loop through each question and check if the selected answer is correct
        for (int i = 0; i < correctAnswers.Length; i++)
        {
            if (selectedAnswers[i] == correctAnswers[i])
            {
                score++;  // Increase score if the selected answer is correct
            }
        }

        // Display feedback
        feedbackText.text = "You scored " + score + " out of " + totalQuestions;
        feedbackText.gameObject.SetActive(true);
    }

    // Reset the quiz by clearing selections, resetting button colors, and hiding feedback
    public void ResetQuiz()
    {
        // Clear selected answers
        selectedAnswers = new string[totalQuestions];

        // Reset all buttons to default color
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Image>().color = defaultColor;
        }

        // Hide the feedback text
        feedbackText.gameObject.SetActive(false);

        Debug.Log("Quiz reset");
    }
}
