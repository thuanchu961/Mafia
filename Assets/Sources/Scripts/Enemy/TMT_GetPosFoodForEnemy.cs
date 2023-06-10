using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_GetPosFoodForEnemy : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField] List<GameObject> foodPosNear = new List<GameObject>();
    [SerializeField] GameObject nearFood = null;
    [SerializeField] float detectRange = 5.2f;
    public GameObject _nearFood => nearFood;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            foodPosNear.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            foodPosNear.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (foodPosNear.Count <= 0)
        {
            nearFood = null;
            return;
        }

        if (nearFood != null)
        {
            if (Vector3.Distance(transform.position, nearFood.transform.position) > detectRange)
            {
                foodPosNear.Remove(nearFood);
            }

            if (!nearFood.activeSelf)
            {
                foodPosNear.Remove(nearFood);
                nearFood = null;
                enemy.GetComponent<Enemy>().TMT_SetAgentTarget();
            }
        }
        else
        {
            nearFood = CheckNearestFood();
        }
    }

    GameObject CheckNearestFood()
    {
        GameObject temp = foodPosNear[0];
        if (foodPosNear.Count > 1)
        {
            for (int i = 0; i < foodPosNear.Count;)
            {
                return temp;
            }
        }
        return temp;
    }

    public void TMT_SetNearFood()
    {
        nearFood = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.8f, 0.8f, 0.2f);
        Gizmos.DrawSphere(transform.position, detectRange);
    }
}
