using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMT_SkillCtrl : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMT_BoxDetectAllMesh boxDetectAllMesh;
    [SerializeField] GameObject botPlayerParent;
    [SerializeField] float oldSpeed, timeSkill1Deactive, timeSkill2Deactive, timeSkill3Deactive, timeSkill4Deactive;
    [SerializeField] int oldLayerPlayer, oldLayerBotPlayer;
    [SerializeField] TMT_GroupSkill[] groupSkills;
    [SerializeField] Image imgTimeSkill1Active, imgDeactiveSkill1;
    [SerializeField] Image imgTimeSkill2Active, imgDeactiveSkill2;
    [SerializeField] Image imgTimeSkill3Active, imgDeactiveSkill3;
    [SerializeField] Image imgTimeSkill4Active, imgDeactiveSkill4;
    [SerializeField] Text txtSKill1Time1, txtSkill1Time2;
    [SerializeField] Text txtSkill2Time1, txtSkill2Time2;
    [SerializeField] Text txtSkill3Time1, txtSkill3Time2;
    [SerializeField] Text txtSkill4Time1, txtSkill4Time2;
    [SerializeField] Coroutine c1;

    private void Start()
    {
        groupSkills = GetComponentsInChildren<TMT_GroupSkill>();
        foreach (var i in groupSkills)
        {
            i.gameObject.SetActive(false);
        }

        if (PlayerPrefs.HasKey("skill"))
        {
            int i = PlayerPrefs.GetInt("skill");
            groupSkills[i].gameObject.SetActive(true);
        }
        else
        {
            groupSkills[0].gameObject.SetActive(true);
        }
    }

    public void TMT_SkillUpSpeed()
    {
        oldSpeed = playerController._speed;
        playerController.TMT_SetSpeedUp(oldSpeed * 2);

        StartCoroutine(CallTimeSkillActive(imgTimeSkill1Active, imgDeactiveSkill1, 11, timeSkill1Deactive, txtSKill1Time1, 15, 1));
    }

    public void TMT_SkillBuffShield()
    {
        // oldLayerPlayer = playerController.gameObject.layer;
        // oldLayerBotPlayer = botPlayerParent.layer;

        // foreach (var i in TMT_ObjectPooling._inst.BotPlayerPools)
        // {
        //     if (i.activeSelf)
        //     {
        //         i.layer = 23;
        //     }
        // }
        // playerController.gameObject.layer = 23;
        boxDetectAllMesh.activeShield = true;
        StartCoroutine(CallTimeSkillActive(imgTimeSkill2Active, imgDeactiveSkill2, 11, timeSkill2Deactive, txtSkill2Time1, 15, 2));
    }

    public void TMT_SkillJumpAttack()
    {
        playerController.TMT_ActiveSkill3();
        StartCoroutine(CallTimeSkillActive(imgTimeSkill3Active, imgDeactiveSkill3, 1, timeSkill3Deactive, txtSkill3Time1, 15, 3));
    }

    public void TMT_SkillUltiThrowFire()
    {
        if (!playerController._enemyLock)
            return;

        playerController.TMT_ActiveSkill4();
        imgTimeSkill4Active.gameObject.SetActive(true);
        StartCoroutine(CallTimeSkillActive(imgTimeSkill4Active, imgDeactiveSkill4, 11, timeSkill4Deactive, txtSkill4Time1, 15, 4));
    }

    IEnumerator CallTimeSkillActive(Image imgTimeSkillActive, Image imgDeactiveSkill, float time, float timeDeactive, Text txtSKillTime1, float timeCountDown, int skill)
    {
        yield return new WaitUntil(() => imgTimeSkillActive.gameObject.activeSelf);
        timeDeactive = time;
        while (timeDeactive > 1)
        {
            timeDeactive -= 1;
            txtSKillTime1.text = timeDeactive.ToString();
            yield return new WaitForSeconds(1);
        }

        if (skill == 1)
            playerController.TMT_SetSpeedUp(oldSpeed);
        else if (skill == 2)
        {
            // playerController.gameObject.layer = oldLayerPlayer;
            // foreach (var i in TMT_ObjectPooling._inst.BotPlayerPools)
            // {
            //     if (i.activeSelf)
            //     {
            //         i.layer = oldLayerBotPlayer;
            //     }
            // }
            boxDetectAllMesh.activeShield = false;
        }

        imgTimeSkillActive.gameObject.SetActive(false);
        imgDeactiveSkill.gameObject.SetActive(true);
        StartCoroutine(DeactiveTimeSkill(imgDeactiveSkill, timeCountDown));
        yield return null;
    }

    IEnumerator DeactiveTimeSkill(Image imgDeactiveSkill, float time)
    {
        yield return new WaitUntil(() => imgDeactiveSkill.gameObject.activeSelf);
        while (imgDeactiveSkill.fillAmount > 0)
        {
            imgDeactiveSkill.fillAmount -= (1f / time * 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitUntil(() => imgDeactiveSkill.fillAmount <= 0);
        imgDeactiveSkill.gameObject.SetActive(false);
        imgDeactiveSkill.fillAmount = 1;
        yield return null;
    }
}