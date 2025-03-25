using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{
    
    void OnMouseDown()
    {

        int index = int.Parse(gameObject.name.Substring(5));
        if(SceneManager.GetActiveScene().name == "Board")
        {

            GameObject.Find("Board").GetComponent<BoardManager>().Tile(index);

        }
        else if(SceneManager.GetActiveScene().name == "Briefing")
        {

            GameObject.Find("Board").GetComponent<BriefingManager>().Tile(index);

        }

    }

}
