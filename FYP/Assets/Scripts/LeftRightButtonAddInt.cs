using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftRightButtonAddInt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private int minValue = 0; // Minimum value limit
    [SerializeField] private int maxValue = 10; // Maximum value limit
    public void Increment()
    {
        int currentNumber = int.Parse(numberText.text); // Parse current number from text
        if (currentNumber < maxValue) // Check if it is less than the max value
        {
            currentNumber++; // Increment the number
            UpdateNumberText(currentNumber); // Update the display
        }
    }

    // Function to decrement the number by 1
    public void Decrement()
    {
        int currentNumber = int.Parse(numberText.text); // Parse current number from text
        if (currentNumber > minValue) // Check if it is greater than the min value
        {
            currentNumber--; // Decrement the number
            UpdateNumberText(currentNumber); // Update the display
        }
    }

    // Update the TextMeshPro text to display the current number
    private void UpdateNumberText(int number)
    {
        numberText.text = number.ToString(); // Convert the number back to string and update the text
    }
}
