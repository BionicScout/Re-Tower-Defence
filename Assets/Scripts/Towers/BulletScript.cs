using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    Camera cam;
    Transform target;
    Vector3 startPoint, endLoc;
    public float speed = 5f;

    float range;

    public float damage = 1;

    float m, b;

    public void setTarget(Transform target) {
        cam = Camera.main;
        this.target = target;
        findEndLocation();
        AudioManager.instance.Play("Laser");

        //Debug.Log("Current: " + transform.position + "\nTarget: " + target.position + "\nEnd Location: " + endLoc);
    }

    public void setRange(float range, Vector3 vec) {
        this.range = range;
        startPoint = vec;
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, endLoc, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, startPoint) > range) {
            destroy();
        }
    }

    void findEndLocation()
    { //Find y in y = mx+b, where n is a location of screen
      //Needed Info
        Vector3 playerPos = target.position;
        float screenHeight = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y;
        float D; //Direction Facing
        float n; //Target x
        float y; //Target y

        //Get point x and y
        if (transform.position.x - playerPos.x == 0)
        { //If Vertical Line
            Vector3 normalized = transform.position - target.position;

            if (normalized.y < 0)
                y = screenHeight * 2;
            else
                y = -screenHeight;

            //Get Line Components
            n = transform.position.x;

            //Debug.Log("Vertical Bullet");
        }
        else
        {
            Vector3 normalized = transform.position - target.position;

            //Get Line Components
            m = (transform.position.y - playerPos.y) / (transform.position.x - playerPos.x);
            b = transform.position.y - (m * transform.position.x);

            //Get Vector
            if (normalized.y < 0)
            { //Up
                if (normalized.x < 0)
                { //Up Right
                    n = screenHeight * 2;
                    y = (m * screenHeight * 2) + b;
                }
                else
                { //Up Left
                    y = screenHeight * 2;
                    n = (y - b) / m;
                }
            }
            else
            { //Down
                if (normalized.x < 0)
                { //Down Right
                    y = -screenHeight;
                    n = (y - b) / m;
                }
                else
                { //Down Left
                    n = -screenHeight * 2;
                    y = (m * -screenHeight * 2) + b;
                }
            }
        }


        //Debug.Log("x: " + n + "\ty: " + y + "\nSlope: " + m + "\tHeight: " + b);
        endLoc = new Vector3(n, y, transform.position.z);

    //Rotate to Face Point
        Quaternion rotateDir = Quaternion.LookRotation(Vector3.forward, -endLoc);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDir, 20000);
    }

    void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    public float getDamage() {
        return damage;
    }

    public void destroy() {
        Destroy(this.gameObject);
    }
        

}