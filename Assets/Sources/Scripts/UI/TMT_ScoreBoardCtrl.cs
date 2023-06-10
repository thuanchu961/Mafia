using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_ScoreBoardCtrl : MonoBehaviour
{
    [SerializeField] bool showScorePlayer;
    [SerializeField] Transform scoreParent;
    [SerializeField] GameObject scorePlayer, scoreEnemy;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();

    private void Start()
    {
        showScorePlayer = false;
        enemyList = TMT_GameManager.Instant.enemyLists;

        GameObject goTmp = null;

        for (int i = 0; i < enemyList.Count; i++)
        {
            for (int i1 = i + 1; i1 < enemyList.Count; i1++)
            {
                if (enemyList[i].GetComponent<Enemy>()._hp < enemyList[i1].GetComponent<Enemy>()._hp)
                {
                    goTmp = enemyList[i];
                    enemyList[i] = enemyList[i1];
                    enemyList[i1] = goTmp;
                }
            }
        }

        int y = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (PlayerController._inst._hp > enemyList[i].GetComponent<Enemy>()._hp && !showScorePlayer)
            {
                GameObject g1 = Instantiate(scorePlayer, scoreParent);
                g1.GetComponent<TMT_ScoreField>().num = i + 1;
                g1.GetComponent<TMT_ScoreField>()._name = PlayerController._inst._name;
                g1.GetComponent<TMT_ScoreField>().unitCount = PlayerController._inst._hp;
                y = i + 1;
                showScorePlayer = true;
            }
            else
            {
                if (showScorePlayer)
                    y = i + 1;
                else
                    y = i;
            }
            GameObject g = Instantiate(scoreEnemy, scoreParent);
            g.GetComponent<TMT_ScoreField>().num = y + 1;
            g.GetComponent<TMT_ScoreField>()._name = enemyList[i].GetComponent<Enemy>()._name;
            g.GetComponent<TMT_ScoreField>().unitCount = enemyList[i].GetComponent<Enemy>()._hp;
        }

        if (!showScorePlayer)
        {
            GameObject g2 = Instantiate(scorePlayer, scoreParent);
            g2.GetComponent<TMT_ScoreField>().num = enemyList.Count + 1;
            g2.GetComponent<TMT_ScoreField>()._name = PlayerController._inst._name;
            g2.GetComponent<TMT_ScoreField>().unitCount = PlayerController._inst._hp;
        }
    }
}
