using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_Skill4KillEnemy : MonoBehaviour
{
    float posy;
    [SerializeField] Transform posspamfood;
    [SerializeField] GameObject foods;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
            CallKillWithSkill4(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
            CallKillWithSkill4(other);
    }

    void CallKillWithSkill4(Collider other)
    {
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
