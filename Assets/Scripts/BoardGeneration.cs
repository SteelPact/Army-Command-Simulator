using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardGeneration : MonoBehaviour
{

    public string setupString = "";
    public Color[] palette;

    public int width = 10;
    public int height = 10;

    public GameObject tile;

    public TMP_Text levelIndicator;

    public GameObject friendlyFlag;
    public GameObject enemyFlag;
    
    public Sprite[] flags;

    void Start()
    {

        int slot = PlayerPrefs.GetInt("Slot");

        setupString = "2222222222000000000000000000000000000000000000000000000000000000000000000000000000000000002222222222";
        levelIndicator.text = "Sample Scene";
        friendlyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[0];
        enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[1];

        if(PlayerPrefs.GetString(slot.ToString() + "Nation", "Germany") == "Germany")
        {

            if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 1)
            {

                setupString = "2222222222111110000011110000001111000000111110000011111000001111100000111111000011111110002222222222";
                levelIndicator.text = "Danzig, 1939";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[1];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 2)
            {

                setupString = "2222222222010000000000000000000100000000011100000000011110000000011100000111110110110000002222222222";
                levelIndicator.text = "Warsaw, 1939";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[1];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 3)
            {

                setupString = "2222222222111000000011000000001100000000110000000011000010001110001100111101110011111101102222222222";
                levelIndicator.text = "Norway, 1940";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[2];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 4)
            {

                setupString = "2222222222000000000000000000000000000000000000000000000000000000000000000000000000000000002222222222";
                levelIndicator.text = "Ardennes, 1940";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[3];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 5)
            {

                setupString = "2222222222111111000011110000001110000000110000000011000000001000000000100000000010000000002222222222";
                levelIndicator.text = "Dunkirk, 1940";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[3];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 6)
            {

                setupString = "2222222222111033333310003333330033333333033333333300333333331033333333100000000311001111002222222222";
                levelIndicator.text = "Cyrenaica, 1941";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[4];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 7)
            {

                setupString = "2222222222110000000001111100000000000100000000110000000010000000001000000000110000000001112222222222";
                levelIndicator.text = "Belgrade, 1941";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[5];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 8)
            {

                setupString = "2222222222100011111111111010101111100000111100000111000011111110001100111110000110000000002222222222";
                levelIndicator.text = "Greece, 1941";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[6];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 9)
            {

                setupString = "2222222222000000000000000000000000400000000000000000000000000000000000000000000000000000002222222222";
                levelIndicator.text = "Minsk, 1941";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[7];

            }
            else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 10)
            {

                setupString = "2222222222000000000000000000010000000111100040110011111000001110000000000000000000000000002222222222";
                levelIndicator.text = "Kiev, 1941";
                enemyFlag.GetComponent<UnityEngine.UI.Image>().sprite = flags[7];

            }
        
        }

        for(int i = 0; i < height; i++)
        {
            
            for(int j = i * width; j < (i + 1) * width; j++)
            {

                GameObject newTile = Instantiate(tile, new Vector3((j % width) - 4.5f, (height - i - 1) - 4.5f, 0.0f), Quaternion.identity);
                if(j.ToString().Length == 1)
                {

                    newTile.name = string.Concat("Tile 0", j.ToString());

                }
                else
                {

                    newTile.name = string.Concat("Tile ", j.ToString());

                }
                newTile.transform.localScale = new Vector3(0.9f, 0.9f, 0.0f);
                newTile.GetComponent<SpriteRenderer>().color = palette[setupString[j] - '0'];

                if(SceneManager.GetActiveScene().name == "Board")
                {

                    gameObject.GetComponent<BoardManager>().tiles.Add(newTile);

                }
                else if(SceneManager.GetActiveScene().name == "Briefing")
                {

                    gameObject.GetComponent<BriefingManager>().tiles.Add(newTile);

                }

            }

        }

    }

}
