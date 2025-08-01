using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BriefingManager : MonoBehaviour
{

    public List<GameObject> tiles;
    public List<GameObject> units;

    public int width = 10;
    public int height = 10;

    public int selectedUnit = 1;

    public List<int> alreadySelected;

    public Color selectedTiles;

    public List<GameObject> unitPrefabs;

    public bool[] friendlyBonuses = new bool[16];
    public bool[] enemyBonuses = new bool[16];

    void Start()
    {

        int slot = PlayerPrefs.GetInt("Slot");

        string setupString = "7777777777";

        if(PlayerPrefs.GetString(slot.ToString() + "Nation", "Germany") == "Germany")
        {

            friendlyBonuses[6] = true;
            friendlyBonuses[9] = true;
            
            if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 1)
            {

                setupString = "2000000022";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 2)
            {

                setupString = "2000022220";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 3)
            {

                setupString = "0000000664";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 4)
            {

                setupString = "0022334555";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 5)
            {

                setupString = "5554444333";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 6)
            {

                setupString = "1116663444";

                enemyBonuses[2] = true;
                enemyBonuses[15] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 7)
            {

                setupString = "1112222000";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 8)
            {

                setupString = "6777111111";

                enemyBonuses[0] = true;
                enemyBonuses[1] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 9)
            {

                setupString = "5447310000";

                enemyBonuses[0] = true;
                enemyBonuses[7] = true;

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 10)
            {

                setupString = "3000117445";

                enemyBonuses[0] = true;
                enemyBonuses[7] = true;

            }

        }

        for(int i = 0; i < 10; i++)
        {

            units[i] = unitPrefabs[int.Parse("" + setupString[i])];

        }

        for(int i = 0; i < (width * height); i++)
        {
            
            GameObject unit = units[i];

            if(unit == null)
            {

                continue;

            }

            GameObject newUnit = Instantiate(unit, new Vector3((i % 10) - 4.5f, (i - (i % 10)) / -10.0f + 4.5f, 0.0f), Quaternion.identity);
            newUnit.transform.localScale = new Vector3(0.8f, 0.8f, 0.0f);

            units[i] = newUnit;

        }
        
        for(int i = 0; i < 10; i++)
        {

            if((setupString[i] == '0' || setupString[i] == '1') && enemyBonuses[0])
            {
                
                units[i].GetComponent<UnitManager>().hitpoints++;

            }
            else if((setupString[i] == '2' || setupString[i] == '3') && enemyBonuses[1])
            {

                units[i].GetComponent<UnitManager>().hitpoints++;

            }
            else if((setupString[i] == '4' || setupString[i] == '5') && enemyBonuses[2])
            {

                units[i].GetComponent<UnitManager>().hitpoints++;

            }
            else if((setupString[i] == '6' || setupString[i] == '7') && enemyBonuses[3])
            {

                units[i].GetComponent<UnitManager>().hitpoints++;

            }

        }

    }

    public void Tile(int index)
    {
        
        int slot = PlayerPrefs.GetInt("Slot");

        if(index >= 90 && !alreadySelected.Contains(index) && alreadySelected.Count < 5)
        {

            PlayerPrefs.SetInt(slot.ToString() + "Index" + selectedUnit.ToString(), index);

            alreadySelected.Add(index);

            selectedUnit += 1;

            tiles[index].GetComponent<SpriteRenderer>().color = selectedTiles;

        }

    }

    public void Confirm()
    {

        if(alreadySelected.Count == 5)
        {

            SceneManager.LoadScene("Board");

        }
        else
        {
            
            while(alreadySelected.Count < 5)
            {
                
                Tile(Random.Range(90, 99));
            
            }

            SceneManager.LoadScene("Board");

        }

    }

}
