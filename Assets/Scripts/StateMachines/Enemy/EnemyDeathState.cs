using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    //public GameObject ragdollObject;
    
    public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() 
    {
        stateMachine.RagdollLogic.SpawnRagdoll(stateMachine.transform);

        var enemy = stateMachine.transform.gameObject;
        UnityEngine.Object.Destroy(enemy);
    }

    public override void Tick() { }

    public override void Exit() { }
}
