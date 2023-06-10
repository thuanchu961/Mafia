using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_FollowPlayer : TMT_Singleton<TMT_FollowPlayer>
{
    [SerializeField] GameObject player;
    [SerializeField] float smoothTime, tmpHp;
    Vector3 currentVelocity = Vector3.zero;
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref currentVelocity, smoothTime);
        transform.localPosition = new Vector3(transform.localPosition.x, -1, transform.localPosition.z);

        if (player.GetComponent<PlayerController>()._hp - tmpHp >= 15)
        {
            tmpHp = player.GetComponent<PlayerController>()._hp;
            smoothTime += 0.1f;
        }
    }
}
