using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    Grid grid;
    GameObject currentTarget = null;
    public float range = 5;

    public GameObject bulletPrefab;
    public float attackTime;
    float timer;

    public int i;

    private void Update() {
        if (currentTarget == null) {
            findTarget();
        }

        if (currentTarget != null) {
            Debug.DrawLine(transform.position, currentTarget.transform.position, Color.red, Time.deltaTime);

            timer += Time.deltaTime;

            if (timer >= attackTime) {
                timer = 0;
                attack();
            }

            if (Vector3.Distance(currentTarget.transform.position, transform.position) > range)
            {
                currentTarget = null;
            }
        }


    }

    public void findTarget() {
        if (EnemyList.enemies.Count == 0) {
            currentTarget = null;
            return;
        }

        int closestIndex = -1;
        float closest = Mathf.Infinity;

        for (int i = 0; i < EnemyList.enemies.Count; i++) {
            float distance = Vector2.Distance(transform.position, EnemyList.enemies[i].transform.position);

            if(distance < closest && distance <= range) {
                closest = distance;
                closestIndex = i;
            }
        }

        if (closestIndex != -1) {
            currentTarget = EnemyList.enemies[closestIndex];
        }
        else
            currentTarget = null;

    }

    public void attack() {
        GameObject temp = Instantiate(bulletPrefab);
        temp.transform.position = new Vector3(transform.position.x, transform.position.y, temp.transform.position.z);

        if (i == 1) {
            BulletScript tempScript = temp.GetComponent<BulletScript>();
            tempScript.setTarget(currentTarget.transform);
            tempScript.setRange(range, transform.position);
        }
        if (i == 2)
        {
            BombScript tempScript = temp.GetComponent<BombScript>();
            tempScript.setTarget(currentTarget.transform.position);
            tempScript.setRange(range, transform.position);
            Debug.Log("Here");
        }
    }

    public void setGrid(Grid grid) {
        this.grid = grid;
    }

    public void setStartPos(int x, int y) {
        Vector3 vec = grid.getWorldPosition(x + .5f, y + .25f);
        transform.position = new Vector3(vec.x, vec.y, transform.position.z);
    }
}
