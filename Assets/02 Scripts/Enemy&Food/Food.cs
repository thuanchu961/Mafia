using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyfood());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 || other.gameObject.layer == 11)
            return;

        Vector3 direction = other.transform.position - this.transform.position;
        float angle1 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, angle1, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 21)
        {
            Vector3 tmpPos = transform.position;
            tmpPos.y = oldPos.y;
            transform.position = tmpPos;
        }

        if (other.gameObject.layer == 20)
        {
            oldPos = transform.position;
            int i1 = GetComponentsInChildren<Renderer>().Length;
            if (i1 > 1)
            {
                foreach (Renderer i2 in GetComponentsInChildren<Renderer>())
                {
                    i2.enabled = true;
                }
            }
            else
                GetComponentInChildren<Renderer>().enabled = true;
        }

        if (other.gameObject.layer == 6 || other.gameObject.layer == 11)
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.layer == 7 || other.gameObject.layer == 4 || other.gameObject.layer == 22)
            gameObject.SetActive(false);
    }

    IEnumerator destroyfood()
    {
        yield return new WaitForSeconds(120);
        gameObject.SetActive(false);
    }
}
