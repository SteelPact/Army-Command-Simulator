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

    void Start()
    {

        int slot = PlayerPrefs.GetInt("Slot");

        string setupString = "7777777777";

        if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 1)
        {

            setupString = "2000000022";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 2)
        {

            setupString = "2000022220";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 3)
        {

            setupString = "0000000664";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 4)
        {

            setupString = "0022334555";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 5)
        {

            setupString = "5554444333";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 6)
        {

            setupString = "1116663444";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 7)
        {

            setupString = "1112222000";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 8)
        {

            setupString = "6777111111";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 9)
        {

            setupString = "5447321111";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 10)
        {

            setupString = "3000117445";

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
