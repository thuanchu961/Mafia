using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_FoodCtrl : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] List<GameObject> foodList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(GetFoodList());
    }

    IEnumerator GetFoodList()
    {
        yield return new WaitUntil(() => TMT_ObjectPooling._inst.BotFoodPools.Count > 0);
        foodList = TMT_ObjectPooling._inst.BotFoodPools;
    }

    private void Update()
    {
        foreach (var i in foodList)
        {
            if (i.activeSelf)
            {
                i.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }
}