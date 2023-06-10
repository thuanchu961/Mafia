using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_ObjectPooling : TMT_Singleton<TMT_ObjectPooling>
{
    public List<GameObject> BotFoodPools = new List<GameObject>();
    public List<GameObject> BotPlayerPools = new List<GameObject>();
    public List<GameObject> EnemyPools = new List<GameObject>();
    public List<GameObject> BotEnemyPools = new List<GameObject>();
    public List<GameObject> EffectHitPools = new List<GameObject>();
    public List<GameObject> EffectHitFoodPools = new List<GameObject>();
    public List<GameObject> EffectLightningBoltPools = new List<GameObject>();
    public List<GameObject> SoundPools = new List<GameObject>();

    public GameObject TMT_GetBotFood(GameObject obj, Transform parent = null)
    {
        foreach (var i in BotFoodPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        BotFoodPools.Add(g);
        return g;
    }

    public GameObject TMT_GetBotPlayer(GameObject obj, Transform parent = null)
    {
        foreach (var i in BotPlayerPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        BotPlayerPools.Add(g);
        return g;
    }

    public GameObject TMT_GetEnemy(GameObject obj, Transform parent = null)
    {
        foreach (var i in EnemyPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        EnemyPools.Add(g);
        return g;
    }

    public GameObject TMT_GetBotEnemy(GameObject obj, Transform parent = null)
    {
        foreach (var i in BotEnemyPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        BotEnemyPools.Add(g);
        return g;
    }

    public GameObject TMT_GetEffectHit(GameObject obj, Transform parent = null)
    {
        foreach (var i in EffectHitPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        EffectHitPools.Add(g);
        return g;
    }

    public GameObject TMT_GetEffectHitFood(GameObject obj, Transform parent = null)
    {
        foreach (var i in EffectHitFoodPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        EffectHitFoodPools.Add(g);
        return g;
    }

    public GameObject TMT_GetSound(GameObject obj, Transform parent = null)
    {
        foreach (var i in SoundPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        SoundPools.Add(g);
        return g;
    }

    public GameObject TMT_GetEffectLightningBolt(GameObject obj, Transform parent = null)
    {
        foreach (var i in EffectLightningBoltPools)
        {
            if (i.activeSelf)
                continue;
            return i;
        }

        GameObject g = Instantiate(obj, parent);
        EffectLightningBoltPools.Add(g);
        return g;
    }

    public int TMT_GetAllGameObjectPool()
    {
        int count = 0;
        count = TMT_SearchActiveObj(BotFoodPools) + TMT_SearchActiveObj(BotPlayerPools) + TMT_SearchActiveObj(EnemyPools) + TMT_SearchActiveObj(BotEnemyPools);
        return count;
    }

    public int TMT_SearchActiveObj(List<GameObject> goLists)
    {
        int count = goLists.Count;

        foreach (var i in goLists)
        {
            if (!i.activeSelf)
                count--;
        }

        return count;
    }
}
