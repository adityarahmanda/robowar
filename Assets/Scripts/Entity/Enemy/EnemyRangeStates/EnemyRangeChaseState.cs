using RobustFSM.Base;
using UnityEngine;

public class EnemyRangeChaseState : MonoState
{
    public EnemyRange Owner
    {
        get
        {
            return ((EnemyRangeFSM)SuperMachine).Owner;
        }
    }

    private void Update() 
    {
        if(LevelManager.i.LevelEnded || !Owner.IsPlayerInSight() || Owner.IsPlayerInShootRange())
        {
            Owner.main.agent.ResetPath();

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

            if(Owner.IsPlayerInShootRange())
            {
                SuperMachine.ChangeState<EnemyRangeShootState>();
                return;
            }
        }

        Owner.main.agent.SetDestination(LevelManager.i.player.transform.position);
        Owner.LookAt(Owner.main.agent.velocity);
    }
}
