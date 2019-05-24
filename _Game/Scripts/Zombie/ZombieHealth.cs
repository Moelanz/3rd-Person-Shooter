using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : Destructable
{
    [HideInInspector] public bool isCrouched = false;

    public override void Die()
    {
        base.Die();
        GameManager.Instance.EventBus.RaiseEvent("ZombieDeath");

        Destroy(this.gameObject, 10f);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if(isCrouched)
            return;

        if(hitPointsRemaining > 5)
            return;
    }
}
