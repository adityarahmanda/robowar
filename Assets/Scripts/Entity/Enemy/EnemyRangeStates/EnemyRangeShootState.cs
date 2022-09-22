using System.Collections;
using UnityEngine;
using RobustFSM.Base;

public class EnemyRangeShootState : MonoState
{
    public EnemyRange Owner
    {
        get
        {
            return ((EnemyRangeFSM)SuperMachine).Owner;
        }
    }

    private float m_shootForce = 3f;

    public override void OnEnter()
    {
        base.OnEnter();

        StartShoot();
    }

    private void Update() 
    {        
        Owner.LookAtPlayer();

        if(LevelManager.i.LevelEnded || !Owner.IsPlayerInSight() || !Owner.IsPlayerInShootRange())
        {
            StopAllCoroutines();
            Owner.main.animator.SetBool("isShoot", false);
            EndShoot();
        }
    }

    public void StartShoot()
    {
        Owner.main.animator.SetFloat("shootSpeed", Owner.shootSpeed);
        Owner.main.animator.SetBool("isShoot", true);
        Owner.main.animator.SetTrigger("shoot");
    }

    public IEnumerator StartShootAfterCooldown()
    {
        yield return new WaitForSeconds(Owner.timeBetweenShoot);

        StartShoot();
    }

    public void Shoot()
    {
        Vector3 shootDirection = LevelManager.i.player.transform.position - transform.position;

        Bullet currentBullet = Instantiate(Owner.bulletPrefab, Owner.shootPoint.position, Quaternion.identity);
        currentBullet.Init(Owner.damage, Owner.shootRange, shootDirection.normalized, m_shootForce); 

        AudioManager.i.PlaySFX("laser");
    }

    public void EndShoot()
    {
        if(LevelManager.i.LevelEnded)
        {
            SuperMachine.ChangeState<EnemyVictoryState>();
            return;
        }

        if(!Owner.IsPlayerInSight())
        {
            SuperMachine.ChangeState<EnemyRangePatrolState>();
            return;
        }

        if(!Owner.IsPlayerInShootRange())
        {
            SuperMachine.ChangeState<EnemyRangeChaseState>();
            return;
        }

        StartCoroutine(StartShootAfterCooldown());
    }
}