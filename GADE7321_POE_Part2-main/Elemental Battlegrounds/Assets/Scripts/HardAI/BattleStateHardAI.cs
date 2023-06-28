using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public enum battleStateAIHard
{
    Start,
    Player1Turn,
    AITurn,
    Won,
    Lost
}

public class BattleStateHardAI : MonoBehaviour
{

    public GameObject[] PlayerPrefab;

    public GameObject[] EnemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;


    public Text dialogueText;


    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    Dropdown elementDropdownPlayer1;
    Dropdown elementDropdownPlayer2;



    Unit PlayerUnit;
    Unit EnemyUnit;

    private int aiOptions;
    private int Starter;

    public Button Attack;
    public Button Heal;








    public battleStateAIHard state;
    // Start is called before the first frame update
    void Start()
    {

        state = battleStateAIHard.Start;
        Attack.enabled = false;
        Heal.enabled = false;
        StartCoroutine(SetUpBattle());
    }



    IEnumerator SetUpBattle()
    {

        //Random Element at the start of the game for Player2

        int randomEnemyElementIndex = Random.Range(0, EnemyPrefab.Length);
        GameObject enemyUnitPrefab = EnemyPrefab[randomEnemyElementIndex];

        GameObject enemyUnit = Instantiate(enemyUnitPrefab, enemyBattleStation);
        enemyUnit.transform.localScale = new Vector3(.1f, .1f, 1);

        EnemyUnit = enemyUnit.GetComponent<Unit>();
        EnemyUnit.element = (Element)randomEnemyElementIndex;

        EnemyUnit.attack = 3;
        EnemyUnit.defense = 3;



        //Random Element at start of game for player1


        int randomPlayerElementIndex = Random.Range(0, PlayerPrefab.Length);
        GameObject playerUnitPrefab = PlayerPrefab[randomPlayerElementIndex];

        GameObject playerUnit = Instantiate(playerUnitPrefab, playerBattleStation);
        playerUnit.transform.localScale = new Vector3(.1f, .1f, 1);
        PlayerUnit = playerUnit.GetComponent<Unit>();
        PlayerUnit.element = (Element)randomPlayerElementIndex;

        PlayerUnit.attack = 3;
        PlayerUnit.defense = 3;



        // GameObject playerGo = Instantiate(PlayerPrefab, playerBattleStation);
        //playerGo.transform.localScale = new Vector3(.1f, .1f, 1);

        //playerUnit = playerGo.GetComponent<Unit>();


        //GameObject enemyGo = Instantiate(EnemyPrefab, enemyBattleStation);
        // enemyGo.transform.localScale = new Vector3(.10f, .10f, 1);
        //EnemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "P1:" + PlayerUnit.unitName + " VS CPU:" + EnemyUnit.unitName;


        playerHUD.SetUpHUD(PlayerUnit);
        enemyHUD.SetUpHUD(EnemyUnit);

        yield return new WaitForSeconds(2f);


        Starter = Random.Range(0, 2);

        if (Starter == 0)
        {
            state = battleStateAIHard.Player1Turn;
            PlayerTurn();
        }
        else if (Starter == 1)
        {
            state = battleStateAIHard.AITurn;
            AITurn();
        }





    }


    IEnumerator AIAttack()
    {
        // if (PlayerUnit.block == true)
        //  {
        //     dialogueText.text = "Player 2 Has blocked the attack";
        // }
        // else if(PlayerUnit.block == false)
        // {

        Attack.enabled = false;
        Heal.enabled = false;
        yield return new WaitForSeconds(5f);


        bool isDead = PlayerUnit.TakeDamage(EnemyUnit.damage, EnemyUnit.element);

        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "Your opponent has dealt Damage";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = battleStateAIHard.Won;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = battleStateAIHard.Player1Turn;
            PlayerTurn();

        }
        // }

    }

    IEnumerator PlayerAttack()
    {
        Attack.enabled = true;
        
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
            state = battleStateAIHard.Won;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = battleStateAIHard.AITurn;
            AITurn();


        }



    }






    IEnumerator EndBattle()
    {
        if (state == battleStateAIHard.Won)
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


    IEnumerator PlayerHeal()
    {
        
        Heal.enabled = true;

        PlayerUnit.Heal(2);
        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "You Have Healed for: 2HP";

        yield return new WaitForSeconds(2f);

        state = battleStateAIHard.AITurn;
        AITurn();

    }


    IEnumerator AIHeal()
    {
        Attack.enabled = false;
        Heal.enabled = false;

        yield return new WaitForSeconds(5f);
        EnemyUnit.Heal(2);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        dialogueText.text = "Your Opponent Has Healed for: 2HP";


        yield return new WaitForSeconds(2f);
        state = battleStateAIHard.Player1Turn;
        PlayerTurn();
    }

    IEnumerator Player1Block()
    {
        PlayerUnit.block = true;
        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "You Have blocked for 2 damage";

        yield return new WaitForSeconds(2f);
        PlayerUnit.block = false;
        state = battleStateAIHard.AITurn;
        AITurn();


    }

    IEnumerator AIBlock()
    {
        EnemyUnit.block = true;
        enemyHUD.SetHP(EnemyUnit.currentHP);
        dialogueText.text = "Your Opponent has blocked for 2 damage";

        yield return new WaitForSeconds(2f);
        EnemyUnit.block = false;
        state = battleStateAIHard.Player1Turn;
        PlayerTurn();
    }





    public void PlayerTurn()
    {
        dialogueText.text = "Please Choose an action";
        Attack.enabled = true;
        Heal.enabled = true;
    }


    public void AITurn()
    {
        dialogueText.text = "Your Opponent is Thinking";

        aiOptions = Random.Range(0, 250);

        if (aiOptions <= 125)
        {
            if (PlayerUnit.currentHP == 20)
            {
                StartCoroutine(AIAttack());
            }
            else if (EnemyUnit.currentHP <= 10)
            {
                StartCoroutine(AIHeal());
            }

            StartCoroutine(AIAttack());

        }
        else if (aiOptions > 125)
        {
            if (PlayerUnit.currentHP == 20 && EnemyUnit.currentHP == 20)
            {
                StartCoroutine(AIAttack());

            }
            else if (PlayerUnit.currentHP <= 10)
            {
                StartCoroutine(AIAttack());
            }

            StartCoroutine(AIHeal());

        }
    }



    public void OnAttackButton()
    {
        if (state != battleStateAIHard.Player1Turn)

            return;
        StartCoroutine(PlayerAttack());

    }






    public void OnHealButton()
    {
        if (state != battleStateAIHard.Player1Turn)

            return;
        StartCoroutine(PlayerHeal());


    }

    /*public void Player1BlockButton()
    {
        if (state != battleState.Player1Turn)

            return;
        StartCoroutine(Player1Block());

    }    

    public void Player2BlockButton()
    {
        if (state != battleState.AiTurn)
            return;
        StartCoroutine(Player2Block());

    }

    public void DropDownSwitch()
    {

    }*/
}
