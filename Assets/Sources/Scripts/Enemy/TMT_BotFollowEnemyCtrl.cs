using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_BotFollowEnemyCtrl : MonoBehaviour
{
    [SerializeField] bool isFood = false;
    public bool _isFood => isFood;
    public GameObject enemy;
    public bool _playerActiveShield = false;
    public bool _enemyCannotEat = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 21)
        {
            transform.position = enemy.transform.position + new Vector3(0, 2, 1);
        }

        if (_enemyCannotEat)
            return;

        if (other.gameObject.layer == 15)
        {
            enemy.GetComponent<Enemy>().TMT_GetUnitEnemy(other, gameObject);
        }

        if (_playerActiveShield)
            return;

        if (other.gameObject.layer == 11)
        {
            if (enemy.GetComponent<Enemy>()._hp > PlayerController._inst._hp)
            {
                enemy.GetComponent<Enemy>().TMT_GetUnitEnemy(other, gameObject);
            }
        }

        if (other.gameObject.layer == 6)
        {
            if (enemy.GetComponent<Enemy>()._hp > PlayerController._inst._hp && PlayerController._inst._hp <= 1)
            {
                enemy.GetComponent<Enemy>().TMT_GetUnitEnemy(other, gameObject);
                enemy.GetComponent<Enemy>().playerIsFood = null;
            }
        }
    }

    public void TMT_SetIsFood(bool b)
    {
        isFood = b;
    }
}
