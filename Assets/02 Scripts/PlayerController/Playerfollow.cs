using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Playerfollow : MonoBehaviour
{
    NavMeshAgent agent;
    PlayerController player;
    Transform posspam, followPlayer;
    Vector3 velocity = Vector3.zero;
    [SerializeField] Animator anim;
    [SerializeField] float smoothTime, dis;
    [SerializeField] float timeKill = 2, lastTimeKill = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController._inst;
        posspam = TMT_FollowPlayer._inst.transform;
        followPlayer = player.followPlayer;
        StartCoroutine(SetAvaID());
    }

    IEnumerator SetAvaID()
    {
        yield return new WaitUntil(() => GetComponent<TMT_AvaCtrl>()._avaId > -1);
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.lookAtPlayer);
        transform.position = Vector3.SmoothDamp(transform.position, followPlayer.position, ref velocity, smoothTime);
    }

    private void FixedUpdate()
    {
        if (lastTimeKill > 0)
            lastTimeKill -= Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (player._playerCannotEat)
            return;

        if (other.gameObject.layer == 15)
        {
            player.TMT_EffectKillFood(other);
            GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(gameObject, posspam);
            spam.transform.position = other.transform.position;
            spam.SetActive(true);
            PlayerController._inst.TMT_UpdateHp();
        }

        if (other.gameObject.layer == 16)
        {
            if (lastTimeKill > 0)
                return;

            if (other.gameObject.GetComponent<TMT_BotFollowEnemyCtrl>()._isFood)
            {
                GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(gameObject, posspam);
                spam.transform.position = other.transform.position;
                spam.SetActive(true);
                lastTimeKill = timeKill;
                other.gameObject.SetActive(false);
                other.gameObject.GetComponent<TMT_BotFollowEnemyCtrl>().enemy.GetComponent<Enemy>().TMT_UpdateHp();
                PlayerController._inst.TMT_UpdateHp();
            }
        }

        if (other.gameObject.layer == 14)
        {
            if (other.gameObject.GetComponent<Enemy>()._isFood)
            {
                StartCoroutine(KillBossEnemy(other));
                player.TMT_SoundEffectKillBossEnemy(other);
            }
        }

        if (other.gameObject.layer == 14 || other.gameObject.layer == 16)
        {
            // Hiệu ứng va chạm với enemy
            player.TMT_EffectKillEnemy(other);
        }
    }

    IEnumerator KillBossEnemy(Collision other)
    {
        float tmp = 0, num = 0;
        while (tmp < 10)
        {
            GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(gameObject, posspam);
            spam.transform.position = player.transform.position + new Vector3(0, 0, num);
            spam.SetActive(true);
            other.gameObject.SetActive(false);
            tmp++;
            num += 0.5f;
            PlayerController._inst.TMT_UpdateHp();
            yield return new WaitForSeconds(0.2f);
        }
    }
}

