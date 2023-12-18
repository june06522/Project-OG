using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DPSSystem : MonoBehaviour
{

    [SerializeField] private TMP_Text totalDamageText;
    [SerializeField] private TMP_Text dpsText;
    [SerializeField] private TMP_Text maxDamageText;
    [SerializeField] private TMP_Text maxDPSText;

    private float totalDamage;
    private float damageToSec;
    private float maxDamage;
    private float maxDps;
    private float lastTime;

    private void Update()
    {
        
        CheckDPS();

        totalDamageText.text = $"누적 데미지 : {totalDamage.ToString("0.#")}";

        maxDamageText.text = $"최고 데미지 : {maxDamage.ToString("0.#")}";
        maxDPSText.text = $"최고 DPS : {maxDps.ToString("0.#")}";

    }

    private void CheckDPS()
    {

        if(Time.time - lastTime >= 1) 
        {

            float dps = damageToSec;

            if(dps > maxDps)
            {

                maxDps = dps;

            }

            lastTime = Time.time;
            dpsText.text = $"DPS : {damageToSec.ToString("0.#")}";
            damageToSec = 0;

        }

    }

    public void TakeDamage(float damage)
    {

        if(damage > maxDamage)
        {

            maxDamage = damage;

        }

        totalDamage += damage;
        damageToSec += damage;

    }

}
