using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class GridTile {
    GameObject tileHolder;

    GameObject textObject;
    TextMeshPro text;
    public int value;

    GameObject tileObject;
    Sprite[] spriteList;
    public int currentSprite = 0;
    public int rotations = 0;

    GameObject tower;
    bool hasTower;

    public GridTile(Sprite[] spriteList, Vector3 pos, float cellSize, GameObject parent) {
        this.spriteList = spriteList;

        tileHolder = new GameObject("(" + (int)(pos.x / cellSize) + ", " + (int)(pos.y / cellSize) + ")");
        tileHolder.transform.SetParent(parent.transform, true);
    }

    public GridTile(int value, Sprite[] spriteList, int currentSprite, int rotations, Vector3 pos, float cellSize, GameObject parent) {
        this.value = value;
        this.spriteList = spriteList;
        this.currentSprite = currentSprite;
        this.rotations = rotations;

        tileHolder = new GameObject("(" + (int)(pos.x / cellSize) + ", " + (int)(pos.y / cellSize) + ")");
        tileHolder.transform.SetParent(parent.transform, true);
    }

//Sprite Functions
    public void CreateSprite(Vector3 pos, float cellSize, Vector3 origin) {
        tileObject = new GameObject("Sprite");
        tileObject.transform.position = pos + origin;
        tileObject.transform.localScale = tileObject.transform.localScale * cellSize;
        tileObject.transform.SetParent(tileHolder.transform, true);

        tileObject.AddComponent<SpriteRenderer>();
        tileObject.GetComponent<SpriteRenderer>().sprite = spriteList[currentSprite];
    }

    public void nextSpriteTile() {
        currentSprite++;

        if (currentSprite >= spriteList.Length) {
            currentSprite = 0;
        }

        tileObject.GetComponent<SpriteRenderer>().sprite = spriteList[currentSprite];
    }

    public void rotateSpriteTile() {
        Quaternion r = tileObject.transform.rotation;
        r *= Quaternion.Euler(0, 0, 90);
        tileObject.transform.rotation = r;

        rotations++;
    }

    public int getSpriteIndex() {
        return currentSprite;
    }

//Text Functions
    public void CreateText(Vector3 pos, float cellSize, Vector3 origin) {
        textObject = new GameObject("Text");
        textObject.transform.position = pos + origin;
        textObject.transform.SetParent(tileHolder.transform, true);

        textObject.AddComponent<TextMeshPro>();
        text = textObject.GetComponent<TextMeshPro>();
        text.text = "";
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 36;
        text.transform.localScale = new Vector3(0.2f * cellSize, 0.2f * cellSize, 0);
    }

    public void setText(string text) {
        if(text == null) {
            setText();
            return;
        }

        tileObject.GetComponent<TextMeshPro>().text = text;
    }

    public string getText() {
        return text.text;
    }

 
//Value
    public void adjustValue(int temp) {
        value += temp;
    }

    public int getValue() {
        return value;
    }

    public void setText() {
        text.text = value.ToString();
    }

    //Has Tower
    public void setTower(GameObject temp) {
        tower = temp;
        tower.transform.SetParent(tileHolder.transform, true);
        hasTower = true;
    }

    public void removeTower() {
        SceneManagerScript.instance.destroyObj(tower);
        tower = null;
        hasTower = false;
    }

    public bool getTower() {
        return hasTower;
    }

//Class Stuff
    public string classInfo() {
        string info = value + ", spriteList, " + currentSprite + ", " + rotations;

        return info;
    }
}
