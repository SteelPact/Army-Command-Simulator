using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitManager : MonoBehaviour
{
    
    public GameObject prefab;
    public int i = 0;

    public int hitpoints = 10;

    public bool moved;
    
    public TMP_Text hitpointIndicator;

    public GameObject movedIndicator;

    void Update()
    {

        hitpointIndicator.text = hitpoints.ToString();

        movedIndicator.SetActive(!moved);

    }

    void OnMouseDown()
    {

        int index = int.Parse(gameObject.name.Substring(5));
        GameObject.Find("Board").GetComponent<BoardManager>().Move(index);

    }

}
