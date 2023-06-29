using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.MLAgents;

public enum BattleStatePart3
{
    Start,
    PlayerTurn,
    AITurn,
    Won,
    Lost
}

public class Part3BattleState : Agent
{
    public GameObject[] playerPrefab;
    public GameObject[] enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    Unit PlayerUnit;
    Unit EnemyUnit;


    public Button Attack;
    public Button Heal;

    private int Starter;



    public BattleStatePart3 state;



     void Start()
    {
        state = BattleStatePart3.Start;
        StartCoroutine(SetUpBattle());
        Attack.enabled = false;
        Heal.enabled = false;
        
    }

    IEnumerator SetUpBattle()
    {

        //AI Random Element
        int randomEnemyElementIndex = Random.Range(0, enemyPrefab.Length);
        GameObject enemyUnitPrefab = enemyPrefab[randomEnemyElementIndex];

        GameObject enemyUnit = Instantiate(enemyUnitPrefab, enemyBattleStation);
        enemyUnit.transform.localScale = new Vector3(.1f, .1f, 1);

        EnemyUnit = enemyUnit.GetComponent<Unit>();
        EnemyUnit.element = (Element)randomEnemyElementIndex;


        //Player Random Element

        int randomPlayerElementIndex = Random.Range(0, playerPrefab.Length);
        GameObject playerUnitPrefab = playerPrefab[randomPlayerElementIndex];

        GameObject playerUnit = Instantiate(playerUnitPrefab, playerBattleStation);
        playerUnit.transform.localScale = new Vector3(.1f, .1f, 1);
        PlayerUnit = playerUnit.GetComponent<Unit>();
        PlayerUnit.element = (Element)randomPlayerElementIndex;

        //setup Text for start of game

        dialogueText.text = "P1:" + PlayerUnit.unitName + " VS CPU:" + EnemyUnit.unitName;


        playerHUD.SetUpHUD(PlayerUnit);
        enemyHUD.SetUpHUD(EnemyUnit);

        yield return new WaitForSeconds(2f);


        Starter = Random.Range(0, 2);

        if (Starter == 0)
        {
            state = BattleStatePart3.PlayerTurn;
            PlayerTurn();
        }
        else if (Starter == 1)
        {
            state = BattleStatePart3.AITurn;
            AITurn();
        }

    }



    public void AITurn()
    {

        dialogueText.text = "Your Opponent is Thinking";
        Attack.enabled = false;
        Heal.enabled = false;


        

        int aiDecision = MakeAIDecision();
        float reward = 0f;

        if (aiDecision == 1)
        {
            StartCoroutine(AIAttack());
            reward = CalculateAttackReward();
            

        }
        else if (aiDecision == 2)
        {
            StartCoroutine(AIHeal());
            reward = CalculateHealReward();
        }








    }

    IEnumerator AIAttack()
    {
        yield return new WaitForSeconds(5f);


        bool isDead = PlayerUnit.TakeDamage(EnemyUnit.damage, EnemyUnit.element);

        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "Your opponent has dealt Damage";

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleStatePart3.Won;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleStatePart3.PlayerTurn;
            PlayerTurn();
        }

    }


    IEnumerator AIHeal()
    {
        EnemyUnit.Heal(2);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        dialogueText.text = "Your Opponent Has Healed for: 2HP";


        yield return new WaitForSeconds(2f);
        state = BattleStatePart3.PlayerTurn;
        PlayerTurn();

    }

    public void PlayerTurn()
    {
        dialogueText.text = "Player 1 Choose an action";
        Attack.enabled = true;
        Heal.enabled = true;

    }

    public void OnAttackButton()
    {
        if (state != BattleStatePart3.PlayerTurn)

            return;
        StartCoroutine(PlayerAttack());

    }


    IEnumerator PlayerHeal()
    {

        PlayerUnit.Heal(2);
        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "Player 1 Has Healed for: 2HP";

        yield return new WaitForSeconds(2f);

        state = BattleStatePart3.AITurn;
        AITurn();

    }


    IEnumerator PlayerAttack()
    {
        // if (EnemyUnit.block == true)
        // {
        // dialogueText.text = "Player2 Has blocked the attack";

        //  }
        //  else if(EnemyUnit.block ==false)
        //  {
        bool isDead = EnemyUnit.TakeDamage(PlayerUnit.damage, PlayerUnit.element);


        enemyHUD.SetHP(EnemyUnit.currentHP);

        dialogueText.text = "You have dealt Damage";
        yield return new WaitForSeconds(2f);


        if (isDead)
        {
            state = BattleStatePart3.Won;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleStatePart3.AITurn;
            AITurn();


        }



    }

    IEnumerator AIWait()
    {
        yield return new WaitForSeconds(5f);
    }







    IEnumerator EndBattle()
    {
        if (state == BattleStatePart3.Won)
        {
            if (EnemyUnit.currentHP <= 0)
            {
                dialogueText.text = "You are Victorious";
            }
            else if (PlayerUnit.currentHP <= 0)
            {
                dialogueText.text = "Your Opponent is Victorious";
            }

            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("MainMenu");



        }
    }


    int MakeAIDecision()
    {
        int decision = Random.Range(1, 3);
        return decision;
    }

    float CalculateAttackReward()
    {
        // Implement the reward calculation logic for the attack action
        // Adjust the reward based on the game outcome (e.g., if the player is defeated or not)

        bool isDead = PlayerUnit.TakeDamage(EnemyUnit.damage, EnemyUnit.element);

        float reward = 0f;

        if (isDead)
        {
            reward = 10f; // Maximum reward when the player is defeated
        }
        else
        {
            // Calculate a partial reward based on the amount of damage dealt, game state, or any other relevant factors
            reward = EnemyUnit.damage;
        }

        return reward;
    }

    float CalculateHealReward()
    {
        // Implement the reward calculation logic for the heal action
        // Adjust the reward based on the game state, effectiveness of the heal, or any other relevant factors

        float reward = 2f;

        return reward;
    }





}