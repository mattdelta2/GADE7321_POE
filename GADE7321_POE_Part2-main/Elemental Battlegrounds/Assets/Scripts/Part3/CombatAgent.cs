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
   

    Part3BattleState State;

    Unit PlayerUnit;
    Unit EnemyUnit;
    EnvironmentParameters GameState;
    BattleStatePart3 Game;
    public bool useVecObs;
    public void Reward(float reward)
    {
        float rewards = 0f;
    }

    public void Start()
    {
        PlayerUnit = GetComponent<Unit>();
        EnemyUnit = GetComponent<Unit>();

        State.GetComponent<Part3BattleState>();
        
    }

    public override void Initialize()
    {
        PlayerUnit = FindObjectOfType<Unit>();
        EnemyUnit = FindObjectOfType<Unit>();
        State = GetComponent<Part3BattleState>();

        Game = BattleStatePart3.Start;
        GameState = Academy.Instance.EnvironmentParameters;
        SetResetParam();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        PlayerUnit = GetComponent<Unit>();
        EnemyUnit = GetComponent<Unit>();
        if (PlayerUnit != null)
        {
            sensor.AddObservation(PlayerUnit.currentHP);
        }


        if(EnemyUnit != null)
        {
            sensor.AddObservation(EnemyUnit.currentHP);
        }


        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        State = GetComponent<Part3BattleState>();
        int actionIndex = actions.DiscreteActions[0];

        if (actionIndex == 0)
        {
            State.AITurn();
            SetReward(1f);
            EndEpisode();
        }
        else if (actionIndex == 1)
        {
            State.AITurn();
        }

        if (EnemyUnit != null && EnemyUnit.currentHP == 0)
        {
            SetReward(-1f);
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
