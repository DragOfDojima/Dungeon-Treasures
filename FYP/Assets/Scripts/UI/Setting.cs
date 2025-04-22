using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Slider soundSlider; // Reference to the Slider
    public TextMeshProUGUI audio;     // Reference to the Text
    [SerializeField] private AudioMixer audioMixer;
    public AnimationCurve volumeCurve;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the slider value and update the text
        UpdateSoundText(soundSlider.value);

    }

    // Method to update sound volume and text

    // Method to update the sound text display
    public void UpdateSoundText(float sliderValue)
    {
        // Round the slider value to the nearest integer for display
        int volumeInt = Mathf.RoundToInt(sliderValue);
        audio.text = volumeInt.ToString(); // Update the text to show the rounded volume

        // Evaluate the curve using the slider value as the input
        // Normalize the slider value (0 to 200) to fit the curve
        float mixerValue = (volumeCurve.Evaluate(sliderValue / 200))*100; // Get mixer value from curve
        Debug.Log(mixerValue);
        audioMixer.SetFloat("Volume", mixerValue); // Set the audio mixer volume
    }

    // Update is called once per frame
    void Update()
    {

    }
}
