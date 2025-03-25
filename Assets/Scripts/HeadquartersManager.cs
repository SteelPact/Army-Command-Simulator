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

    public List<GameObject> unitPrefabs;

    public List<GameObject> images;

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

        foreach(GameObject image in images)
        {

            Destroy(image);

        }

        images.Clear();

        images.Add(Instantiate(unitPrefabs[unit1], new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity));
        images.Add(Instantiate(unitPrefabs[unit2], new Vector3(-7.5f, -0.25f, 0.0f), Quaternion.identity));
        images.Add(Instantiate(unitPrefabs[unit3], new Vector3(-7.5f, -1.5f, 0.0f), Quaternion.identity));
        images.Add(Instantiate(unitPrefabs[unit4], new Vector3(-7.5f, -2.75f, 0.0f), Quaternion.identity));
        images.Add(Instantiate(unitPrefabs[unit5], new Vector3(-7.5f, -4.0f, 0.0f), Quaternion.identity));

        int hitpoints1 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "1", 10);
        int hitpoints2 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "2", 10);
        int hitpoints3 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "3", 10);
        int hitpoints4 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "4", 10);
        int hitpoints5 = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + "5", 10);

        images[0].GetComponent<UnitDataStorage>().hitpointIndicator.text = hitpoints1.ToString();
        images[1].GetComponent<UnitDataStorage>().hitpointIndicator.text = hitpoints2.ToString();
        images[2].GetComponent<UnitDataStorage>().hitpointIndicator.text = hitpoints3.ToString();
        images[3].GetComponent<UnitDataStorage>().hitpointIndicator.text = hitpoints4.ToString();
        images[4].GetComponent<UnitDataStorage>().hitpointIndicator.text = hitpoints5.ToString();

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
