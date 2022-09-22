using UnityEngine;
using RobustFSM.Base;

[RequireComponent(typeof(EnemyRangePatrolState))]
[RequireComponent(typeof(EnemyRangeChaseState))]
[RequireComponent(typeof(EnemyRangeShootState))]
[RequireComponent(typeof(EnemyVictoryState))]
public class EnemyRangeFSM : MonoFSM<EnemyRange> 
{
    public override void AddStates()
    {
        AddState<EnemyRangePatrolState>();
        AddState<EnemyRangeChaseState>();
        AddState<EnemyRangeShootState>();
        AddState<EnemyVictoryState>();

        SetInitialState<EnemyRangePatrolState>();
    }
}