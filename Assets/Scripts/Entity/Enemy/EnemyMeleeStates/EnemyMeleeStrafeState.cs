using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using RobustFSM.Base;

public class EnemyMeleeStrafeState : MonoState 
{
    public EnemyMelee Owner
    {
        get
        {
            return ((EnemyMeleeFSM)SuperMachine).Owner;
        }
    }

    private float m_attackProbability = 1f/4f;
    private bool m_isStrafingRight, m_isStrafingLeft;

    public override void OnEnter()
    {
        base.OnEnter();

        StartStrafe();
    }

    private void Update() 
    {
        Owner.LookAtPlayer();

        if(LevelManager.i.LevelEnded || !Owner.IsPlayerInSight() || !Owner.IsPlayerInAttackRange())
        {
            EndStrafe();
        }
    }

    public void StartStrafe()
    {
        if(Random.Range(0, 1f) > .5f)
        {
            StartCoroutine(StrafeToRight());
        }
        else
        {
            StartCoroutine(StrafeToLeft());
        }
    }

    private IEnumerator StrafeToLeft()
    {
        m_isStrafingLeft = true;

        Vector3 strafeDirection = Vector3.Cross(transform.position - LevelManager.i.player.transform.position, Vector3.up);
        Vector3 strafePosition = transform.position + strafeDirection.normalized * Owner.strafeDistance;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(strafePosition, out hit, Owner.main.agent.height * 2, NavMesh.AllAreas))
        {
            Owner.main.agent.SetDestination(hit.position);
        }

        Owner.main.animator.SetBool("isStrafeToLeft", true);
        Owner.main.animator.SetBool("isStrafe", true);
        
        yield return new WaitUntil(() => !Owner.main.agent.pathPending && Owner.main.agent.remainingDistance <= 0.15f);
        
        m_isStrafingLeft = false;

        EndStrafe();
    }

    private IEnumerator StrafeToRight()
    {
        m_isStrafingRight = true;

        Vector3 strafeDirection = Vector3.Cross(LevelManager.i.player.transform.position - transform.position, Vector3.up);
        Vector3 strafePosition = transform.position + strafeDirection.normalized * Owner.strafeDistance;
        
        NavMeshHit hit;
        if(NavMesh.SamplePosition(strafePosition, out hit, Owner.main.agent.height * 2, NavMesh.AllAreas))
        {
            Owner.main.agent.SetDestination(hit.position);
        }

        Owner.main.animator.SetBool("isStrafeToLeft", false);
        Owner.main.animator.SetBool("isStrafe", true);
        
        yield return new WaitUntil(() => !Owner.main.agent.pathPending && Owner.main.agent.remainingDistance <= 0.15f);
        
        m_isStrafingRight = false;

        EndStrafe();
    }

    public void EndStrafe()
    {
        if(m_isStrafingLeft || m_isStrafingRight)
        {
            StopAllCoroutines();
        }

        if(LevelManager.i.LevelEnded || !Owner.IsPlayerInSight() || !Owner.IsPlayerInAttackRange())
        {
            Owner.main.animator.SetBool("isStrafe", false);
            
            if(LevelManager.i.LevelEnded)
            {
                SuperMachine.ChangeState<EnemyVictoryState>();
                return;
            }

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
        }

        if(Random.Range(0, 1f) <= m_attackProbability)
        {
            Owner.main.animator.SetBool("isStrafe", false);
            SuperMachine.ChangeState<EnemyMeleeAttackState>();
            return;
        }
        
        StartStrafe();
    }
}