using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_Skill3Ctrl : MonoBehaviour
{
    Vector3 oldPos;
    float posy;
    [SerializeField] Transform posspamfood;
    [SerializeField] GameObject player, skill3Prefabs, foods;

    private void OnEnable()
    {
        oldPos = player.transform.position;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
            skill3Prefabs.transform.position = oldPos;
        skill3Prefabs.transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = skill3Prefabs.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            CallKillWithSkill3(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            CallKillWithSkill3(other);
        }
    }

    void CallKillWithSkill3(Collider other)
    {
        other.GetComponent<Enemy>()._enemyCannotEat = true;
        foreach (var i in other.GetComponent<Enemy>().enemyChilds)
        {
            i.GetComponent<TMT_BotFollowEnemyCtrl>()._enemyCannotEat = true;
        }

        foreach (var i in other.GetComponent<Enemy>().enemyChilds)
        {
            if (i.activeSelf)
            {
                i.gameObject.SetActive(false);
                GameObject food2 = TMT_ObjectPooling._inst.TMT_GetBotFood(foods, posspamfood);
                posy = Random.Range(0, 360);
                food2.transform.position = i.transform.position;
                food2.transform.rotation = Quaternion.Euler(0, posy, 0);
                food2.SetActive(true);
                other.GetComponent<Enemy>().TMT_UpdateHp();
            }
        }
    }
}