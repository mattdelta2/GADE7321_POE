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
    EnvironmentParameters GameState;
    BattleStatePart3 Game;
    public void Reward(float reward)
    {
        float rewards = 0f;
    }

    public void Start()
    {
        PlayerUnit = GetComponent<Unit>();
        EnemyUnit = GetComponent<Unit>();
        
    }

    public override void Initialize()
    {

        Game = BattleStatePart3.Start;
        GameState = Academy.Instance.EnvironmentParameters;
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
            SetReward(1f);
            EndEpisode();
        }
        else if(actionIndex == 1)
        {
            State.AITurn();
            SetReward(.1f);
        }
    }

    public void StartGame()
    {
        Game = BattleStatePart3.Start;
    }

    public void SetResetParam()
    {
        StartGame();
    }



}
