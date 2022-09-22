using UnityEngine;
using RobustFSM.Base;

public class EnemyMeleeAttackState : MonoState 
{
    public EnemyMelee Owner
    {
        get
        {
            return ((EnemyMeleeFSM)SuperMachine).Owner;
        }
    }

    private Vector3 m_attackDirection;
    private float m_strafeProbability = 3f/4f;
    
    public override void OnEnter()
    {
        base.OnEnter();

        StartAttack();
    }

    private void Update() 
    {
        if(LevelManager.i.LevelEnded)
        {
            SuperMachine.ChangeState<EnemyVictoryState>();
            return;
        }

        Owner.LookAt(m_attackDirection);
    }

    public void StartAttack()
    {
        m_attackDirection = LevelManager.i.player.transform.position - transform.position;

        Owner.main.animator.SetBool("isAttack", true);
        Owner.main.animator.SetFloat("attackSpeed", Owner.attackSpeed);
        Owner.main.animator.SetTrigger("attack");

        AudioManager.i.PlaySFX("light-saber");
    }

    public void EndAttack()
    {
        Owner.main.animator.SetBool("isAttack", false);

        if(!Owner.IsPlayerInSight())
        {
            SuperMachine.ChangeState<EnemyMeleePatrolState>();
            return;
        }

        if(!Owner.IsPlayerInAttackRange())
        {
            SuperMachine.ChangeState<EnemyMeleeChaseState>();
            return;
        }

        if(Random.Range(0, 1f) <= m_strafeProbability)
        {
            SuperMachine.ChangeState<EnemyMeleeStrafeState>();
            return;
        }
            
        StartAttack();
    }
}