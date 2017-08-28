using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    private char[,] level;
    private Graph graphLevel;
    private List<Button> buttonList;
    private GameObject door;
    private List<Wall> listColorWall = new List<Wall>();


    public Level() {
        level = null;
        graphLevel = null;
        buttonList = new List<Button>();
        listColorWall = new List<Wall>();
    }

    public Level(char[,] level, List<Button> buttonList, List<Wall> listColorWall) : this() {
        this.level = level;
        graphLevel = new Graph(level);
        this.buttonList = buttonList;
        this.listColorWall = listColorWall;
        initiateLevel();
    }

    public void initiateLevel() {
        for (int i = 0; i < level.Length/level.GetLength(0); i++) {
            for (int j = 0; j < level.GetLength(0); j++) {
                if (level[i,j] == 'W') {
                    if (!listContain(new Vector2(i,j))) {
                        Wall wa = new Wall(new Vector2(i, j));
                    }
                } 
                if (level[i,j] == 'V') {
                    door = Instantiate((GameObject)Resources.Load("door"), new Vector2(i, j), Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }

    public bool listContain(Vector2 pos) {
        bool contains = false;
        for (int i = 0; i < listColorWall.Count; i++) {
            if (listColorWall[i].getPosition() == pos) {
                contains = true;
            }
        }
        return contains;
    }

    public Wall wallAt(Vector2 pos) {
        Wall wall = null;
        for (int i = 0; i < listColorWall.Count; i++) {
            if (listColorWall[i].getPosition() == pos) {
                wall = listColorWall[i];
            }
        }
        return wall;
    }

    public void checkButton() {
        bool finish = true;
        int compteur = 0;
        for (int i = 0; i < buttonList.Count; i++) {
            buttonList[i].activateButton(graphLevel.getGraph()[(int)buttonList[i].getPosition().x, (int)buttonList[i].getPosition().y].getType());
        }
        while (finish && compteur < buttonList.Count) {
            finish = buttonList[compteur].getActivate();
            compteur++;
        }
        if (finish) {
            Destroy(door);
        }
    }

    public Graph getGraphLevel() {
        return graphLevel;
    }

    public List<Button> getButtonList() {
        return buttonList;
    }

    public List<Wall> getListColorWall() {
        return listColorWall;
    }

    public GameObject getDoor() {
        return door;
    }
}
