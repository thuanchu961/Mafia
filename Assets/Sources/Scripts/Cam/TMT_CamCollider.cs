using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_CamCollider : MonoBehaviour
{
    [SerializeField] List<Material> oldChildMaterial = new List<Material>();
    [SerializeField] List<Renderer> childRenderer = new List<Renderer>();
    [SerializeField] GameObject cam;
    [SerializeField] Material transparentMaterial;
    Vector3 tempPos;

    private void OnTriggerEnter(Collider other)
    {
        childRenderer.Add(other.GetComponent<Renderer>());
        oldChildMaterial.Add(other.GetComponent<Renderer>().material);

        other.GetComponent<Renderer>().material = transparentMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < childRenderer.Count; i++)
        {
            if (childRenderer[i] == other.GetComponent<Renderer>())
            {
                childRenderer[i].material = oldChildMaterial[i];
                childRenderer.RemoveAt(i);
                oldChildMaterial.RemoveAt(i);
            }
        }
    }
}
