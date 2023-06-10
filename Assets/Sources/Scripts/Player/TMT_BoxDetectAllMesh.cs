using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_BoxDetectAllMesh : MonoBehaviour
{
    [SerializeField] Transform player;
    public bool activeShield = false;
    // [SerializeField] Renderer[] renderers;
    // [SerializeField] SphereCollider colliderDetectMesh;

    // private void Start()
    // {
    //     renderers = FindObjectsOfType<Renderer>();
    //     colliderDetectMesh = GetComponent<SphereCollider>();
    //     foreach (Renderer i in renderers)
    //     {
    //         if (i.gameObject.layer != 9)
    //             i.enabled = false;
    //     }
    // }

    private void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //     DectectMeshCollider(other, true);
        DectectEnemy(other);
    }

    private void OnTriggerStay(Collider other)
    {
        //     DectectMeshCollider(other);
        DectectEnemy(other);
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     DectectMeshCollider(other, false);
    // }

    void DectectEnemy(Collider other)
    {
        if (activeShield)
        {
            if (other.gameObject.layer == 14)
            {
                other.GetComponent<Enemy>()._playerActiveShield = true;
            }
            if (other.gameObject.layer == 16)
            {
                other.GetComponent<TMT_BotFollowEnemyCtrl>()._playerActiveShield = true;
            }
        }
        else
        {
            if (other.gameObject.layer == 14)
            {
                other.GetComponent<Enemy>()._playerActiveShield = false;
            }
            if (other.gameObject.layer == 16)
            {
                other.GetComponent<TMT_BotFollowEnemyCtrl>()._playerActiveShield = false;
            }
        }

    }

    // void DectectMeshCollider(Collider other, bool b)
    // {
    //     if (other.gameObject.layer == 9)
    //         return;

    //     Renderer[] renderers = other.gameObject.GetComponentsInChildren<Renderer>();
    //     int len = renderers.Length;

    //     if (len == 0)
    //     {
    //         other.gameObject.GetComponent<Renderer>().enabled = b;
    //     }
    //     else if (len > 1)
    //     {
    //         foreach (Renderer i in renderers)
    //         {
    //             i.enabled = b;
    //         }
    //     }
    //     else
    //         other.gameObject.GetComponentInChildren<Renderer>().enabled = b;
    // }

    // public void TMT_UpdateRadius()
    // {
    //     // if(colliderDetectMesh == null)
    //     //     return;

    //     float temp = colliderDetectMesh.radius;
    //     temp += 5;
    //     colliderDetectMesh.radius = temp;
    // }
}
