using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_Billboard : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Update()
    {
        if (cam == null)
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }

}
