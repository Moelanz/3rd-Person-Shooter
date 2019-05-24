using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpikeTrap : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float lifeTime;
    [SerializeField] int uses;
    [SerializeField] bool canDamageOwner;

    [HideInInspector] public GameObject owner;
    int used;
    float lifeTimer;

    void Update()
    {
        if(lifeTime == 0) //0 means unlimited
            return;

        lifeTimer += Time.deltaTime;
        if(lifeTimer >= lifeTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(owner != null)
            if(other.gameObject == owner && !canDamageOwner)
                return;

        Debug.Log(other.transform.root.tag);

        Destructable health = other.transform.root.GetComponent<Destructable>();

        if(health == null)
            return;

        health.TakeDamage(damage);
        used++;

        if(uses == 0) //0 means unlimted
            return;

        if(used >= uses)
            Destroy(gameObject);
    }
}
