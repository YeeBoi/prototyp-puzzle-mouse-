using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private bool connection = false;
    private char color;
    private Vector2 direction;
    private Vector2 position;
    private GameObject wall;
    private GameObject wallObject;

    public Wall() {
        direction = new Vector2();
        position = new Vector2();
    }

    public Wall(Vector2 position) : this() {
        this.position = position;
        LoadWall();
    }

    public Wall(bool connection, char color, Vector2 direction, Vector2 position) : this() {
        this.connection = connection;
        if (connection) {
            this.color = color;
            this.direction = direction;
        }
        this.position = position;
        LoadWall();
    }

    private void LoadWall() {
        if (connection) {
            if (color == 'B') {
                wall = (GameObject)Resources.Load("Wall-B");
            } else if (color == 'Y') {
                wall = (GameObject)Resources.Load("Wall-Y");
            } else {
                wall = (GameObject)Resources.Load("Wall-R");
            } 
            if (direction.x == 1) {
                wallObject = Instantiate(wall, position, Quaternion.Euler(0, 0, 0));
            } else if (direction.x == -1) {
                wallObject = Instantiate(wall, position, Quaternion.Euler(0, 0, 180));
            } else if (direction.y == 1) {
                wallObject = Instantiate(wall, position, Quaternion.Euler(0, 0, 90));
            } else if (direction.y == -1) {
                wallObject = Instantiate(wall, position, Quaternion.Euler(0, 0, 270));
            }

        } else {
            wallObject = Instantiate((GameObject)Resources.Load("Wall"), position, Quaternion.Euler(0, 0, 0));
        }
    }

    public Vector2 getPosition() {
        return position;
    }

    public void setPosition(Vector2 position) {
        this.position = position;
    }

    public char getColor() {
        return color;
    }
}
