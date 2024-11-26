using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Player player;
    Coroutine hpanmiation;
    float curhp=100;
    float fakehp;
    void Start()
    {
        fakehp = player.getHp();
        curhp = player.getHp();
    }

    // Update is called once per frame
    void Update()
    {
        if (fakehp != player.getHp())
        {
            if (hpanmiation != null)
                StopCoroutine(hpanmiation);
            hpanmiation = StartCoroutine(CountTo(player.getHp()));
            fakehp = player.getHp();
        }
        //slider.value=player.getHp();
        text.text = Mathf.Ceil(slider.value).ToString();
        if (Mathf.Ceil(slider.value) < 20)
        {
            fill.color=Color.red;
            text.color = Color.red;
        }
        else if(Mathf.Ceil(slider.value) < 50)
        {
            fill.color=Color.yellow;
            text.color = Color.yellow;

        }
        else
        {
            fill.color = Color.green;
            text.color = Color.white;

        }
    }

    IEnumerator hpAnimation()
    {
        if (slider.value > player.getHp())
        {
            while (slider.value != player.getHp())
            {
                slider.value--;
                yield return new WaitForSeconds(0.5f);
            }
        }else if(slider.value < player.getHp())
        {
            slider.value++;
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    public float countDuration = 0.2f;

    IEnumerator CountTo(float targetValue)
    {
        var rate = Mathf.Abs(targetValue - curhp) / countDuration;
        while (curhp != targetValue)
        {
            curhp = Mathf.MoveTowards(curhp, targetValue, rate * Time.deltaTime);
            slider.value = curhp;
            text.text = ((int)curhp).ToString();
            Debug.Log("workingggggg");
            yield return null;
        }
    }

}
