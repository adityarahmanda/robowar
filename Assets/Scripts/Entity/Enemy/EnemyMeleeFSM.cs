using UnityEngine;
using RobustFSM.Base;

[RequireComponent(typeof(EnemyMeleePatrolState))]
[RequireComponent(typeof(EnemyMeleeChaseState))]
[RequireComponent(typeof(EnemyMeleeAttackState))]
[RequireComponent(typeof(EnemyMeleeStrafeState))]
[RequireComponent(typeof(EnemyVictoryState))]
public class EnemyMeleeFSM : MonoFSM<EnemyMelee> 
{
    public override void AddStates()
    {
        AddState<EnemyMeleePatrolState>();
        AddState<EnemyMeleeChaseState>();
        AddState<EnemyMeleeStrafeState>();
        AddState<EnemyMeleeAttackState>();
        AddState<EnemyVictoryState>();

        SetInitialState<EnemyMeleeChaseState>();
    }
}