using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// CREDITS: CODE MONKEY
public class Grid { 
    public int width, height;
    public float cellSize;
    public Vector3 originPos;

    public GridTile[,] grid;

    GameObject gridTileHolder;

    //Constructors
    public Grid(int width, int height, float cellSize, Vector3 origin, Sprite[] spriteList) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        originPos = origin;

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");

        //Draw Lines
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                 newGridTile(x, y, 0, spriteList, 0, 0);
            }
        }
    }

    public Grid(int width, int height, float cellSize, Vector3 origin) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        originPos = origin;

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");
    }

    public void newGridTile(int x, int y, int value, Sprite[] spriteList, int currentSprite, int rotations) {
        grid[x, y] = new GridTile(value, spriteList, currentSprite, rotations, new Vector3(x + .5f, y + .5f, 0) * cellSize, cellSize, gridTileHolder);
        grid[x, y].CreateSprite(new Vector3(x + .5f, y + .5f, 0) * cellSize, cellSize, originPos);
        grid[x, y].CreateText(new Vector3(x + .5f, y + .5f, 0) * cellSize, cellSize, originPos);
        //grid[x, y].setText(null);

        for (int i = 0; i < rotations; i++) {
            grid[x, y].rotateSpriteTile();
        }
    }

//World Pos <---> Grid Pos
    public Vector3 getWorldPosition(float x, float y) {
        return new Vector3(x, y) * cellSize + originPos;
    }

    public void getGridPosition(Vector3 worldPos, out int x, out int y) {
        x = Mathf.FloorToInt((worldPos - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPos).y / cellSize);
    }

//Text Setter/Getters
    public void textRefresh(int x, int y, string text) {
        if(x < 0 || x > width || y < 0 || y > height) {
            //Debug.LogError("Invalid x or y in Grid");
            return;
        }

        grid[x, y].setText(text);
    }

    public void textRefresh(Vector3 worldPos, string text) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        textRefresh(x, y, text);
    }

    public string getText(int x, int y) {
        if (x < 0 || x > width || y < 0 || y > height) {
            //Debug.LogError("Invalid x or y in Grid");
            return null;
        }

        return grid[x, y].getText();
    }

    public string getText(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        return getText(x, y);
    }

    //Tile Functions
    public void nextTile(int x, int y) {
        if (x < 0 || x > width || y < 0 || y > height) {
            //Debug.LogError("Invalid x or y in Grid");
            return;
        }

        grid[x, y].nextSpriteTile();
    }

    public void nextTile(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        nextTile(x, y);
    }

    public void rotateTile(int x, int y) {
        if (x < 0 || x > width || y < 0 || y > height) {
            //Debug.LogError("Invalid x or y in Grid");
            return;
        }

        grid[x, y].rotateSpriteTile();
    }

    public void rotateTile(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        rotateTile(x, y);
    }

//Value Functions
    public void adjustVal(int x, int y, int offset) {
        if (x < 0 || x > width || y < 0 || y > height) {
            Debug.LogError("Invalid x or y in Grid");
            return;
        }

        grid[x, y].adjustValue(offset);
    }

    public void adjustVal(Vector3 worldPos, int offset) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        adjustVal(x, y, offset);
    }

    public int getVal(int x, int y) {
        if (x < 0 || x > width || y < 0 || y > height)
        {
            //Debug.LogError("Invalid x or y in Grid");
            return -1;
        }

        return grid[x, y].getValue();
    }

    public int getVal(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        return getVal(x, y);
    }

    //Get Adjacent Value
    public Vector2Int findNextPath(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        return findNextPath(x, y);
    }

    public Vector2Int findNextPath(int x, int y) {
        Vector2Int[] tilesToCheck = {new Vector2Int(x - 1, y), new Vector2Int(x + 1, y), new Vector2Int(x, y - 1), new Vector2Int(x, y + 1)};
        List<int> indexs = new List<int>();
        int highestVal = -1;

    //Get List of Highest values
        for (int i = 0; i < tilesToCheck.Length; i++) {
        //Get Tile Value
            int currentVal;

            if (tilesToCheck[i].x < 0 || tilesToCheck[i].x > width-1 || tilesToCheck[i].y < 0 || tilesToCheck[i].y > height-1) {
                currentVal = -1;
                continue;
            }
            else {
                currentVal = grid[tilesToCheck[i].x, tilesToCheck[i].y].getValue();
            }

        //Reset list if new high
            if(highestVal < currentVal) {
                indexs.Clear();

                highestVal = currentVal;
            }

            if (currentVal == highestVal) {
                indexs.Add(i);
            }
        }

        //Debug.Log("Current: (" + x + ", " + y + ")");

    //Determine Square to Move to
        int rng = Random.RandomRange(0, indexs.Count);
        //Debug.Log("RNG: " + rng);
        return tilesToCheck[indexs[rng]];


    }

//
    public GridTile getGridTile(int x, int y) {
        return grid[x, y];
    }

    public GridTile getGridTile(Vector2Int vec)
    {
        return grid[vec.x, vec.y];
    }

    //Output Map Info
    public string classInfo() {
        string info = "";

        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                info += "grid.newGridTile(" + x + " ," + y + ", " + grid[x, y].classInfo() + ");\n";
            }
            info += "\n";
        }
       
        //Debug.Log("Before Output");
        return info;
    }    
}
