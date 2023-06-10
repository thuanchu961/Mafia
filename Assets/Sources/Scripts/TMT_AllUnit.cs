using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMT_AllUnit : MonoBehaviour
{
    [SerializeField] int allUnit;
    [SerializeField] Text txtUnit;

    private void Update()
    {
        allUnit = TMT_ObjectPooling._inst.TMT_GetAllGameObjectPool();
        txtUnit.text = allUnit.ToString();
    }
}
