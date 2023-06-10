using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : TMT_Singleton<PlayerController>
{
    public TMT_AvaCtrl _anim => ani;
    public float _speed => speed;
    public int _hp => HP;
    public Transform lookAtPlayer, followPlayer;
    public string _name;
    public bool _playerCannotEat = false, _playerMove = true, _enemyLock = false, _skill4Active = false;
    public GameObject _enemySkill4;

    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] int HP, tmpHp = 0;
    [SerializeField] Color colorGizmos;
    [SerializeField] float speed, oldSpeed, oldPosY;
    [SerializeField] Rigidbody rb;
    // [SerializeField] FixedJoystick joystick;
    [SerializeField] TMT_AvaCtrl ani;
    [SerializeField] AudioClip soundKillEnemy, soundKillBossEnemy;
    [SerializeField] Transform posspam;
    [SerializeField] GameObject playerFollow, boxAva;
    [SerializeField] GameObject effectTuLuc, skill3Group, skill3, skill3Zone1, skill3Zone2, skill4;
    [SerializeField] GameObject soundPrefabs, soundParent;
    [SerializeField] TMT_JoyStick joyStick;
    [SerializeField] TMT_AvaCtrl avaCtrl;
    [SerializeField] TMT_BoxDetectAllMesh boxDetectAllMesh;
    [SerializeField] float ver, hor, ver2, hor2;
    [SerializeField] TMP_Text txtHp, txtName;
    [SerializeField] float timeKill = 2, lastTimeKill = 0;
    [SerializeField] float timeJump, oldScale;
    [SerializeField] Object[] effecEnemytLists, effecFoodtLists;
    [SerializeField] List<GameObject> botPlayerTmpList = new List<GameObject>();

    private void Start()
    {
        ver2 = 0;
        hor2 = 0;
        HP = 1;
        oldSpeed = speed;
        speed = 0;
        joyStick = TMT_JoyStick._inst;
        effecEnemytLists = TMT_GameManager.Instant.effectEnemyLists;
        effecFoodtLists = TMT_GameManager.Instant.effectFoodLists;

        StartCoroutine(SetAvaID());
        StartCoroutine(SetName());
    }

    IEnumerator SetAvaID()
    {
        yield return new WaitUntil(() => avaCtrl._avaList.Count > 0);
        int i = Random.Range(0, avaCtrl._avaList.Count);
        avaCtrl._avaId = i;
        yield return new WaitUntil(() => avaCtrl._avaId != -1);
        ani = boxAva.GetComponentInChildren<TMT_AvaCtrl>();
    }

    IEnumerator SetName()
    {
        yield return new WaitUntil(() => TMT_GameManager.Instant._setPlayerName);
        _name = PlayerPrefs.GetString("player_name");
        txtName.text = _name;
    }

    public void TMT_UpdateHp()
    {
        HP = TMT_ObjectPooling._inst.TMT_SearchActiveObj(TMT_ObjectPooling._inst.BotPlayerPools) + 1;
        if (HP - tmpHp >= 10)
        {
            tmpHp = HP;
            float scale = 0.3f;
            skill3Group.transform.localScale += new Vector3(scale, scale, scale);
        }
    }

    private void Update()
    {
        txtHp.text = HP.ToString();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            ver2 = Input.GetAxis("Horizontal");
            hor2 = Input.GetAxis("Vertical");
        }
        else
        {
            ver = joyStick.TMT_GetJoyVector().x;
            hor = joyStick.TMT_GetJoyVector().y;

            if (ver != 0 || hor != 0)
            {
                ver2 = ver;
                hor2 = hor;
            }
        }

        if (_skill4Active)
        {
            if (_enemySkill4 != null)
            {
                transform.LookAt(_enemySkill4.transform);
                transform.localRotation = Quaternion.Euler(new Vector3(0, transform.localRotation.y, 0));
            }
        }
    }

    private void FixedUpdate()
    {
        if (lastTimeKill > 0)
            lastTimeKill -= Time.fixedDeltaTime;

        // if (ani == null)
        //     return;

        if (!_playerMove)
            return;

        rb.velocity = -new Vector3(ver2 * speed, rb.velocity.y, hor2 * speed);

        if (ver2 != 0 || hor2 != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            //     ani.SetBool("RUN", true);
            //     ani.SetBool("IDLE", false);
        }
        // else
        // {
        //     ani.SetBool("RUN", false);
        //     ani.SetBool("IDLE", true);
        // }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_playerCannotEat)
            return;

        if (other.gameObject.layer == 15)
        {
            TMT_EffectKillFood(other);
            GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(playerFollow, posspam);
            spam.GetComponent<TMT_AvaCtrl>()._avaId = avaCtrl._avaId;
            spam.GetComponent<TMT_AvaCtrl>()._avaList = avaCtrl._avaList;
            spam.transform.position = other.transform.position;
            spam.SetActive(true);
            TMT_UpdateHp();
        }

        if (other.gameObject.layer == 16)
        {
            if (lastTimeKill > 0)
                return;

            if (other.gameObject.GetComponent<TMT_BotFollowEnemyCtrl>()._isFood)
            {
                GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(playerFollow, posspam);
                spam.GetComponent<TMT_AvaCtrl>()._avaId = avaCtrl._avaId;
                spam.GetComponent<TMT_AvaCtrl>()._avaList = avaCtrl._avaList;
                spam.transform.position = other.transform.position;
                spam.SetActive(true);
                lastTimeKill = timeKill;
                other.gameObject.SetActive(false);
                other.gameObject.GetComponent<TMT_BotFollowEnemyCtrl>().enemy.GetComponent<Enemy>().TMT_UpdateHp();
                TMT_UpdateHp();
            }
        }

        if (other.gameObject.layer == 14)
        {
            if (other.gameObject.GetComponent<Enemy>()._isFood)
            {
                StartCoroutine(KillBossEnemy(other));
                TMT_SoundEffectKillBossEnemy(other);
            }
        }

        if (other.gameObject.layer == 14 || other.gameObject.layer == 16)
        {
            // Hiệu ứng va chạm với enemy
            TMT_EffectKillEnemy(other);
        }
    }

    IEnumerator KillBossEnemy(Collision other)
    {
        float tmp = 0, num = 0;
        while (tmp < 10)
        {
            GameObject spam = TMT_ObjectPooling._inst.TMT_GetBotPlayer(playerFollow, posspam);
            spam.GetComponent<TMT_AvaCtrl>()._avaId = avaCtrl._avaId;
            spam.GetComponent<TMT_AvaCtrl>()._avaList = avaCtrl._avaList;
            spam.transform.position = transform.position + new Vector3(0, 0, num);
            spam.SetActive(true);
            other.gameObject.SetActive(false);
            tmp++;
            num += 0.5f;
            TMT_UpdateHp();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void TMT_EffectKillEnemy(Collision other)
    {
        GameObject g = TMT_ObjectPooling._inst.TMT_GetEffectHit((GameObject)effecEnemytLists[Random.Range(0, effecEnemytLists.Length)]);
        g.transform.position = other.transform.position + new Vector3(0, 1, 0);
        g.SetActive(true);
        delayDestroy(g);
        GameObject gSound = TMT_ObjectPooling._inst.TMT_GetSound(soundPrefabs, soundParent.transform);
        gSound.GetComponent<AudioSource>().clip = soundKillEnemy;
        gSound.transform.position = other.transform.position;
        gSound.SetActive(true);
        gSound.GetComponent<AudioSource>().Play();
    }
    
    public void TMT_SoundEffectKillBossEnemy(Collision other)
    {
        StartCoroutine(DelaySoundKillBoss(other));
    }

    IEnumerator DelaySoundKillBoss(Collision other)
    {
        yield return new WaitForSeconds(1);
        GameObject gSound = TMT_ObjectPooling._inst.TMT_GetSound(soundPrefabs, soundParent.transform);
        gSound.GetComponent<AudioSource>().clip = soundKillBossEnemy;
        gSound.transform.position = other.transform.position;
        gSound.GetComponent<AudioSource>().volume = 0.5f;
        gSound.SetActive(true);
        gSound.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        gSound.GetComponent<AudioSource>().volume = 0.2f;
    }

    public void TMT_EffectKillFood(Collision other)
    {
        GameObject g = TMT_ObjectPooling._inst.TMT_GetEffectHitFood((GameObject)effecFoodtLists[Random.Range(0, effecFoodtLists.Length)]);
        g.transform.position = other.transform.position + new Vector3(0, 1, 0);
        g.SetActive(true);
        delayDestroy(g);
    }

    public void delayDestroy(GameObject e)
    {
        e.SetActive(false);
    }

   
    public void TMT_SetSpeedUp(float speedUp)
    {
        speed = speedUp;
    }

    public void TMT_ActiveSkill3()
    {
        StartCoroutine(CheckTimeRunAnim());
    }

    IEnumerator CheckTimeRunAnim()
    {
        timeJump = 0;
        GameObject g = skill3;
        GameObject gParent = g.GetComponentInParent<Transform>().gameObject;
        float timeLimit1 = 28, timeLimit2 = 16;
        while (timeJump < timeLimit1)
        {
            timeJump++;
            if (timeJump >= 2 && timeJump < timeLimit2)
            {
                effectTuLuc.SetActive(true);
            }
            if (timeJump == timeLimit2)
            {
                effectTuLuc.SetActive(false);
                g.SetActive(true);
                skill3Zone1.SetActive(true);
                skill3Zone2.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        g.SetActive(false);
        skill3Zone1.SetActive(false);
        skill3Zone2.SetActive(false);
        yield return null;
    }

    public void TMT_ActiveSkill4()
    {
        if (!_enemyLock)
            return;

        _skill4Active = true;
        rb.velocity = Vector3.zero;
        speed = 0;
        _playerMove = false;
        if (TMT_ObjectPooling._inst.BotPlayerPools.Count > 0)
        {
            foreach (var i in TMT_ObjectPooling._inst.BotPlayerPools)
            {
                if (i.activeSelf)
                {
                    botPlayerTmpList.Add(i);
                    i.SetActive(false);
                }
            }

            StartCoroutine(ScalesPlayer());
            StartCoroutine(ShowBotPlayerAfterSkill4());
        }
        else
        {
            StartCoroutine(ScalesPlayer());
        }
    }

    IEnumerator ScalesPlayer()
    {
        // TMT_Skill4Ctrl._inst._skill4Active = true;
        _enemySkill4.GetComponent<Enemy>()._enemyCannotEat = true;
        foreach (var i in _enemySkill4.GetComponent<Enemy>().enemyChilds)
        {
            i.GetComponent<TMT_BotFollowEnemyCtrl>()._enemyCannotEat = true;
        }
        rb.useGravity = false;
        capsuleCollider.enabled = false;
        float scale = 0.5f;
        oldScale = transform.localScale.x;
        boxDetectAllMesh.activeShield = true;
        _playerCannotEat = true;
        while (scale < 0.8)
        {
            scale += 0.03f;
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0);
        skill4.SetActive(true);
        yield return new WaitForSeconds(5);
        while (scale > 0.5f)
        {
            scale -= 0.03f;
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.1f);
        }
        _playerCannotEat = false;
        speed = oldSpeed;
        boxDetectAllMesh.activeShield = false;
        _playerMove = true;
        skill4.SetActive(false);
        rb.useGravity = true;
        capsuleCollider.enabled = true;
        _enemySkill4.GetComponent<Enemy>()._enemyCannotEat = false;
        foreach (var i in _enemySkill4.GetComponent<Enemy>().enemyChilds)
        {
            i.GetComponent<TMT_BotFollowEnemyCtrl>()._enemyCannotEat = false;
        }
        _skill4Active = false;
        // TMT_Skill4Ctrl._inst._skill4Active = false;
    }

    IEnumerator ShowBotPlayerAfterSkill4()
    {
        yield return new WaitUntil(() => !_playerCannotEat);
        foreach (var i in botPlayerTmpList)
        {
            i.SetActive(true);
        }
    }
}