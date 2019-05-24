using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ZombieHealth))]
public class Zombie : MonoBehaviour
{
    [SerializeField] Character npcSettings;
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float damage;

    [HideInInspector] public NavMeshAgent agent;

    Player target;
    bool targetInRange;
    bool canAttack;
    CapsuleCollider capsuleCollider;

    public event System.Action OnAttack;

    private ZombieHealth m_ZombieHealth;
    public ZombieHealth ZombieHealth
    {
        get
        {
            if (m_ZombieHealth == null)
                m_ZombieHealth = GetComponent<ZombieHealth>();
            return m_ZombieHealth;
        }
    }

    void Awake()
    {
        canAttack = true;
        targetInRange = false;
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        ZombieHealth.OnDeath += OnDeath;

        LookForTarget();
    }

    void Update()
    {
        if(gameObject == null)
            return;

        if (target == null || !ZombieHealth.isAlive)
			return;

        if(!target.GetComponent<PlayerHealth>().isAlive)
        {
            LookForTarget();
            return;
        }

        if(canAttack && targetInRange)
            Attack();

		transform.LookAt (target.transform.position);
        agent.SetDestination (target.gameObject.transform.position);
    }

    private void OnDeath()
    {
        capsuleCollider.enabled = false;
        agent.isStopped = true;
    }

     void OnTriggerEnter (Collider other)
     {
        // If the entering collider is the target...
        if(other.gameObject == target.gameObject)
        {
            // ... the target is in range.
            targetInRange = true;
        }
     }


    void OnTriggerExit (Collider other)
    {
        // If the exiting collider is the target...
        if(other.gameObject == target.gameObject)
        {
            // ... the target is no longer in range.
            targetInRange = false;
        }
    }

    void LookForTarget()
    {
        if(this == null)
            return;

        target = FindClosestTarget();
        GameManager.Instance.Timer.Add(LookForTarget, 10f);
    }

    void Attack()
    {
        canAttack = false;
        target.PlayerHealth.TakeDamage(damage);

        if(OnAttack != null)
            OnAttack();

        GameManager.Instance.Timer.Add(() => 
        {
            canAttack = true;
        }, timeBetweenAttacks);
    }

    Player FindClosestTarget()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest.GetComponent<Player>();;
    }
}
