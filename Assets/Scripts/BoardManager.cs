using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardManager : MonoBehaviour
{

    public int slot;

    public List<GameObject> tiles;
    public List<GameObject> units;

    public GameObject semiFriendlyInfantry;
    public GameObject semiEnemyInfantry;

    public int width = 10;
    public int height = 10;

    public int[] speed;
    public int[] softAttack;
    public int[] hardAttack;
    public int[] softDefense;
    public int[] hardDefense;
    public int[] range;

    public List<GameObject> unitPrefabs;

    public TMP_Text moneyIndicator;

    public bool[] friendlyBonuses = new bool[16];
    public bool[] enemyBonuses = new bool[16];

    void Start()
    {
        
        slot = PlayerPrefs.GetInt("Slot");

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

            units[i] = unitPrefabs[int.Parse("" + setupString[i]) + 8];

        }

        for(int i = 1; i <= 5; i++)
        {
            
            int unit = PlayerPrefs.GetInt(slot.ToString() + "Unit" + i.ToString());
            int index = PlayerPrefs.GetInt(slot.ToString() + "Index" + i.ToString());

            units[index] = unitPrefabs[unit];

        }

        for(int i = 90; i < 100; i++)
        {

            if(units[i] == null)
            {

                units[i] = semiFriendlyInfantry;

            }

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
            if(i.ToString().Length == 1)
            {

                newUnit.name = string.Concat("Unit 0", i.ToString());

            }
            else
            {

                newUnit.name = string.Concat("Unit ", i.ToString());

            }

            newUnit.GetComponent<UnitManager>().prefab = units[i];
            units[i] = newUnit;

        }

        for(int i = 1 ; i <= 5; i++)
        {

            int hitpoints = PlayerPrefs.GetInt(slot.ToString() + "Hitpoints" + i.ToString());
            int index = PlayerPrefs.GetInt(slot.ToString() + "Index" + i.ToString());

            units[index].GetComponent<UnitManager>().i = i;
            units[index].GetComponent<UnitManager>().hitpoints = hitpoints;

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

    void Update()
    {

        for(int i = 0; i < (width * height); i++)
        {

            tiles[i].GetComponent<BoxCollider2D>().enabled = (units[i] == null);

        }

        moneyIndicator.text = "$" + PlayerPrefs.GetInt(slot.ToString() + "Money").ToString();

    }

    int type;

    public List<int> possibleMoveIndexes;
    public List<int> possibleAttackIndexes;

    public List<Color> moveTileColors;
    public List<Color> attackTileColors;

    public int selectedUnit;

    public Color moveTiles;
    public Color attackTiles;
    public Color myTile;

    public void BreadthFirstSearchMove(int index, int speed, int type)
    {

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        int[] visited = new int[width * height];

        visited[index] = 1;
        queue.Enqueue(new Vector2Int(index, speed));

        while(queue.Count > 0)
        {

            Vector2Int newIndexAndRemainingSpeed = queue.Dequeue();
            int newIndex = newIndexAndRemainingSpeed[0];
            int remainingSpeed = newIndexAndRemainingSpeed[1];

            List<int> neighbours = new List<int>();

            if(newIndex > 9) 
            {

                if(units[newIndex - 10] == null)
                {

                    neighbours.Add(newIndex - 10);

                }

            }
            if(newIndex < 90) 
            {
                
                if(units[newIndex + 10] == null)
                {
                
                    neighbours.Add(newIndex + 10);
                
                }

            }
            if((newIndex % 10) != 0) 
            {

                if(units[newIndex - 1] == null)
                {

                    neighbours.Add(newIndex - 1);

                }

            }
            if((newIndex % 10) != 9) 
            {
                
                if(units[newIndex + 1] == null)
                {

                    neighbours.Add(newIndex + 1);

                }

            }

            foreach(int neighbour in neighbours)
            {

                if(visited[neighbour] < 2)
                {

                    int cost = 0;
                    int tile = int.Parse("" + gameObject.GetComponent<BoardGeneration>().setupString[neighbour]);

                    if(tile == 0)
                    {

                        cost = 1;

                    }
                    else if(tile == 1)
                    {
                        
                        cost = 21;
                        if(type == 1)
                        {

                            cost = 1;

                        }

                    }
                    else if(tile == 2)
                    {

                        cost = 0;

                    }
                    else if(tile == 3)
                    {
                        
                        cost = 2;

                    }
                    else if(tile == 4)
                    {

                        cost = 1;

                    }
                    else if(tile == 5)
                    {

                        cost = 1;

                    }

                    if(remainingSpeed - cost >= 0)
                    {
                        
                        visited[neighbour] += 1;
                        queue.Enqueue(new Vector2Int(neighbour, remainingSpeed - cost));

                    }

                }

            }

        }

        for(int i = 0; i < visited.Length; i++)
        {

            if(visited[i] > 0 && i != index)
            {

                possibleMoveIndexes.Add(i);

            }

        }

    }

    public void BreadthFirstSearchAttack(int index, int range)
    {

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[] visited = new bool[width * height];

        visited[index] = true;
        queue.Enqueue(new Vector2Int(index, range));

        while(queue.Count > 0)
        {

            Vector2Int newIndexAndRemainingRange = queue.Dequeue();
            int newIndex = newIndexAndRemainingRange[0];
            int remainingRange = newIndexAndRemainingRange[1];

            List<int> neighbours = new List<int>();

            if(newIndex > 9) 
            {

                neighbours.Add(newIndex - 10);

            }
            if(newIndex < 90) 
            {
                
                neighbours.Add(newIndex + 10);

            }
            if((newIndex % 10) != 0) 
            {

                neighbours.Add(newIndex - 1);


            }
            if((newIndex % 10) != 9) 
            {
                
                neighbours.Add(newIndex + 1);

            }

            foreach(int neighbour in neighbours)
            {

                if(!visited[neighbour] && remainingRange > 0)
                {

                    visited[neighbour] = true;
                    queue.Enqueue(new Vector2Int(neighbour, remainingRange - 1));

                }

            }

        }

        bool enemy1 = units[index].tag == "EI" || units[index].tag == "ES" || units[index].tag == "EC" || units[index].tag == "EM" || units[index].tag == "EL" || units[index].tag == "EH" || units[index].tag == "EA" || units[index].tag == "ER";
        for(int i = 0; i < visited.Length; i++)
        {

            bool enemy2 = false;

            if(units[i] != null)
            {
                
                enemy2 = units[i].tag == "EI" || units[i].tag == "ES" || units[i].tag == "EC" || units[i].tag == "EM" || units[i].tag == "EL" || units[i].tag == "EH" || units[i].tag == "EA" || units[i].tag == "ER";

            }

            if(visited[i] && i != index && (enemy1 != enemy2) && units[i] != null)
            {

                possibleAttackIndexes.Add(i);

            }

        }

    }

    public void Move(int index)
    {
        
        int bonusMovement = 0;
        int bonusRange = 0;

        string tag = units[index].tag;
        if(tag == "FI" || tag == "EI")
        {

            type = 0;
            
            if(friendlyBonuses[8])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[12])
            {

                bonusRange++;

            }

        }
        else if(tag == "FS" || tag == "ES")
        {

            type = 1;

            if(friendlyBonuses[8])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[12])
            {

                bonusRange++;

            }

        }
        else if(tag == "FC" || tag == "EC")
        {

            type = 2;

            if(friendlyBonuses[9])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[13])
            {

                bonusRange++;

            }

        }
        else if(tag == "FM" || tag == "EM")
        {

            type = 3;

            if(friendlyBonuses[9])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[13])
            {

                bonusRange++;

            }

        }
        else if(tag == "FL" || tag == "EL")
        {

            type = 4;

            if(friendlyBonuses[10])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[14])
            {

                bonusRange++;

            }

        }
        else if(tag == "FH" || tag == "EH")
        {

            type = 5;

            if(friendlyBonuses[10])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[14])
            {

                bonusRange++;

            }

        }
        else if(tag == "FA" || tag == "EA")
        {

            type = 6;

            if(friendlyBonuses[11])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[15])
            {

                bonusRange++;

            }

        }
        else if(tag == "FR" || tag == "ER")
        {

            type = 7;

            if(friendlyBonuses[11])
            {

                bonusMovement++;

            }
            if(friendlyBonuses[15])
            {

                bonusRange++;

            }

        }
        
        if(selectedUnit == -1 && !units[index].GetComponent<UnitManager>().moved && (tag == "FI" || tag == "FS" || tag == "FC" || tag == "FM" || tag == "FL" || tag == "FH" || tag == "FA" || tag == "FR"))
        {

            BreadthFirstSearchMove(index, speed[type] + bonusMovement, type);
            BreadthFirstSearchAttack(index, range[type] + bonusRange);

            foreach(int possibleMoveIndex in possibleMoveIndexes)
            {
        
                moveTileColors.Add(tiles[possibleMoveIndex].GetComponent<SpriteRenderer>().color);
                tiles[possibleMoveIndex].GetComponent<SpriteRenderer>().color = moveTiles;

            }
            moveTileColors.Add(tiles[index].GetComponent<SpriteRenderer>().color);
            tiles[index].GetComponent<SpriteRenderer>().color = myTile;
            foreach(int possibleAttackIndex in possibleAttackIndexes)
            {
        
                attackTileColors.Add(tiles[possibleAttackIndex].GetComponent<SpriteRenderer>().color);
                tiles[possibleAttackIndex].GetComponent<SpriteRenderer>().color = attackTiles;

            }
            attackTileColors.Add(tiles[index].GetComponent<SpriteRenderer>().color);
            tiles[index].GetComponent<SpriteRenderer>().color = myTile;

            selectedUnit = index;

        }
        else if(tag == "FI" || tag == "FS" || tag == "FC" || tag == "FM" || tag == "FL" || tag == "FH" || tag == "FA" || tag == "FR")
        {

            Tile(-1);

        }
        else if(possibleAttackIndexes.Contains(index))
        {

            units[selectedUnit].GetComponent<UnitManager>().moved = true;
            
            int type1 = 0;
            int type2 = 0;
            bool hard1 = false;
            bool hard2 = false;

            int bonusAttack = 0;
            int bonusAttack2 = 0;

            string tag1 = units[selectedUnit].tag;
            if(tag1 == "FI" || tag1 == "EI")
            {

                type1 = 0;
                hard1 = false;
                
                if(friendlyBonuses[4])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FS" || tag1 == "ES")
            {

                type1 = 1;
                hard1 = false;

                if(friendlyBonuses[4])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FC" || tag1 == "EC")
            {
                
                type1 = 2;
                hard1 = false;
                
                if(friendlyBonuses[5])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FM" || tag1 == "EM")
            {
                
                type1 = 3;
                hard1 = false;

                if(friendlyBonuses[5])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FL" || tag1 == "EL")
            {
                
                type1 = 4;
                hard1 = true;

                if(friendlyBonuses[6])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FH" || tag1 == "EH")
            {
                
                type1 = 5;
                hard1 = true;

                if(friendlyBonuses[6])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FA" || tag1 == "EA")
            {
                
                type1 = 6;
                hard1 = false;

                if(friendlyBonuses[7])
                {

                    bonusAttack++;

                }

            }
            else if(tag1 == "FR" || tag1 == "ER")
            {
                
                type1 = 7;
                hard1 = false;

                if(friendlyBonuses[7])
                {

                    bonusAttack++;

                }

            }

            int bonusRange2 = 0;

            string tag2 = units[index].tag;
            if(tag2 == "FI" || tag2 == "EI")
            {

                type2 = 0;
                hard2 = false;

                if(enemyBonuses[4])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[12])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FS" || tag2 == "ES")
            {

                type2 = 1;
                hard2 = false;

                if(enemyBonuses[4])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[12])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FC" || tag2 == "EC")
            {

                type2 = 2;
                hard2 = false;

                if(enemyBonuses[5])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[13])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FM" || tag2 == "EM")
            {

                type2 = 3;
                hard2 = false;

                if(enemyBonuses[5])
                {

                    bonusAttack2++;

                }   
                if(enemyBonuses[13])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FL" || tag2 == "EL")
            {

                type2 = 4;
                hard2 = true;

                if(enemyBonuses[6])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[14])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FH" || tag2 == "EH")
            {

                type2 = 5;
                hard2 = true;

                if(enemyBonuses[6])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[14])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FA" || tag2 == "EA")
            {

                type2 = 6;
                hard2 = false;

                if(enemyBonuses[7])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[15])
                {

                    bonusRange2++;

                }

            }
            else if(tag2 == "FR" || tag2 == "ER")
            {

                type2 = 7;
                hard2 = false;

                if(enemyBonuses[7])
                {

                    bonusAttack2++;

                }
                if(enemyBonuses[15])
                {

                    bonusRange2++;

                }

            }

            List<int> copyOfPossibleAttackIndexes = new List<int>();
            foreach(int possibleAttackIndex in possibleAttackIndexes)
            {

                copyOfPossibleAttackIndexes.Add(possibleAttackIndex);

            }
            possibleAttackIndexes.Clear();
            BreadthFirstSearchAttack(index, range[type2] + bonusRange2);

            if(hard1 && hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                if(units[index].GetComponent<UnitManager>().hitpoints < 1)
                {
                    
                    if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                    {

                        PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                    }

                    Destroy(units[index]);
                    units[index] = null;

                }
                else if(possibleAttackIndexes.Contains(selectedUnit))
                {

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(hard1 && !hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                if(units[index].GetComponent<UnitManager>().hitpoints < 1)
                {
                    
                    if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                    {

                        PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                    }

                    Destroy(units[index]);
                    units[index] = null;

                }
                else if(possibleAttackIndexes.Contains(selectedUnit))
                {

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(!hard1 && hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                if(units[index].GetComponent<UnitManager>().hitpoints < 1)
                {

                    if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                    {

                        PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                    }

                    Destroy(units[index]);
                    units[index] = null;

                }
                else if(possibleAttackIndexes.Contains(selectedUnit))
                {

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(!hard1 && !hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                if(units[index].GetComponent<UnitManager>().hitpoints < 1)
                {

                    if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                    {

                        PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                    }

                    Destroy(units[index]);
                    units[index] = null;

                }
                else if(possibleAttackIndexes.Contains(selectedUnit))
                {

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }

            possibleAttackIndexes.Clear();
            foreach(int copyOfPossibleAttackIndex in copyOfPossibleAttackIndexes)
            {

                possibleAttackIndexes.Add(copyOfPossibleAttackIndex);

            }

            Tile(-1);

        }

    }

    public void Tile(int index)
    {

        if(possibleMoveIndexes.Contains(index))
        {

            units[index] = units[selectedUnit];

            GameObject newUnit = Instantiate(units[selectedUnit].GetComponent<UnitManager>().prefab, new Vector3((index % 10) - 4.5f, (index - (index % 10)) / -10.0f + 4.5f, 0.0f), Quaternion.identity);
            newUnit.transform.localScale = new Vector3(0.8f, 0.8f, 0.0f);
            if(index.ToString().Length == 1)
            {

                newUnit.name = string.Concat("Unit 0", index.ToString());

            }
            else
            {

                newUnit.name = string.Concat("Unit ", index.ToString());

            }

            units[index] = newUnit;
            newUnit.GetComponent<UnitManager>().moved = true;
            newUnit.GetComponent<UnitManager>().prefab = units[selectedUnit].GetComponent<UnitManager>().prefab;
            newUnit.GetComponent<UnitManager>().hitpoints = units[selectedUnit].GetComponent<UnitManager>().hitpoints;
            newUnit.GetComponent<UnitManager>().i = units[selectedUnit].GetComponent<UnitManager>().i;

            GameObject unit = units[selectedUnit];
            Destroy(unit);

            units[selectedUnit] = null;

            Tile(-1);

        }
        else
        {
            
            if(moveTileColors.Count != 0)
            {

                for(int i = 0; i < possibleMoveIndexes.Count; i++)
                {

                tiles[possibleMoveIndexes[i]].GetComponent<SpriteRenderer>().color = moveTileColors[i];

                }
                tiles[selectedUnit].GetComponent<SpriteRenderer>().color = moveTileColors[moveTileColors.Count - 1];

            }

            possibleMoveIndexes.Clear();
            moveTileColors.Clear();

            for(int i = 0; i < possibleAttackIndexes.Count; i++)
            {

                tiles[possibleAttackIndexes[i]].GetComponent<SpriteRenderer>().color = attackTileColors[i];

            }

            possibleAttackIndexes.Clear();
            attackTileColors.Clear();

            selectedUnit = -1;

        }

    }

    public void EndTurn()
    {
        
        Tile(-1);

        int friendlies = 0;

        List<int> enemies = new List<int>();
        List<bool> enemiesHaveMoved = new List<bool>();

        for(int i = 0; i < (width * height); i++)
        {
            
            if(units[i] != null)
            {

                units[i].GetComponent<UnitManager>().moved = false;

                if(units[i].tag == "EI" || units[i].tag == "ES" || units[i].tag == "EC" || units[i].tag == "EM" || units[i].tag == "EL" || units[i].tag == "EH" || units[i].tag == "EA" || units[i].tag == "ER")
                {

                    enemies.Add(i);
                    enemiesHaveMoved.Add(false);

                }
                else
                {

                    friendlies += 1;

                }

            }
            else
            {

                if(int.Parse("" + gameObject.GetComponent<BoardGeneration>().setupString[i]) == 4)
                {

                    GameObject newUnit = Instantiate(semiEnemyInfantry, new Vector3((i % 10) - 4.5f, (i - (i % 10)) / -10.0f + 4.5f, 0.0f), Quaternion.identity);
                    newUnit.transform.localScale = new Vector3(0.8f, 0.8f, 0.0f);
                    if(i.ToString().Length == 1)
                    {

                        newUnit.name = string.Concat("Unit 0", i.ToString());

                    }
                    else
                    {

                        newUnit.name = string.Concat("Unit ", i.ToString());

                    }

                    newUnit.GetComponent<UnitManager>().prefab = semiEnemyInfantry;
                    units[i] = newUnit;

                    enemies.Add(i);
                    enemiesHaveMoved.Add(false);

                    if(enemyBonuses[0])
                    {

                        newUnit.GetComponent<UnitManager>().hitpoints++;

                    }

                }

            }

        }

        if(friendlies == 0)
        {

            EndGame();

        }
        else if(enemies.Count == 0)
        {
            
            PlayerPrefs.SetInt(slot.ToString() + "Level", PlayerPrefs.GetInt(slot.ToString() + "Level", 1) + 2);

            EndGame();

        }
        else
        {

            for(int i = enemies.Count - 1; i >= 0; i--)
            {
                
                selectedUnit = enemies[i];

                int bonusMovement = 0;
                int bonusRange = 0;

                string tag = units[selectedUnit].tag;
                if(tag == "FI" || tag == "EI")
                {

                    type = 0;
                    
                    if(enemyBonuses[8])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[12])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FS" || tag == "ES")
                {

                    type = 1;

                    if(enemyBonuses[8])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[12])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FC" || tag == "EC")
                {

                    type = 2;

                    if(enemyBonuses[9])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[13])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FM" || tag == "EM")
                {

                    type = 3;

                    if(enemyBonuses[9])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[13])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FL" || tag == "EL")
                {

                    type = 4;

                    if(enemyBonuses[10])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[14])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FH" || tag == "EH")
                {

                    type = 5;

                    if(enemyBonuses[10])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[14])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FA" || tag == "EA")
                {

                    type = 6;

                    if(enemyBonuses[11])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[15])
                    {

                        bonusRange++;

                    }

                }
                else if(tag == "FR" || tag == "ER")
                {

                    type = 7;

                    if(enemyBonuses[11])
                    {

                        bonusMovement++;

                    }
                    if(enemyBonuses[15])
                    {

                        bonusRange++;

                    }

                }

                possibleMoveIndexes.Clear();
                possibleAttackIndexes.Clear();
                BreadthFirstSearchAttack(selectedUnit, range[type] + bonusRange);

                if(possibleAttackIndexes.Count > 0)
                {

                    units[selectedUnit].GetComponent<UnitManager>().moved = true;
            
                    int type1 = 0;
                    int type2 = 0;
                    bool hard1 = false;
                    bool hard2 = false;

                    int bonusAttack = 0;
                    int bonusAttack2 = 0;

                    string tag1 = units[selectedUnit].tag;
                    if(tag1 == "FI" || tag1 == "EI")
                    {
                    
                        type1 = 0;
                        hard1 = false;

                        if(enemyBonuses[4])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FS" || tag1 == "ES")
                    {
                    
                        type1 = 1;
                        hard1 = false;

                        if(enemyBonuses[4])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FC" || tag1 == "EC")
                    {

                        type1 = 2;
                        hard1 = false;

                        if(enemyBonuses[5])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FM" || tag1 == "EM")
                    {

                        type1 = 3;
                        hard1 = false;

                        if(enemyBonuses[5])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FL" || tag1 == "EL")
                    {

                        type1 = 4;
                        hard1 = true;

                        if(enemyBonuses[6])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FH" || tag1 == "EH")
                    {

                        type1 = 5;
                        hard1 = true;

                        if(enemyBonuses[6])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FA" || tag1 == "EA")
                    {

                        type1 = 6;
                        hard1 = false;

                        if(enemyBonuses[7])
                        {

                            bonusAttack++;

                        }

                    }
                    else if(tag1 == "FR" || tag1 == "ER")
                    {

                        type1 = 7;
                        hard1 = false;

                        if(enemyBonuses[7])
                        {

                            bonusAttack++;

                        }

                    }

                    int attackIndex = 0;
                    int lowestHitpoints = 2147483647;

                    foreach(int possibleAttackIndex in possibleAttackIndexes)
                    {

                        if(units[possibleAttackIndex].GetComponent<UnitManager>().hitpoints < lowestHitpoints)
                        {

                            lowestHitpoints = units[possibleAttackIndex].GetComponent<UnitManager>().hitpoints;
                            attackIndex = possibleAttackIndex;

                        }

                    }

                    int bonusRange2 = 0;

                    string tag2 = units[attackIndex].tag;
                    if(tag2 == "FI" || tag2 == "EI")
                    {

                        type2 = 0;
                        hard2 = false;

                        if(friendlyBonuses[4])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[12])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FS" || tag2 == "ES")
                    {

                        type2 = 1;
                        hard2 = false;

                        if(friendlyBonuses[4])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[12])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FC" || tag2 == "EC")
                    {

                        type2 = 2;
                        hard2 = false;

                        if(friendlyBonuses[5])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[13])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FM" || tag2 == "EM")
                    {

                        type2 = 3;
                        hard2 = false;

                        if(friendlyBonuses[5])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[13])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FL" || tag2 == "EL")
                    {

                        type2 = 4;
                        hard2 = true;

                        if(friendlyBonuses[6])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[14])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FH" || tag2 == "EH")
                    {

                        type2 = 5;
                        hard2 = true;

                        if(friendlyBonuses[6])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[14])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FA" || tag2 == "EA")
                    {

                        type2 = 6;
                        hard2 = false;

                        if(friendlyBonuses[7])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[15])
                        {

                            bonusRange2++;

                        }

                    }
                    else if(tag2 == "FR" || tag2 == "ER")
                    {

                        type2 = 7;
                        hard2 = false;

                        if(friendlyBonuses[7])
                        {

                            bonusAttack2++;

                        }
                        if(friendlyBonuses[15])
                        {

                            bonusRange2++;

                        }

                    }

                    BreadthFirstSearchAttack(attackIndex, range[type2] + bonusRange2);

                    if(hard1 && hard2)
                    {
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));

                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else if(possibleAttackIndexes.Contains(selectedUnit))
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                            if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                            {
                            
                                PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                                Destroy(units[selectedUnit]);
                                units[selectedUnit] = null;

                            }

                        }

                    }
                    else if(hard1 && !hard2)
                    {
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else if(possibleAttackIndexes.Contains(selectedUnit))
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                            if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                            {
                            
                                PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                                Destroy(units[selectedUnit]);
                                units[selectedUnit] = null;

                            }

                        }

                    }
                    else if(!hard1 && hard2)
                    {
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else if(possibleAttackIndexes.Contains(selectedUnit))
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                            if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                            {
                            
                                PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                                Destroy(units[selectedUnit]);
                                units[selectedUnit] = null;

                            }

                        }

                    }
                    else if(!hard1 && !hard2)
                    {
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack;
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Money") - 5, 0));
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else if(possibleAttackIndexes.Contains(selectedUnit))
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f) + bonusAttack2;
                            if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                            {
                            
                                PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") + 10);

                                Destroy(units[selectedUnit]);
                                units[selectedUnit] = null;

                            }

                        }

                    }

                }
                else if(!enemiesHaveMoved[i])
                {

                    BreadthFirstSearchMove(enemies[i], speed[type] + bonusMovement, type);

                    List<int> choices = new List<int>();

                    foreach(int possibleMoveIndex in possibleMoveIndexes)
                    {

                        for(int j = 0; j < possibleMoveIndex; j++)
                        {

                            choices.Add(possibleMoveIndex);

                        }
                        
                    }

                    if(choices.Count == 0)
                    {

                        continue;

                    }

                    int index = choices[Random.Range(0, choices.Count)];

                    units[index] = units[selectedUnit];
                    
                    GameObject newUnit = Instantiate(units[selectedUnit].GetComponent<UnitManager>().prefab, new Vector3((index % 10) - 4.5f, (index - (index % 10)) / -10.0f + 4.5f, 0.0f), Quaternion.identity);
                    newUnit.transform.localScale = new Vector3(0.8f, 0.8f, 0.0f);
                    if(index.ToString().Length == 1)
                    {
                    
                        newUnit.name = string.Concat("Unit 0", index.ToString());

                    }
                    else
                    {
                    
                        newUnit.name = string.Concat("Unit ", index.ToString());

                    }

                    units[index] = newUnit;
                    newUnit.GetComponent<UnitManager>().moved = true;
                    newUnit.GetComponent<UnitManager>().prefab = units[selectedUnit].GetComponent<UnitManager>().prefab;
                    newUnit.GetComponent<UnitManager>().hitpoints = units[selectedUnit].GetComponent<UnitManager>().hitpoints;
                    newUnit.GetComponent<UnitManager>().i = units[selectedUnit].GetComponent<UnitManager>().i;

                    GameObject unit = units[selectedUnit];
                    Destroy(unit);

                    units[selectedUnit] = null;

                }

            }
            
            possibleMoveIndexes.Clear();
            moveTileColors.Clear();

            possibleAttackIndexes.Clear();
            attackTileColors.Clear();

            selectedUnit = -1;

        }
    
    }

    public void EndGame()
    {

        PlayerPrefs.SetInt(slot.ToString() + "Level", Mathf.Max(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) - 1, 1));

        SceneManager.LoadScene("Headquarters");

    }

}
