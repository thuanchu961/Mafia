using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_FollowEnemy : MonoBehaviour
{
    public GameObject target, enemy;
    public float smoothTime = 0.5f;
    public int idFollowEnemy;
    Vector3 currentVelocity = Vector3.zero;

    private void Update()
    {
        if (idFollowEnemy == 0)
            transform.LookAt(enemy.transform);
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref currentVelocity, smoothTime);
    }
}
