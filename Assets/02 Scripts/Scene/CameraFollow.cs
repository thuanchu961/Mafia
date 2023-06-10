using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private float posTempX;
    private float posTempY;
    private float posTempZ;
    [SerializeField] Vector3 camBoxPositionBegin, camBoxRotationBegin;
    [SerializeField] GameObject camBox, tmt_followPlayer;

    // Use this for initialization
    void Start()
    {
        posTempX = transform.position.x - target.position.x;
        posTempY = transform.position.y - target.position.y;
        posTempZ = transform.position.z - target.position.z;
    }


    public void TMT_CallSetCamBox(bool b)
    {
        if (!b)
            StartCoroutine(TMT_SetCamBox());
        else
            TMT_SetCamBoxToPlay();
    }
    IEnumerator TMT_SetCamBox()
    {
        yield return new WaitUntil(() => !tmt_followPlayer.activeSelf);
        camBox.transform.localPosition = camBoxPositionBegin;
        camBox.transform.localRotation = Quaternion.Euler(camBoxRotationBegin);
    }

    void TMT_SetCamBoxToPlay()
    {
        camBox.transform.localPosition = Vector3.zero; ;
        camBox.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = new Vector3(target.position.x + posTempX, target.position.y + posTempY, target.position.z + posTempZ);

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
    }
}
