using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CombatAgent : Agent
{
    /*
    Unit PlayerUnit;
    Unit EnemyUnit;

    public override void Initialize()
    {
        var attackAction = new DiscreteActionBranch(2);
        var healAction = new DiscreteActionBranch(2);


        var combineBranches = new List<ActionSpec> {attackAction, healAction};

        ActionSpec = ActionSpec.MakeCombineActionSpec(combineBranches.ToArray());
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(PlayerUnit.currentHP);
        sensor.AddObservation(EnemyUnit.currentHP);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int attackAction = actions.DiscreteActions[0];
        int healAction = actions.DiscreteActions[1];


        if(attackAction ==1)
        {
            //perform attack sequence
        }

        if(healAction ==1)
        {
            //perform heal sequence
        }
    }*/


    private int playerHealth = 20;
    private int aiHealth = 20;
    private int attackDamageMin = 2;
    private int attackDamageMax = 4;

    public override void OnEpisodeBegin()
    {
        // Reset the player and AI health at the beginning of each episode
        playerHealth = 20;
        aiHealth = 20;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int attackAction = actions.DiscreteActions[0]; // Get the attack action

        // Process the attack action
        if (attackAction == 1)
        {
            // Calculate the damage dealt
            int damageDealt = UnityEngine.Random.Range(attackDamageMin, attackDamageMax + 1);

            // Update player and AI health
            playerHealth -= damageDealt;

            // Assign rewards based on the outcome
            if (playerHealth <= 0)
            {
                // AI wins (terminal state)
                SetReward(100f);
                EndEpisode();
            }
            else
            {
                // AI successfully deals damage
                SetReward(damageDealt);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for terminal state if needed
        if (playerHealth <= 0)
        {
            // Player wins (terminal state)
            SetReward(-100f);
            EndEpisode();
        }
    }
}
