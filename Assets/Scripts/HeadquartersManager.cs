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
    public TMP_Text moneyIndicator2;

    public List<TMP_Text> units;
    public string[] unitNames;

    public List<TMP_Text> expandIndicator;
    public List<TMP_Text> upgradeIndicator;

    public TMP_Text nextMaps;

    public GameObject canvasMain;
    public GameObject canvasUpgrade;
    //public GameObject canvasResearch;

    void Start()
    {

        slot = PlayerPrefs.GetInt("Slot");

        titleText.text = "Campaign -	" + PlayerPrefs.GetString(slot.ToString() + "Nation", "Germany") + " -	" + PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard");

        int level = PlayerPrefs.GetInt(slot.ToString() + "Level", 1);

        if(PlayerPrefs.GetString(slot.ToString() + "Nation", "Germany") == "Germany")
        {

            if(level >= 1 && level <= 8)
            {

                nextMaps.text = "Part 1 - Level " + level.ToString() + "\n\n1. Danzig, 1939\n2. Warsaw, 1939\n3. Norway, 1940\n4. Ardennes, 1940\n5. Dunkirk, 1940\n6. Cyrenaica, 1941\n7. Belgrade, 1941\n8. Greece, 1941";

            }
            else if(level >= 9)
            {

                nextMaps.text = "Part 2 - Level " + level.ToString() + "\n\n9. Belarus, 1941\n10. Kiev, 1941\n11. Smolensk, 1941\n12. Leningrad, 1941\n13. Moscow, 1941\n14. Rostov, 1941\n15. Stalingrad, 1942\n16. Kursk, 1943";

            }
        
        }

    }

    void Update()
    {

        money = PlayerPrefs.GetInt(slot.ToString() + "Money", 0);
        moneyIndicator.text = "$" + money.ToString();
        moneyIndicator2.text = "$" + money.ToString();
        
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

        units[0].text = unitNames[unit1] + " - Size " + hitpoints1.ToString();
        units[1].text = unitNames[unit2] + " - Size " + hitpoints2.ToString();
        units[2].text = unitNames[unit3] + " - Size " + hitpoints3.ToString();
        units[3].text = unitNames[unit4] + " - Size " + hitpoints4.ToString();
        units[4].text = unitNames[unit5] + " - Size " + hitpoints5.ToString();
        units[5].text = unitNames[unit1] + " - Size " + hitpoints1.ToString();
        units[6].text = unitNames[unit2] + " - Size " + hitpoints2.ToString();
        units[7].text = unitNames[unit3] + " - Size " + hitpoints3.ToString();
        units[8].text = unitNames[unit4] + " - Size " + hitpoints4.ToString();
        units[9].text = unitNames[unit5] + " - Size " + hitpoints5.ToString();

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
            
            expandIndicator[i].text = "Expand - $" + Cost(i + 1).ToString();   
            if(Cost2(i + 1) != 0)
            {
                
                upgradeIndicator[i].text = "Upgrade - $" + Cost2(i + 1).ToString();

            }
            else
            {

                upgradeIndicator[i].text = "Upgrade - $N/A";

            }

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

    public int Cost2(int unit)
    {

        int unitType = PlayerPrefs.GetInt(slot.ToString() + "Unit" + unit.ToString());
        int unitHitpoints = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + unit.ToString());

        int costPerUpgrade = 0;

        if(unitType == 0)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 2)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 4)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 6)
        {

            costPerUpgrade = 2;

        }

        if(PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard") == "Easy")
        {

            return 10 * unitHitpoints * costPerUpgrade;

        }
        return 20 * unitHitpoints * costPerUpgrade;

    }

    public void Expand(int unit)
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
        if(PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard") == "Easy")
        {

            cost = 5 * (unitHitpoints - 9) * costPerUpgrade;

        }

        if(PlayerPrefs.GetInt(slot.ToString() + "Money") >= cost)
        {

            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - cost);
            PlayerPrefs.SetInt(slot.ToString() + "Hitpoints" + unit.ToString(), unitHitpoints + 1);

        }

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
        else if(unitType == 2)
        {

            costPerUpgrade = 1;

        }
        else if(unitType == 4)
        {

            costPerUpgrade = 2;

        }
        else if(unitType == 6)
        {

            costPerUpgrade = 2;

        }

        if(costPerUpgrade == 0)
        {

            return;

        }

        int cost = 20 * unitHitpoints * costPerUpgrade;
        if(PlayerPrefs.GetString(slot.ToString() + "Difficulty", "Hard") == "Easy")
        {

            cost = 10 * unitHitpoints * costPerUpgrade;

        }
        
        if(unitType % 2 == 0)
        {

            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - cost);
            PlayerPrefs.SetInt(slot.ToString() + "Unit" + unit.ToString(), unitType + 1);

        }

    }

    public void ChangeCanvasUpgrade()
    {

        canvasMain.SetActive(!canvasMain.activeSelf);
        canvasUpgrade.SetActive(!canvasUpgrade.activeSelf);

    }

    public void ChangeCanvasResearch()
    {

        canvasMain.SetActive(!canvasMain.activeSelf);
        //canvasResearch.SetActive(!canvasResearch.activeSelf);

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
