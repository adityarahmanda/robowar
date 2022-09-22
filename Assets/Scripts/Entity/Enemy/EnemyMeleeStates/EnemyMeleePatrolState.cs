using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using RobustFSM.Base;

public class EnemyMeleePatrolState : MonoState 
{
    public EnemyMelee Owner
    {
        get
        {
            return ((EnemyMeleeFSM)SuperMachine).Owner;
        }
    }

    private float m_waitTime = 1f;
    private bool m_isPatrolling;

    private Vector3 m_initPos;

    public override void OnEnter()
    {
        base.OnEnter();

        m_initPos = transform.position;
        StartPatrol();
    }

    private void Update() 
    {
        Owner.LookAt(Owner.main.agent.velocity);

        if(LevelManager.i.LevelEnded || Owner.IsPlayerInSight())
        {
            EndPatrol();
        } 
    }

    public void StartPatrol()
    {
        StartCoroutine(PatrolCoroutine());
    }

    private IEnumerator PatrolCoroutine()
    {
        m_isPatrolling = true;

        Vector3 randomRange = Random.insideUnitSphere * Owner.sightRange;
        Vector3 targetPosition = m_initPos + new Vector3(randomRange.x, 0, randomRange.z);
        
        NavMeshHit hit;
        if(NavMesh.SamplePosition(targetPosition, out hit, Owner.main.agent.height * 2, NavMesh.AllAreas))
        {
            Owner.main.agent.SetDestination(hit.position);
        }

        yield return new WaitUntil(() => !Owner.main.agent.pathPending && Owner.main.agent.remainingDistance < .1f);

        yield return new WaitForSeconds(m_waitTime);

        m_isPatrolling = false;

        EndPatrol();
    }

    public void EndPatrol()
    {
        if(m_isPatrolling)
        {
            StopAllCoroutines();
            Owner.main.agent.ResetPath();
        }

        if(LevelManager.i.LevelEnded)
        {
            SuperMachine.ChangeState<EnemyVictoryState>();
            return;
        }

        if(Owner.IsPlayerInSight())
        {
            SuperMachine.ChangeState<EnemyMeleeChaseState>();
            return;
        }

        StartPatrol();
    }
}