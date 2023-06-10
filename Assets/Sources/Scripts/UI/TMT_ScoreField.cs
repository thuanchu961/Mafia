using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMT_ScoreField : MonoBehaviour
{
    public int unitCount, num;
    public string _name = "";
    [SerializeField] Text txtNum, txtName, txtUnitCount;

    private void Start()
    {
        StartCoroutine(SetScore());
    }

    IEnumerator SetScore()
    {
        yield return new WaitUntil(() => _name != "");
        txtNum.text = num.ToString();
        txtName.text = _name;
        txtUnitCount.text = unitCount.ToString();
    }
}
