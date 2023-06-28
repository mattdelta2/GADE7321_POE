using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element
{
    Fire,
    Water,
    Air,
    Lightning,
    Rock,
    Sun,
    Plant
}



public class Unit : MonoBehaviour
{

    public string unitName;

    public int damage;
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defense;
    public bool block = false;

    



    public Element element;



    public int CalculateDamage(Unit defender)
    {
        int damage = attack - defender.defense;

        // Check for elemental strength and weakness
        if (element == Element.Fire && defender.element == Element.Plant)
        {
            damage *= 2;
        }
        else if (element == Element.Water && defender.element == Element.Fire)
        {
            damage *= 2;
        }
        else if (element == Element.Plant && defender.element == Element.Air)
        {
            damage *= 2;
        }
        else if (element == Element.Air && defender.element == Element.Rock)
        {
            damage *= 2;
        }
        else if (element == Element.Rock && defender.element == Element.Lightning)
        {
            damage *= 2;
        }
        else if (element == Element.Lightning && defender.element == Element.Sun)
        {
            damage *= 2;
        }
        else if (element == Element.Sun && defender.element == Element.Water)
        {
            damage *= 2;
        }
        else if (element == defender.element)
        {
            damage /= 2;
        }

        return damage;
    }



    public bool TakeDamage(int damage, Element attackerElement)
    {
        int modifiedDamage = damage;
        if (attackerElement == Element.Fire && element == Element.Plant)
        {
            modifiedDamage *= 1;
        }
        else if (attackerElement == Element.Water && element == Element.Fire)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == Element.Plant && element == Element.Air)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == Element.Air && element == Element.Rock)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == Element.Rock && element == Element.Lightning)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == Element.Lightning && element == Element.Sun)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == Element.Sun && element == Element.Water)
        {
            modifiedDamage *= 2;
        }
        else if (attackerElement == element)
        {
            modifiedDamage /= 2;
        }

        currentHP -= modifiedDamage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }



    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

    }

 /*   public void Block()
    {
        if(block == true)
        {
            damage = 0;
            block = false;
        }
        else if(block == false)
        {
            return;
        }
    }*/


}
