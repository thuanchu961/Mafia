using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool isFood;
    public bool _isFood => isFood;
    public bool _playerActiveShield = false;
    public bool _enemyCannotEat = false;
    [SerializeField] int HP, nextPos = -1;
    public int _hp => HP;
    public GameObject botEnemyParent, enemyFollow, coliderFollow;
    public Transform[] transformsmove;
    public int posCurrent;
    public string _name;
    NavMeshAgent agent;
    [SerializeField] TMP_Text txtHp, txtName;
    public List<GameObject> enemyChilds = new List<GameObject>();
    public int enemyId;
    public GameObject playerIsFood/*, playerIsHunter*/;
    // [SerializeField] bool runcode = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        HP = 1;
        _name = "Enemy" + Random.Range(0, 9999).ToString("0000");
        txtName.text = _name;
    }

    void RandomPosMove()
    {
        nextPos = Random.Range(0, transformsmove.Length);
        MoveRandomPos();
    }

    IEnumerator MoveWhenBegin()
    {
        yield return new WaitUntil(() => transformsmove.Length > 0);
        RandomPosMove();
    }

    void MoveRandomPos()
    {
        if (nextPos != posCurrent)
        {
            CallAgentMove();
        }
        else
            RandomPosMove();
    }

    void CallAgentMove()
    {
        agent.destination = transformsmove[nextPos].position;
        StartCoroutine(SetPosCurrentAgain());
    }

    IEnumerator SetPosCurrentAgain()
    {
        yield return new WaitForSeconds(1);
        posCurrent = nextPos;
    }

    // Update is called once per frame
    void Update()
    {
        txtHp.text = HP.ToString();

        if (HP <= 1)
            isFood = true;
        else
            isFood = false;

        // if (playerIsHunter != null)
        // {
        //     if (runcode)
        //     {
        //         Vector3 tempPos = transform.position - playerIsHunter.transform.position;

        //         foreach (var i in transformsmove)
        //         {
        //             if (CheckDirX(tempPos.x) == CheckDirX(i.position.x) && CheckDirZ(tempPos.z) == CheckDirZ(i.position.z))
        //             {
        //                 runcode = false;
        //                 agent.destination = i.position;
        //             }
        //         }
        //     }
        // }
        // else
        // {
        // runcode = true;
        if (playerIsFood == null)
        {
            if (coliderFollow.GetComponent<TMT_GetPosFoodForEnemy>()._nearFood != null)
            {
                agent.destination = coliderFollow.GetComponent<TMT_GetPosFoodForEnemy>()._nearFood.transform.position + transform.forward;
            }
        }
        else
        {
            agent.destination = playerIsFood.transform.position + transform.forward;
        }
        // }
    }

    bool CheckDirX(float x)
    {
        if (x >= 0)
        {
            return true;
        }
        return false;
    }

    bool CheckDirZ(float z)
    {
        if (z >= 0)
        {
            return true;
        }
        return false;
    }

    public void TMT_UpdateHp()
    {
        HP = GetCountBot() + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            StartCoroutine(MoveWhenBegin());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_enemyCannotEat)
            return;

        if (other.gameObject.layer == 15)
        {
            TMT_GetUnitEnemy(other, enemyFollow);
        }

        if (_playerActiveShield)
            return;

        if (other.gameObject.layer == 11)
        {
            if (HP > PlayerController._inst._hp)
            {
                TMT_GetUnitEnemy(other, enemyFollow);
            }
        }

        if (other.gameObject.layer == 6)
        {
            if (HP > PlayerController._inst._hp && PlayerController._inst._hp <= 1)
            {
                TMT_GetUnitEnemy(other, enemyFollow);
                playerIsFood = null;
            }
        }
    }

    public void TMT_GetUnitEnemy(Collision other, GameObject prefabs)
    {
        GameObject g = TMT_GetBotEnemy(prefabs, botEnemyParent.transform);
        if (!g.GetComponent<TMT_FollowEnemy>())
            g.AddComponent<TMT_FollowEnemy>();
        g.GetComponent<TMT_FollowEnemy>().target = botEnemyParent;
        g.GetComponent<TMT_FollowEnemy>().enemy = gameObject;
        g.GetComponent<TMT_FollowEnemy>().smoothTime = 2;
        g.GetComponent<TMT_BotFollowEnemyCtrl>().enemy = gameObject;
        g.GetComponent<TMT_BotFollowEnemyCtrl>()._enemyCannotEat = _enemyCannotEat;
        g.transform.position = other.transform.position;
        other.gameObject.SetActive(false);
        g.SetActive(true);
        coliderFollow.GetComponent<TMT_GetPosFoodForEnemy>().TMT_SetNearFood();
        TMT_SetAgentTarget();
        TMT_UpdateHp();
        PlayerController._inst.TMT_UpdateHp();
    }

    public void TMT_SetAgentTarget()
    {
        if (gameObject.activeSelf)
            agent.destination = transformsmove[nextPos].position;
    }

    public void TMT_SetIsFood(bool b)
    {
        isFood = b;
    }

    public GameObject TMT_GetBotEnemy(GameObject obj, Transform parent = null)
    {
        foreach (var i in enemyChilds)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        enemyChilds.Add(g);
        return g;
    }

    int GetCountBot()
    {
        int count = enemyChilds.Count;
        foreach (var i in enemyChilds)
        {
            if (!i.activeSelf)
                count--;
        }
        return count;
    }
}