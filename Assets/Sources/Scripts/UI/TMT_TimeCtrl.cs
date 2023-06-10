using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMT_TimeCtrl : Singleton<TMT_TimeCtrl>
{
    public float tmt_time = 150, h, m;
    [SerializeField] float limitTime;
    [SerializeField] Text txtTime;
    [SerializeField] string strTime, strH, strM;

    public void TMT_ActiveTimePlay()
    {
        StartCoroutine(SetTime());
        StartCoroutine(CountTime());
    }

    IEnumerator SetTime()
    {
        while (true)
        {
            tmt_time--;
            h = (int)tmt_time / 60;
            m = (int)tmt_time % 60;

            if (h < 10)
                strH = "0" + h;
            else
                strH = h.ToString();
            if (m < 10)
                strM = "0" + m;
            else
                strM = m.ToString();
            txtTime.text = strH + ": " + strM;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator CountTime()
    {
        yield return new WaitUntil(() => tmt_time <= limitTime);
        TMT_GameManager.Instant.TMT_GameOver();
    }
}
