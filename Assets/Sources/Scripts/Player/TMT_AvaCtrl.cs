using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_AvaCtrl : MonoBehaviour
{
    [SerializeField] int avaId = -1;
    public int _avaId { get => avaId; set => avaId = value; }
    Object[] avaArr;
    [SerializeField] List<GameObject> avaList = new List<GameObject>();
    public List<GameObject> _avaList { get => avaList; set => avaList = value; }

    private void Start()
    {
        if (avaId == -1)
        {
            avaArr = Resources.LoadAll("PrefabsPlayer/Ava", typeof(GameObject));

            foreach (var i in avaArr)
            {
                avaList.Add((GameObject)i);
            }

            if (avaList.Count > 1)
            {
                for (int i = 0; i < avaList.Count; i++)
                {
                    avaList[i].GetComponent<TMT_AvaId>().avaId = i;
                }

                StartCoroutine(GetAvaID());
            }
        }
        else
        {
            TMT_AvaId temp = GetComponentInChildren<TMT_AvaId>();
            if (temp == null)
                ShowAva();
        }
    }

    IEnumerator GetAvaID()
    {
        yield return new WaitUntil(() => avaId > -1);
        ShowAva();
    }

    void ShowAva()
    {
        Instantiate(avaList[avaId], transform);
    }
}
