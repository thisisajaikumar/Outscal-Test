using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Damage = 10;

    [SerializeField, Range(0, 5)] float moveSpeed = 2.0f;
    [SerializeField] Vector2 minPosition, maxPosition; 

    private Vector2 currentTarget;

    private void Start()
    {
        currentTarget = maxPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == currentTarget)
        {
            if (currentTarget == maxPosition)
                currentTarget = minPosition;
            else
                currentTarget = maxPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.HitEnemyEvent?.Invoke(Damage, this.gameObject);
        }
    }
}
