using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<Trung> : MonoBehaviour where Trung : MonoBehaviour
{
    static Trung inst;
    public static Trung Instant
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<Trung>();
            }
            if (inst == null)
            {
                GameObject g = new GameObject("singleton");
                g.AddComponent<Trung>();
                inst = g.GetComponent<Trung>();
            }
            else
            {
                Trung[] tmts = FindObjectsOfType<Trung>();
                if (tmts.Length == 1)
                    inst = tmts[0];
                if (tmts.Length > 1)
                {
                    inst = tmts[0];
                    for (int i = 1; i <= tmts.Length - 1; i++)
                    {
                        Destroy(tmts[i].gameObject);
                    }
                }
            }
            return inst;
        }
    }
}
