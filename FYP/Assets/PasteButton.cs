using TMPro;
using UnityEngine;

public class PasteButton : MonoBehaviour
{
    public TMP_InputField inputField; // Drag your TMP Input Field here in the Inspector

    public void PasteText()
    {
        // Get the copied text from the clipboard
        string copiedText = GUIUtility.systemCopyBuffer;

        // Paste it into the input field
        inputField.text = copiedText;
    }
}