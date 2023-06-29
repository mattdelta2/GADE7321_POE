using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using System.Threading;

public class CombatAgent : Agent
{
    Unit PlayerUnit;
    Unit EnemyUnit;

    Part3BattleState State;
    public void Reward(float reward)
    {
        float rewards = 0f;
    }

    public void Start()
    {
        
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        if (PlayerUnit != null)
        {
            sensor.AddObservation(PlayerUnit.currentHP);
        }
        else
        {
            Debug.Log("PlayerUnit comonent not found");
        }

        if(EnemyUnit != null)
        {
            sensor.AddObservation(EnemyUnit.currentHP);
        }
        else
        {
            Debug.Log("Enemy unit component not found");
        }

        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int actionIndex = actions.DiscreteActions[0];


        if(actionIndex == 0)
        {
            State.AITurn();
        }
        else if(actionIndex == 1)
        {
            State.AITurn();
        }
    }

}
