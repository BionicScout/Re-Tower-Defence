using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    public float spawnTimer;
    public int goblins, eyes;
    int goblinsSpawned, eyesSpawned;

    public GameObject goblinPrefab, eyePrefab;

    public void spawnEnemy()
    {
        GameObject temp;
        Grid grid = SceneManagerScript.instance.getGrid();

        //Spawn 
            Vector2Int vec = SceneManagerScript.instance.randomSpawn();

        int rng = Random.Range(1, 3);
        Debug.Log(rng);

        if (goblinsSpawned < goblins && rng == 1 || (eyesSpawned >= eyes)) {
            temp = GameObject.Instantiate(goblinPrefab);
            goblinsSpawned++;
        }
        else
        {
            temp = GameObject.Instantiate(eyePrefab);
            eyesSpawned++;
        }




            temp.GetComponent<EnemyMovement>().setGrid(grid);
            temp.GetComponent<EnemyMovement>().setStartPos(vec.x, vec.y);
    }
}