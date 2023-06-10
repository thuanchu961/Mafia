using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Activewall : MonoBehaviour
{

    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("SpamwPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
        if (other.gameObject.layer == 15)
        {
            other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        if (other.gameObject.layer == 15)
        {
            other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    }
}
