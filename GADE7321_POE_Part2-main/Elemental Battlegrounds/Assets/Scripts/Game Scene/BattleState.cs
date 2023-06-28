using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum battleState
{
    Start,
    Player1Turn,
    Player2Turn,
    Won,
    Lost
}



public class BattleState : MonoBehaviour
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

    private int Starter;

    public Button Player1AttackBtn;
    public Button Player1HealBtn;
    public Button Player2AttackBtn;
    public Button Player2HealBtn;






    public battleState state;
    // Start is called before the first frame update
    void Start()
    {

        state = battleState.Start;
        Player1AttackBtn.enabled = false;
        Player1HealBtn.enabled = false;
        Player2AttackBtn.enabled = false;
        Player2HealBtn.enabled = false;
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

        dialogueText.text ="P1:"+ PlayerUnit.unitName + " VS P2:" + EnemyUnit.unitName;


        playerHUD.SetUpHUD(PlayerUnit);
        enemyHUD.SetUpHUD(EnemyUnit);

        yield return new WaitForSeconds(2f);

        Starter = Random.Range(0, 2);

        if(Starter == 0)
        {
            state = battleState.Player1Turn;
            PlayerTurn();
        }
        else if(Starter == 1)
        {
            state = battleState.Player2Turn;
            Player2Turn();
        }



    }


    IEnumerator Player2Attack()
    {
        // if (PlayerUnit.block == true)
        //  {
        //     dialogueText.text = "Player 2 Has blocked the attack";
        // }
        // else if(PlayerUnit.block == false)
        // {

        Player1AttackBtn.enabled = false;
        Player1HealBtn.enabled = false;
        Player2HealBtn.enabled = false;
            bool isDead = PlayerUnit.TakeDamage(EnemyUnit.damage, EnemyUnit.element);

            playerHUD.SetHP(PlayerUnit.currentHP);
            dialogueText.text = "Player 2 did damage";

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = battleState.Won;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = battleState.Player1Turn;
                PlayerTurn();

            }
       // }

    }

    IEnumerator PlayerAttack()
    {
        // if (EnemyUnit.block == true)
        // {
        // dialogueText.text = "Player2 Has blocked the attack";

        //  }
        //  else if(EnemyUnit.block ==false)
        //  {

        Player2AttackBtn.enabled = false;
        Player2HealBtn.enabled = false;
        Player1HealBtn.enabled = false;
            bool isDead = EnemyUnit.TakeDamage(PlayerUnit.damage, PlayerUnit.element);


            enemyHUD.SetHP(EnemyUnit.currentHP);

            dialogueText.text = "Player 1 did Damage";
            yield return new WaitForSeconds(2f);


            if (isDead)
            {
                state = battleState.Won;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = battleState.Player2Turn;
                Player2Turn();


            }
        //}


    }


    /*public void Attack(Unit attacker, Unit defender)
    {
        int damage = attacker.CalculateDamage(defender);
        defender.TakeDamage(damage);

        // Show damage in the UI
        dialogueText.text = attacker.unitName + " attacked " + defender.unitName + " for " + damage + " damage.";

        // Check if the defender is defeated
        if (defender.currentHP <= 0)
        {
            dialogueText.text = defender.unitName + " has been defeated!";


        }
    }*/

    /*
    IEnumerator EnemyTurn()
    {
        dialogueText.text = EnemyUnit.unitName + "Attacks";
        yield return new WaitForSeconds(1f);

        bool isDead = PlayerUnit.TakeDamage(EnemyUnit.damage,EnemyUnit.element);

        playerHUD.SetHP(PlayerUnit.currentHP);

        yield return new WaitForSeconds(1f);


        if (isDead)
        {
            state = battleState.Lost;
        }
        else
        {
            state = battleState.Player1Turn;
            PlayerTurn();
        }
        

        




    }*/

    IEnumerator EndBattle()
    {
        if (state == battleState.Won)
        {
            if (EnemyUnit.currentHP <= 0)
            {
                dialogueText.text = "Player 1 Has Become Victorious";
            }
            else if (PlayerUnit.currentHP <= 0)
            {
                dialogueText.text = "Player 2 Has Become Victorious";
            }

            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("MainMenu");



        }
    }


    IEnumerator PlayerHeal()
    {
        Player1AttackBtn.enabled = false;
        Player2HealBtn.enabled=false;
        Player2AttackBtn.enabled = false;

        PlayerUnit.Heal(2);
        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "Player 1 Has Healed for: 2HP";

        yield return new WaitForSeconds(2f);

        state = battleState.Player2Turn;
        Player2Turn();

    }


    IEnumerator Player2Heal()
    {
        Player1AttackBtn.enabled = false;
        Player1HealBtn.enabled = false;
        Player2AttackBtn.enabled = false;


        EnemyUnit.Heal(2);
        enemyHUD.SetHP(EnemyUnit.currentHP);
        dialogueText.text = "Player 2 Has Healed for: 2HP";


        yield return new WaitForSeconds(2f);
        state = battleState.Player1Turn;
        PlayerTurn();
    }

    IEnumerator Player1Block()
    {
        PlayerUnit.block = true;
        playerHUD.SetHP(PlayerUnit.currentHP);
        dialogueText.text = "Player 1 Has blocked for 2 damage";

        yield return new WaitForSeconds(2f);
        PlayerUnit.block = false;
        state = battleState.Player2Turn;
        Player2Turn();

        
    }

    IEnumerator Player2Block()
    {
        EnemyUnit.block = true;
        enemyHUD.SetHP(EnemyUnit.currentHP);
        dialogueText.text = "Player 2 has blocked for 2 damage";

        yield return new WaitForSeconds(2f);
        EnemyUnit.block = false;
        state = battleState.Player1Turn;
        PlayerTurn();
    }





    public void PlayerTurn()
    {
        dialogueText.text = "Player 1 Choose an action";
        Player1AttackBtn.enabled = true;
        Player1HealBtn.enabled = true;
        Player2AttackBtn.enabled = false;
        Player2HealBtn.enabled = false;
    }


    public void Player2Turn()
    {
        dialogueText.text = "Player 2 Choose an Action";
        Player2AttackBtn.enabled = true;
        Player2HealBtn.enabled = true;
        Player1AttackBtn.enabled = false;
        Player1HealBtn.enabled = false;
    }



    public void OnAttackButton()
    {
        if (state != battleState.Player1Turn)

            return;
        StartCoroutine(PlayerAttack());

    }


    public void Player2AttackButton()
    {
        if (state != battleState.Player2Turn)
            return;
        StartCoroutine(Player2Attack());
    }

    public void Player2HealButton()
    {

        if (state != battleState.Player2Turn)
            return;
        StartCoroutine(Player2Heal());

    }

    public void OnHealButton()
    {
        if (state != battleState.Player1Turn)

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
        if (state != battleState.Player2Turn)
            return;
        StartCoroutine(Player2Block());

    }

    public void DropDownSwitch()
    {

    }*/

 








}
