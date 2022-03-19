using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManagerScript : MonoBehaviour {
//Instance
    public static SceneManagerScript instance;

//Health
    int health;
    public int maxHealth;
    public TextMeshProUGUI healthText;

//Waves
    public Wave[] waves;
    public Vector2Int[] spawnTiles;
    public TextMeshProUGUI waveText;

    int currentWave = 0;
    int eneimesLeft, eneimesSpawned;

    float spawnTimer;
    float currentTime;

    //Towers
    int maxTowers, towers;
    public TextMeshProUGUI towerText;

    //Grid
    Grid grid;
    public PlayerMovement pm;

    private void Awake() {
        instance = this;
    }

    void Start() {
        instance = this;

    //Health
        health = maxHealth;
        adjustHealth(0);

        //Waves
        eneimesLeft = waves[currentWave].goblins + waves[currentWave].eyes;
        waveText.text = "Wave: 1/" + waves.Length;
        spawnTimer = waves[currentWave].spawnTimer;

        //Towers
        maxTowers = 1;
        addTower(0);
    }

//Health
    public void adjustHealth(int value) {
        health += value;
        healthText.text = "HP: " + health + "/" + maxHealth;

        if(value < 0)
            AudioManager.instance.Play("Hurt");
    }

//Waves
    private void Update() {
        if (currentWave >= waves.Length)
            return;

    //Spawn Eneimes for wave
        currentTime += Time.deltaTime;

        if (currentTime >= spawnTimer && eneimesSpawned < waves[currentWave].goblins + waves[currentWave].eyes) {
            waves[currentWave].spawnEnemy();
            currentTime = 0;
            eneimesSpawned++;
        }

        //Next Wave
        if (eneimesLeft <= 0)  {
            

            currentWave++;

            if (currentWave >= waves.Length)
            {
                Debug.Log("Win 1");
                win();
            }

            waveText.text = "Wave: " + (currentWave + 1) + "/" + waves.Length;
            eneimesLeft = waves[currentWave].goblins + waves[currentWave].eyes;
            eneimesSpawned = 0;
            spawnTimer = waves[currentWave].spawnTimer;
            updateMaxTower();




            //currentWaveTime = waves[currentWave].waveTime;
            //waves[currentWave].spawnEnemies();
        }
    }

    private void win() {
        Debug.Log("Win");
        SceneSwitcher.instance.A_LoadScene(5);
    }

    public Vector2Int randomSpawn() {
        return spawnTiles[Random.Range(0, spawnTiles.Length)];
    }

    public void oneLessEnemy() {
        eneimesLeft--;
    }

//Grid
    public void setGrid()
    {
        grid = pm.getGrid();
        Debug.Log("Set");
    }

    public Grid getGrid() {
        return grid;
    }

//Towers
    public void addTower(int change) {
        towers += change;

        towerText.text = "Towers: " + towers + "/" + maxTowers;
    }

    public void updateMaxTower() {
        maxTowers = currentWave + 1;

        towerText.text = "Towers: " + towers + "/" + maxTowers;
    }

    public bool canAddTower() {
        return (towers < maxTowers);
    }

    public void destroyObj(GameObject obj) {
        Destroy(obj);
    }

}
