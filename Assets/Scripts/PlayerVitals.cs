using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : PlayerStateMachine
{
    public float Health = 100f;
    float enemyDamage = 20f;
    public PlayerStateMachine playerState;

    private void Awake()
    {
        EnemyAttackState.playerIsAttackedCallBack += DecreaseHealth;
    }

    void DecreaseHealth()
    {
        Debug.Log("Player was attacked! Health: " + Health);
        if (Health - enemyDamage > 0)
        {
            Health -= enemyDamage;
            return;
        }

        Debug.Log("Player is dead!");
        playerState.SwitchState(new PlayerDeadState(playerState));
    }
}
