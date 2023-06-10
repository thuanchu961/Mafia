using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_MapCtrl : MonoBehaviour
{
    [SerializeField] Transform[] buildingArr;
    [SerializeField] List<GameObject> buildingList = new List<GameObject>();
    [SerializeField] GameObject mapAn, terrain, building;

    private void Start()
    {
        buildingArr = building.GetComponentsInChildren<Transform>();
        foreach (var i in buildingArr)
        {
            buildingList.Add(i.gameObject);
        }
    }

    private void Update()
    {
        if (mapAn != null)
        {
            OneObject(mapAn);
        }

        if (terrain != null)
        {
            OneObject(terrain);
        }

        if (building != null)
        {
            if (buildingList.Count <= 0)
                return;

            foreach (var i in buildingList)
            {
                OneObject(i);
            }
        }
    }

    void OneObject(GameObject obj)
    {
        if (!obj.activeSelf)
            obj.SetActive(true);
    }
}
