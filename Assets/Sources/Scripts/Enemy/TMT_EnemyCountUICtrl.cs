using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMT_EnemyCountUICtrl : MonoBehaviour
{
    [SerializeField] GameObject UiCountEnemyPrefabs;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] List<GameObject> UiCountEnemyList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CheckEnemyListCount());
    }

    IEnumerator CheckEnemyListCount()
    {
        yield return new WaitUntil(() => TMT_GameManager.Instant.enemyLists.Count > 0);
        enemyList = TMT_GameManager.Instant.enemyLists;

        foreach (var i in enemyList)
        {
            GameObject g = Instantiate(UiCountEnemyPrefabs, transform);
            g.GetComponentInChildren<TMP_Text>().text = i.GetComponent<Enemy>()._hp.ToString();
            UiCountEnemyList.Add(g);
        }
    }

    private void Update()
    {
        ChangePosEnemyToPosUi();
        for (int i = 0; i < UiCountEnemyList.Count; i++)
        {
            UiCountEnemyList[i].GetComponentInChildren<TMP_Text>().text = enemyList[i].GetComponent<Enemy>()._hp.ToString();
        }
    }

    void ChangePosEnemyToPosUi()
    {
        // Debug.Log(enemyList[0].transform.position);
        // Debug.Log(UiCountEnemyList[0].GetComponent<RectTransform>().rect.position);
    }
}
