using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_PlayerCtrlCam : MonoBehaviour
{
    Vector3 posCam = Vector3.zero, target;
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] GameObject camBox;
    [SerializeField] int hp, tempHp;
    [SerializeField] float smoothTime;
    // [SerializeField] TMT_BoxDetectAllMesh boxDetectAllMesh;

    private void Update()
    {
        hp = GetComponent<PlayerController>()._hp;

        if (hp - tempHp >= 30)
        {
            tempHp = hp;
            target = camBox.transform.localPosition - new Vector3(0, 0, 2);
            // boxDetectAllMesh.TMT_UpdateRadius();
        }
        camBox.transform.localPosition = Vector3.SmoothDamp(camBox.transform.localPosition, target, ref velocity, smoothTime);
    }
}
