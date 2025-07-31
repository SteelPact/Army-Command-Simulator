using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HeadquartersManager : MonoBehaviour
{

    public int slot;

    public int money;

    public TMP_Text titleText;

    public TMP_Text moneyIndicator;

    public List<TMP_Text> units;
    public string[] unitNames;

    public List<TMP_Text> upgradeIndicator;

    void Start()
    {

        slot = PlayerPrefs.GetInt("Slot");

        titleText.text = "Campaign -	" + PlayerPrefs.GetString(slot.ToString() + "Nation", "Germany") + " -	" + PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard");

    }

    void Update()
    {

        money = PlayerPrefs.GetInt(slot.ToString() + "Money", 0);
        moneyIndicator.text = "$" + money.ToString();
        
        int unit1 = PlayerPrefs.GetInt(slot.ToString() + "Unit" + "1", 0);
        int unit2 = PlayerPrefs.GetInt(slot.ToString() + "Unit" + "2", 2);
        int unit3 = PlayerPrefs.GetInt(slot.ToString() + "Unit" + "3", 4);
        int unit4 = PlayerPrefs.GetInt(slot.ToString() + "Unit" + "4", 6);
        int unit5 = PlayerPrefs.GetInt(slot.ToString() + "Unit" + "5", 4);

        int hitpoints1 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "1", 10);
        int hitpoints2 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "2", 10);
        int hitpoints3 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "3", 10);
        int hitpoints4 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "4", 10);
        int hitpoints5 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "5", 10);

        units[0].text = unitNames[unit1] + " - Level " + hitpoints1.ToString();
        units[1].text = unitNames[unit2] + " - Level " + hitpoints2.ToString();
        units[2].text = unitNames[unit3] + " - Level " + hitpoints3.ToString();
        units[3].text = unitNames[unit4] + " - Level " + hitpoints4.ToString();
        units[4].text = unitNames[unit5] + " - Level " + hitpoints5.ToString();

        PlayerPrefs.SetInt(slot.ToString() + "Money", money);

        PlayerPrefs.SetInt(slot.ToString() + "Unit" + "1", unit1);
        PlayerPrefs.SetInt(slot.ToString() + "Unit" + "2", unit2);
        PlayerPrefs.SetInt(slot.ToString() + "Unit" + "3", unit3);
        PlayerPrefs.SetInt(slot.ToString() + "Unit" + "4", unit4);
        PlayerPrefs.SetInt(slot.ToString() + "Unit" + "5", unit5);

        PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + "1", hitpoints1);
        PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + "2", hitpoints2);
        PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + "3", hitpoints3);
        PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + "4", hitpoints4);
        PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + "5", hitpoints5);

        for(int i = 0; i < 5; i++)
        {
            
            upgradeIndicator[i].text = "Upgrade - $" + Cost(i + 1).ToString();

        }

    }

    public int Cost(int unit)
    {

        int unitType = PlayerPrefs.GetInt(slot.ToString() + "Unit" + unit.ToString());
        int unitHitpoints = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + unit.ToString());

        int costPerUpgrade = 0;

        if(unitType == 0)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 1)
        {

            costPerUpgrade = 3;

        }
        else if(unitType == 2)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 3)
        {

            costPerUpgrade = 3;

        }
        else if(unitType == 4)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 5)
        {

            costPerUpgrade = 5;

        }
        else if(unitType == 6)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 7)
        {

            costPerUpgrade = 4;

        }

        if(PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard") == "Easy")
        {

            return 5 * (unitHitpoints - 9) * costPerUpgrade;

        }
        return 10 * (unitHitpoints - 9) * costPerUpgrade;

    }

    public void Upgrade(int unit)
    {
        
        int unitType = PlayerPrefs.GetInt(slot.ToString() + "Unit" + unit.ToString());
        int unitHitpoints = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + unit.ToString());

        int costPerUpgrade = 0;

        if(unitType == 0)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 1)
        {

            costPerUpgrade = 3;

        }
        else if(unitType == 2)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 3)
        {

            costPerUpgrade = 3;

        }
        else if(unitType == 4)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 5)
        {

            costPerUpgrade = 5;

        }
        else if(unitType == 6)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 7)
        {

            costPerUpgrade = 4;

        }

        int cost = 10 * (unitHitpoints - 9) * costPerUpgrade;

        if(PlayerPrefs.GetInt(slot.ToString() + "Money") >= cost)
        {

            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - cost);
            PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + unit.ToString(), unitHitpoints + 1);

        }
        
        if(PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + unit.ToString()) == 16 && unitType % 2 == 0)
        {

            PlayerPrefs.SetInt(slot.ToString() + "Unit" + unit.ToString(), PlayerPrefs.GetInt(slot.ToString() + "Unit" + unit.ToString()) + 1);
            PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + unit.ToString(), 10);

        }

    }

    public void Play()
    {

        SceneManager.LoadScene("Briefing");

    }

    public void Exit()
    {

        SceneManager.LoadScene("MainMenu");

    }

}
