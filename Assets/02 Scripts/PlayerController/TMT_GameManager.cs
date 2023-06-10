using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMT_GameManager : Singleton<TMT_GameManager>
{
    float posx, posz, posy;

    public Transform spawmenemy;
    public Transform spawmPlayer;
    public Transform player;
    public bool _setPlayerName = false;

    [SerializeField] float timeGame;
    [SerializeField] Button btnPlay;
    [SerializeField] GameObject environmentParent, pnlScoreBoard, skillGroupParent;
    [SerializeField] TMT_GroupSkill[] groupSkills;
    [SerializeField] InputField inputName;
    [SerializeField] Transform posspamfood;
    [SerializeField] Transform posspamenemy;
    [SerializeField] float spawnTimeBot, limitSpawnBotNumber, countFoodBegin, numFoodAddList = 50;
    [SerializeField] GameObject soundPrefads, soundParent;
    [SerializeField] AudioClip soundWin;

    public Transform[] transformSpawnEnemy;
    public List<GameObject> enemyLists = new List<GameObject>();
    public Object[] effectEnemyLists, effectFoodLists;
    public List<GameObject> foods = new List<GameObject>();

    [SerializeField] List<Transform> tempTransformSpawn = new List<Transform>();
    [SerializeField] Object[] enemys, enemyFollows;
    [SerializeField] List<Color> colorChar = new List<Color>();
    [SerializeField] UnityEvent event1, event2;
    int zPos = 150, xPos = 120;


    // Start is called before the first frame update
    void Start()
    {
        TMT_ResumeGame();
        groupSkills = skillGroupParent.GetComponentsInChildren<TMT_GroupSkill>();
        groupSkills[PlayerPrefs.GetInt("skill")].gameObject.GetComponent<Button>().Select();
        transformSpawnEnemy = posspamenemy.GetComponentsInChildren<Transform>();
        enemys = Resources.LoadAll("PrefabsEnemy/BoxCharacter", typeof(GameObject));
        enemyFollows = Resources.LoadAll("PrefabsEnemy/FollowCharacter", typeof(GameObject));
        //GameObject environment = Resources.Load<GameObject>("PrefabsBuilding/Environment2");
        //Instantiate(environment, environmentParent.transform);
        GameObject foodBoy = Resources.Load<GameObject>("PrefabsFood/BoyFood");
        effectEnemyLists = Resources.LoadAll("PrefabsEffect/enemy", typeof(GameObject));
        effectFoodLists = Resources.LoadAll("PrefabsEffect/food", typeof(GameObject));

        btnPlay.interactable = false;

        for (int i = 0; i < numFoodAddList; i++)
        {
            foods.Add(foodBoy);
        }

        for (int i = 0; i < enemys.Length + 1; i++)
        {
            colorChar.Add(RandomColor());
        }

        SpawnFoodBegin();

        StartCoroutine(spamfood());
        StartCoroutine(CallLobby());
        StartCoroutine(CheckPlayerExists());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Time.timeScale = 0;
        if (Input.GetKey(KeyCode.C))
            Time.timeScale = 1;
    }

    IEnumerator CheckPlayerExists()
    {
        yield return new WaitUntil(() => !player.gameObject.activeSelf);
        TMT_GameOver();
    }

    void SpawnFoodBegin()
    {
        for (int ii = 0; ii < countFoodBegin; ii++)
        {
            for (int i = 0; i < foods.Count; i++)
            {
                GameObject food1 = TMT_ObjectPooling._inst.TMT_GetBotFood(foods[i], posspamfood);
                posx = Random.Range(-xPos + posspamfood.position.x, xPos + posspamfood.position.x);
                posz = Random.Range(-zPos + posspamfood.position.z, zPos + posspamfood.position.z);
                posy = Random.Range(0, 360);
                food1.transform.position = new Vector3(posx, posspamfood.position.y, posz);
                food1.transform.rotation = Quaternion.Euler(0, posy, 0);
                int i1 = food1.GetComponentsInChildren<Renderer>().Length;
                if (i1 > 1)
                {
                    foreach (Renderer i2 in food1.GetComponentsInChildren<Renderer>())
                    {
                        i2.enabled = false;
                    }
                }
                else
                    food1.GetComponentInChildren<Renderer>().enabled = false;
                food1.SetActive(true);
            }
        }

        int x = 0;
        foreach (var i in TMT_ObjectPooling._inst.BotFoodPools)
        {
            if (x > (TMT_ObjectPooling._inst.BotFoodPools.Count / 2))
                return;

            if (i.activeSelf)
            {
                i.SetActive(false);
            }
            x++;
        }
    }

    Color RandomColor()
    {
        return Random.ColorHSV(0, 1);
    }

    IEnumerator spamfood()
    {
        while (true)
        {
            for (int i = 0; i < foods.Count; i++)
            {
                GameObject food1 = TMT_ObjectPooling._inst.TMT_GetBotFood(foods[i], posspamfood);
                posx = Random.Range(-xPos + posspamfood.position.x, xPos + posspamfood.position.x);
                posz = Random.Range(-zPos + posspamfood.position.z, zPos + posspamfood.position.z);
                posy = Random.Range(0, 360);
                food1.transform.position = new Vector3(posx, posspamfood.position.y, posz);
                food1.transform.rotation = Quaternion.Euler(0, posy, 0);
                int i1 = food1.GetComponentsInChildren<Renderer>().Length;
                if (i1 > 1)
                {
                    foreach (Renderer i2 in food1.GetComponentsInChildren<Renderer>())
                    {
                        i2.enabled = false;
                    }
                }
                else
                    food1.GetComponentInChildren<Renderer>().enabled = false;
                food1.SetActive(true);
            }
            yield return new WaitForSeconds(spawnTimeBot);
            yield return new WaitUntil(() => TMT_ObjectPooling._inst.TMT_GetAllGameObjectPool() <= limitSpawnBotNumber);
        }
    }

    void SpawnEnemy()
    {
        foreach (var i in transformSpawnEnemy)
        {
            tempTransformSpawn.Add(i);
        }

        // for (int i = 0; i < 5; i++)
        for (int i = 0; i < enemys.Length; i++)
        {
            int iPos = RandomPos();
            GameObject e = TMT_ObjectPooling._inst.TMT_GetEnemy((GameObject)enemys[i], tempTransformSpawn[iPos]);
            GameObject gFollow = new GameObject(enemyFollows[i].name + "Parent");
            gFollow.transform.parent = tempTransformSpawn[iPos];
            gFollow.AddComponent<MeshRenderer>();
            gFollow.AddComponent<TMT_FollowEnemy>();
            gFollow.GetComponent<TMT_FollowEnemy>().target = e;
            gFollow.GetComponent<TMT_FollowEnemy>().idFollowEnemy = 1;
            gFollow.gameObject.layer = 14;
            float ii = 1;
            gFollow.transform.localScale = new Vector3(ii, ii, ii);
            GameObject coliderFollow = new GameObject(enemyFollows[i].name + "Collider");
            coliderFollow.transform.parent = gFollow.transform;
            coliderFollow.AddComponent<MeshRenderer>();
            coliderFollow.AddComponent<SphereCollider>();
            coliderFollow.layer = 18;
            coliderFollow.AddComponent<TMT_GetPosFoodForEnemy>();
            coliderFollow.GetComponent<SphereCollider>().radius = 5;
            coliderFollow.GetComponent<SphereCollider>().isTrigger = true;
            coliderFollow.GetComponent<TMT_GetPosFoodForEnemy>().enemy = e;
            e.GetComponent<Enemy>().botEnemyParent = gFollow;
            e.GetComponent<Enemy>().coliderFollow = coliderFollow;
            e.GetComponent<Enemy>().transformsmove = transformSpawnEnemy;
            e.GetComponent<Enemy>().posCurrent = iPos;
            e.GetComponent<Enemy>().enemyFollow = (GameObject)enemyFollows[i];
            e.GetComponent<Enemy>().enemyId = i;
            tempTransformSpawn.Remove(tempTransformSpawn[iPos]);
            enemyLists.Add(e);
            e.SetActive(true);
        }
    }

    int RandomPos()
    {
        return Random.Range(1, tempTransformSpawn.Count);
    }

    public void TMT_PlayGame()
    {
        if (inputName.text != "")
            PlayerPrefs.SetString("player_name", inputName.text);
        else
            PlayerPrefs.SetString("player_name", "Player" + Random.Range(0, 9999).ToString("0000"));
        SpawnEnemy();
        _setPlayerName = true;
        event2.Invoke();
    }

    public void TMT_Lobby()
    {
        event1.Invoke();
    }

    IEnumerator CallLobby()
    {
        if (PlayerPrefs.HasKey("player_name"))
        {
            inputName.text = PlayerPrefs.GetString("player_name");
        }
        yield return new WaitUntil(() => player.gameObject.GetComponent<PlayerController>()._anim != null);
        TMT_Lobby();
    }

    public void TMT_SetupLimitSpawnBotNumber(float num)
    {
        limitSpawnBotNumber = num;
    }

    public void TMT_SetupTimeSpawnFood(float num)
    {
        spawnTimeBot = num;
    }

    public void TMT_CallActiveBtnPlay()
    {
        StartCoroutine(ActiveBtnPlay());
    }

    IEnumerator ActiveBtnPlay()
    {
        yield return new WaitUntil(() => !player.gameObject.GetComponent<PlayerController>().enabled);
        btnPlay.interactable = true;
    }

    public void TMT_PauseGame()
    {
        Time.timeScale = 0;
    }

    public void TMT_ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void TMT_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TMT_RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void TMT_GameOver()
    {
        TMT_SoundGameOver();
        pnlScoreBoard.SetActive(true);
        TMT_PauseGame();
    }

    public void TMT_SoundGameOver()
    {
        GameObject gSound = TMT_ObjectPooling._inst.TMT_GetSound(soundPrefads, soundParent.transform);
        gSound.GetComponent<AudioSource>().clip = soundWin;
        gSound.transform.position = player.position;
        gSound.SetActive(true);
        gSound.GetComponent<AudioSource>().Play();
    }

    public void TMT_ChooseSkillToPlay(int skill)
    {
        PlayerPrefs.SetInt("skill", skill);
    }
}