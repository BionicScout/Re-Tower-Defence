using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    Grid grid;
    Vector2Int currentTile, nextTile;
    Vector3 nextPos;

    public int speed;
    public int damage;

    float health;
    public int maxHealth;

    private void Start() {
        EnemyList.enemies.Add(this.gameObject);
        health = maxHealth;
    }
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
       
        if(Vector3.Distance(transform.position, nextPos) == 0) {
            if (grid.getVal(nextPos) == 100) {
                SceneManagerScript.instance.adjustHealth(-damage);
                kill();
            }

            currentTile = nextTile;
            nextTile = grid.findNextPath(transform.position);
            nextPos = grid.getWorldPosition(nextTile.x + .5f, nextTile.y + .75f) + new Vector3(0, 0, transform.position.z);
        }
    }

    public void adjustHealth(float value) {
        health += value;
        Debug.Log("Health: " + health);
        if (health <= 0) {
            kill();
        }
    }

    public void kill() {
        SceneManagerScript.instance.oneLessEnemy();
        EnemyList.enemies.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void setStartPos(int x, int y) {
        Vector3 vec = grid.getWorldPosition(x + .5f, y + .75f);
        transform.position = new Vector3(vec.x, vec.y, transform.position.z);

        currentTile = new Vector2Int(x, y);
        nextTile = grid.findNextPath(vec);
        nextPos = grid.getWorldPosition(nextTile.x + .5f, nextTile.y + .75f) + new Vector3(0, 0, transform.position.z);

        //Debug.Log("Current: " + currentTile + "\nNext: " + nextTile);
    }

    public void setGrid(Grid grid) {
        this.grid = grid;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            adjustHealth(-bullet.getDamage());
            bullet.destroy();
        }
    }
}
