using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_EnemyCheckIsFood : MonoBehaviour
{
    PlayerController player;
    Enemy enemy;
    GameObject botEnemyParent;
    List<GameObject> botFollowEnemyCtrl;
    [SerializeField] SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        enemy = GetComponentInParent<Enemy>();
        botEnemyParent = enemy.botEnemyParent;
        sphereCollider.radius = 40;
    }

    private void Update()
    {
        botFollowEnemyCtrl = enemy.enemyChilds;
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<PlayerController>();
        if (player._hp > enemy._hp)
        {
            // SetPlayerIsHunter(player.gameObject);
            SetPlayerIsFood(null);
            SetIsFood(true);
        }
        else if (player._hp < enemy._hp)
        {
            SetPlayerIsFood(player.gameObject);
            SetIsFood(false);
        }
        else
        {
            SetPlayerIsFood(null);
            SetIsFood(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        player = other.gameObject.GetComponent<PlayerController>();
        if (player._hp > enemy._hp)
        {
            // SetPlayerIsHunter(player.gameObject);
            SetPlayerIsFood(null);
            SetIsFood(true);
        }
        else if (player._hp < enemy._hp)
        {
            SetPlayerIsFood(player.gameObject);
            SetIsFood(false);
        }
        else
        {
            SetPlayerIsFood(null);
            SetIsFood(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SetPlayerIsFood(null);
        // SetPlayerIsHunter(null);
    }

    void SetIsFood(bool b)
    {
        foreach (var i in botFollowEnemyCtrl)
        {
            i.GetComponent<TMT_BotFollowEnemyCtrl>().TMT_SetIsFood(b);
        }
    }

    void SetPlayerIsFood(GameObject g)
    {
        enemy.playerIsFood = g;
    }

    // void SetPlayerIsHunter(GameObject g)
    // {
    //     enemy.playerIsHunter = g;
    // }
}