using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject healthBar;

    public void SetHP(float hpNormalized)
    {
        healthBar.transform.localScale = new Vector3(hpNormalized, 1f);
    }
    public IEnumerator SetHpSmoothly(float newHP)
    {
        float curHP = healthBar.transform.localScale.x;
        float changeAmt = curHP - newHP;
        while(curHP - newHP > Mathf.Epsilon)
        {
            curHP -= changeAmt * Time.deltaTime;
            healthBar.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }
        healthBar.transform.localScale = new Vector3(newHP, 1f);
    }

    public IEnumerator RestoreHpSmoothly(float newHP)
    {
        float curHP = healthBar.transform.localScale.x;
        float changeAmt = curHP + newHP;
        while (curHP + newHP > Mathf.Epsilon)
        {
            curHP += changeAmt * Time.deltaTime;
            healthBar.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }
        healthBar.transform.localScale = new Vector3(newHP, 1f);
    }
}
