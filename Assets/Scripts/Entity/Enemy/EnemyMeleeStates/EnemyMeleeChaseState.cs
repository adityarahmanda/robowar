using UnityEngine;
using RobustFSM.Base;

public class EnemyMeleeChaseState : MonoState 
{
    public EnemyMelee Owner
    {
        get
        {
            return ((EnemyMeleeFSM)SuperMachine).Owner;
        }
    }

    private void Update() 
    {
        if(LevelManager.i.LevelEnded || !Owner.IsPlayerInSight() || Owner.IsPlayerInAttackRange())
        {
            Owner.main.agent.ResetPath();

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

            if(Owner.IsPlayerInAttackRange())
            {
                SuperMachine.ChangeState<EnemyMeleeAttackState>();
                return;
            }
        }

        Owner.main.agent.SetDestination(LevelManager.i.player.transform.position);
        Owner.LookAt(Owner.main.agent.velocity);
    }
}