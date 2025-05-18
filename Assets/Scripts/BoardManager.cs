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

    void Start()
    {
        
        slot = PlayerPrefs.GetInt("Slot");

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

            setupString = "5447310000";

        }
        else if(PlayerPrefs.GetInt(slot.ToString() + "Level", 1) == 10)
        {

            setupString = "3000117445";

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
        
        string tag = units[index].tag;
        if(tag == "FI" || tag == "EI")
        {

            type = 0;

        }
        else if(tag == "FS" || tag == "ES")
        {

            type = 1;

        }
        else if(tag == "FC" || tag == "EC")
        {

            type = 2;

        }
        else if(tag == "FM" || tag == "EM")
        {

            type = 3;

        }
        else if(tag == "FL" || tag == "EL")
        {

            type = 4;

        }
        else if(tag == "FH" || tag == "EH")
        {

            type = 5;

        }
        else if(tag == "FA" || tag == "EA")
        {

            type = 6;

        }
        else if(tag == "FR" || tag == "ER")
        {

            type = 7;

        }
        
        if(selectedUnit == -1 && !units[index].GetComponent<UnitManager>().moved && (tag == "FI" || tag == "FS" || tag == "FC" || tag == "FM" || tag == "FL" || tag == "FH" || tag == "FA" || tag == "FR"))
        {

            BreadthFirstSearchMove(index, speed[type], type);
            BreadthFirstSearchAttack(index, range[type]);

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

            string tag1 = units[selectedUnit].tag;
            if(tag1 == "FI" || tag1 == "EI")
            {

                type1 = 0;
                hard1 = false;

            }
            else if(tag1 == "FS" || tag1 == "ES")
            {

                type1 = 1;
                hard1 = false;

            }
            else if(tag1 == "FC" || tag1 == "EC")
            {
                
                type1 = 2;
                hard1 = false;

            }
            else if(tag1 == "FM" || tag1 == "EM")
            {
                
                type1 = 3;
                hard1 = false;

            }
            else if(tag1 == "FL" || tag1 == "EL")
            {
                
                type1 = 4;
                hard1 = true;

            }
            else if(tag1 == "FH" || tag1 == "EH")
            {
                
                type1 = 5;
                hard1 = true;

            }
            else if(tag1 == "FA" || tag1 == "EA")
            {
                
                type1 = 6;
                hard1 = false;

            }
            else if(tag1 == "FR" || tag1 == "ER")
            {
                
                type1 = 7;
                hard1 = false;

            }

            string tag2 = units[index].tag;
            if(tag2 == "FI" || tag2 == "EI")
            {

                type2 = 0;
                hard2 = false;

            }
            else if(tag2 == "FS" || tag2 == "ES")
            {

                type2 = 1;
                hard2 = false;

            }
            else if(tag2 == "FC" || tag2 == "EC")
            {

                type2 = 2;
                hard2 = false;

            }
            else if(tag2 == "FM" || tag2 == "EM")
            {

                type2 = 3;
                hard2 = false;

            }
            else if(tag2 == "FL" || tag2 == "EL")
            {

                type2 = 4;
                hard2 = true;

            }
            else if(tag2 == "FH" || tag2 == "EH")
            {

                type2 = 5;
                hard2 = true;

            }
            else if(tag2 == "FA" || tag2 == "EA")
            {

                type2 = 6;
                hard2 = false;

            }
            else if(tag2 == "FR" || tag2 == "ER")
            {

                type2 = 7;
                hard2 = false;

            }

            List<int> copyOfPossibleAttackIndexes = new List<int>();
            foreach(int possibleAttackIndex in possibleAttackIndexes)
            {

                copyOfPossibleAttackIndexes.Add(possibleAttackIndex);

            }
            possibleAttackIndexes.Clear();
            BreadthFirstSearchAttack(index, range[type2]);

            if(hard1 && hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
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

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f);
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(hard1 && !hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
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

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f);
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(!hard1 && hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
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

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f);
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);

                        }

                        Destroy(units[selectedUnit]);
                        units[selectedUnit] = null;

                    }

                }

            }
            else if(!hard1 && !hard2)
            {

                units[index].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
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

                    units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[index].GetComponent<UnitManager>().hitpoints / 10.0f);
                    if(units[selectedUnit].GetComponent<UnitManager>().hitpoints < 1)
                    {

                        if(tag1 == "FI" || tag1 == "FS" || tag1 == "FC" || tag1 == "FM" || tag1 == "FL" || tag1 == "FH" || tag1 == "FA" || tag1 == "FR")
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);

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

                }

            }

        }

        if(friendlies == 0)
        {

            EndGame();

        }
        else if(enemies.Count == 0)
        {
            
            PlayerPrefs.SetInt(slot.ToString() + "Level", PlayerPrefs.GetInt(slot.ToString() + "Level", 1) + 1);

            EndGame();

        }
        else
        {

            for(int i = enemies.Count - 1; i >= 0; i--)
            {
                
                selectedUnit = enemies[i];

                string tag = units[selectedUnit].tag;
                if(tag == "FI" || tag == "EI")
                {

                    type = 0;

                }
                else if(tag == "FS" || tag == "ES")
                {

                    type = 1;

                }
                else if(tag == "FC" || tag == "EC")
                {

                    type = 2;

                }
                else if(tag == "FM" || tag == "EM")
                {

                    type = 3;

                }
                else if(tag == "FL" || tag == "EL")
                {

                    type = 4;

                }
                else if(tag == "FH" || tag == "EH")
                {

                    type = 5;

                }
                else if(tag == "FA" || tag == "EA")
                {

                    type = 6;

                }
                else if(tag == "FR" || tag == "ER")
                {

                    type = 7;

                }

                possibleMoveIndexes.Clear();
                possibleAttackIndexes.Clear();
                BreadthFirstSearchAttack(selectedUnit, range[type]);

                if(possibleAttackIndexes.Count > 0)
                {

                    units[selectedUnit].GetComponent<UnitManager>().moved = true;
            
                    int type1 = 0;
                    int type2 = 0;
                    bool hard1 = false;
                    bool hard2 = false;

                    string tag1 = units[selectedUnit].tag;
                    if(tag1 == "FI" || tag1 == "EI")
                    {
                    
                        type1 = 0;
                        hard1 = false;

                    }
                    else if(tag1 == "FS" || tag1 == "ES")
                    {
                    
                        type1 = 1;
                        hard1 = false;

                    }
                    else if(tag1 == "FC" || tag1 == "EC")
                    {

                        type1 = 2;
                        hard1 = false;

                    }
                    else if(tag1 == "FM" || tag1 == "EM")
                    {

                        type1 = 3;
                        hard1 = false;

                    }
                    else if(tag1 == "FL" || tag1 == "EL")
                    {

                        type1 = 4;
                        hard1 = true;

                    }
                    else if(tag1 == "FH" || tag1 == "EH")
                    {

                        type1 = 5;
                        hard1 = true;

                    }
                    else if(tag1 == "FA" || tag1 == "EA")
                    {

                        type1 = 6;
                        hard1 = false;

                    }
                    else if(tag1 == "FR" || tag1 == "ER")
                    {

                        type1 = 7;
                        hard1 = false;

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

                    string tag2 = units[attackIndex].tag;
                    if(tag2 == "FI" || tag2 == "EI")
                    {
                    
                        type2 = 0;
                        hard2 = false;

                    }
                    else if(tag2 == "FS" || tag2 == "ES")
                    {
                    
                        type2 = 1;
                        hard2 = false;

                    }
                    else if(tag2 == "FC" || tag2 == "EC")
                    {
                    
                        type2 = 2;
                        hard2 = false;

                    }
                    else if(tag2 == "FM" || tag2 == "EM")
                    {
                    
                        type2 = 3;
                        hard2 = false;

                    }
                    else if(tag2 == "FL" || tag2 == "EL")
                    {
                    
                        type2 = 4;
                        hard2 = true;

                    }
                    else if(tag2 == "FH" || tag2 == "EH")
                    {
                    
                        type2 = 5;
                        hard2 = true;

                    }
                    else if(tag2 == "FA" || tag2 == "EA")
                    {
                    
                        type2 = 6;
                        hard2 = false;

                    }
                    else if(tag2 == "FR" || tag2 == "ER")
                    {
                    
                        type2 = 7;
                        hard2 = false;

                    }

                    if(hard1 && hard2)
                    {
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);

                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f);
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
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f);
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
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(hardAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f);
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
                    
                        units[attackIndex].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softAttack[type1] * units[selectedUnit].GetComponent<UnitManager>().hitpoints / 10.0f);
                        if(units[attackIndex].GetComponent<UnitManager>().hitpoints < 1)
                        {

                            PlayerPrefs.SetInt(slot.ToString() + "Money", PlayerPrefs.GetInt(slot.ToString() + "Money") - 5);
                            
                            Destroy(units[attackIndex]);
                            units[attackIndex] = null;

                        }
                        else
                        {
                        
                            units[selectedUnit].GetComponent<UnitManager>().hitpoints -= Mathf.CeilToInt(softDefense[type2] * units[attackIndex].GetComponent<UnitManager>().hitpoints / 10.0f);
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

                    BreadthFirstSearchMove(enemies[i], speed[type], type);

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

        SceneManager.LoadScene("Headquarters");

    }

}
