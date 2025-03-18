using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Player player; // Ensure the Player class has the getHp() and getMaxHp() methods
    private Coroutine hpAnimation;
    private float curhp;
    private float fakehp;

    void Start()
    {
        // Initialize slider value based on player's health
        curhp = player.getHp();
        slider.maxValue = player.getMaxHp();
        slider.value = curhp;
        text.text = Mathf.Ceil(curhp).ToString();
        UpdateHealthUI(); // Initial UI update
    }

    void Update()
    {
        // Check if the player's health has changed
        if (fakehp != player.getHp())
        {
            if (hpAnimation != null)
                StopCoroutine(hpAnimation);
            hpAnimation = StartCoroutine(CountTo(player.getHp()));
            fakehp = player.getHp();
        }
    }

    private void UpdateHealthUI()
    {
        text.text = Mathf.Ceil(slider.value).ToString();

        if (slider.value < 20)
        {
            fill.color = Color.red;
            text.color = Color.red;
        }
        else if (slider.value < 50)
        {
            fill.color = Color.yellow;
            text.color = Color.yellow;
        }
        else
        {
            fill.color = Color.green;
            text.color = Color.white;
        }
    }

    public float countDuration = 0.2f;

    private IEnumerator CountTo(float targetValue)
    {
        float rate = Mathf.Abs(targetValue - curhp) / countDuration;
        while (!Mathf.Approximately(curhp, targetValue))
        {
            curhp = Mathf.MoveTowards(curhp, targetValue, rate * Time.deltaTime);
            slider.value = curhp;
            text.text = Mathf.Ceil(curhp).ToString();
            yield return null;
        }
        // Ensure the slider value and text are set to the exact target value at the end
        slider.value = targetValue;
        text.text = Mathf.Ceil(targetValue).ToString();
        UpdateHealthUI(); // Update UI colors based on final value
    }
}