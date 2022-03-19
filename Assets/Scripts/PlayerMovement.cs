using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {
    public int height, width;
    public float size;

    public Vector3 origin;

    public Camera cam;
    Grid grid;

    public Sprite[] spriteList = new Sprite[4];

    public GameObject goblinPrefab, eyePrefab;

    public GameObject purpleTowerPrefab, yellowTowerPrefab;

    bool paused;
    public GameObject menu;


    private void Awake() {
        grid = new Grid(width, height, size, origin);
        MapTemplete temp = new MapTemplete(spriteList, grid);
        menu.SetActive(false);
    }

    private void Start() {
        SceneManagerScript.instance.setGrid();
    }

    private void Update() {

        if ((Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)) && grid.getGridTile(getGridPos()).getTower()) {
            removeTower();
        }
        else if (Input.GetMouseButtonDown(0) && SceneManagerScript.instance.canAddTower()) {
            placeTower(purpleTowerPrefab);
        }
        else if (Input.GetMouseButtonDown(1) && SceneManagerScript.instance.canAddTower())
        {
            placeTower(yellowTowerPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
            {
                Time.timeScale = 0;
                menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                menu.SetActive(true);
            }
        }
    }

    public void placeTower(GameObject tower)
    {
        Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);
        vec = new Vector3(vec.x, vec.y, 0f);

        int x, y;
        grid.getGridPosition(vec, out x, out y);

        if (grid.getGridTile(x, y).getTower()) {
            Debug.LogError("TILE HAS A TOWER");
            return;
        }

        if (grid.grid[x, y].getSpriteIndex() == 0)
        {
            GameObject temp = Instantiate(tower);
            temp.GetComponent<Tower>().setGrid(grid);
            temp.GetComponent<Tower>().setStartPos(x, y);
            grid.getGridTile(x, y).setTower(temp);

            SceneManagerScript.instance.addTower(1);
        }
        else
            Debug.LogError("Error: Can't put tower on path");

    }

    public void removeTower() {
        Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);
        vec = new Vector3(vec.x, vec.y, 0f);

        int x, y;
        grid.getGridPosition(vec, out x, out y);

        if (!grid.getGridTile(x, y).getTower()) {
            Debug.LogError("TILE DOES NOT HAVE A TOWER");
            return;
        }

            
            grid.getGridTile(x, y).removeTower();

        SceneManagerScript.instance.addTower(-1);



    }

    public Grid getGrid() {
        return grid;
    }

    public Vector2Int getGridPos() {
        Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);
        vec = new Vector3(vec.x, vec.y, 0f);

        int x, y;
        grid.getGridPosition(vec, out x, out y);
        return new Vector2Int(x, y);
    }
}
