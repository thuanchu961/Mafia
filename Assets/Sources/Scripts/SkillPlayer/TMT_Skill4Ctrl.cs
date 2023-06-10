using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TMT_Skill4Ctrl : TMT_Singleton<TMT_Skill4Ctrl>
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            player.GetComponent<PlayerController>()._enemyLock = true;
            player.GetComponent<PlayerController>()._enemySkill4 = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            player.GetComponent<PlayerController>()._enemyLock = true;
            player.GetComponent<PlayerController>()._enemySkill4 = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            player.GetComponent<PlayerController>()._enemyLock = false;
            player.GetComponent<PlayerController>()._enemySkill4 = null;
        }
    }
}
