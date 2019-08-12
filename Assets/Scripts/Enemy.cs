﻿using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 1f;
    [SerializeField] float baseLazerSpeed = 10f;
    [SerializeField] GameObject lazerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(autoFire());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator autoFire()
    {
        while (true)
        {
            var shotPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var shot = Instantiate(lazerPrefab, shotPosition, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -baseLazerSpeed);

            yield return new WaitForSeconds(Random.Range(minTimeBetweenShots, maxTimeBetweenShots));
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DamageDealer damageDealer = collider.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer)
        {
            return;
        }

        handleHit(damageDealer);
    }

    private void handleHit(DamageDealer damageDealer)
    {
        health -= damageDealer.Damage;
        
        damageDealer.hit();

        if (health <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
