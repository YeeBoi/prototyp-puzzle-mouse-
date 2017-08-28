using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    private Vector2 position;
    private GameObject button;
    private GameObject activation;
    private char color;
    private bool activate = false;

	public Button() {
        position = new Vector2();
        color = '\0';
    }

    public Button(Vector2 position, char color) : this() {
        this.position = position;
        this.color = color;
        getColorObject();
    }

    private void getColorObject () {
        if (color == 'B') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("blue-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'R') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("red-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'Y') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("yellow-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'P') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("purple-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'O') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("orange-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'G') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("green-B"), position, Quaternion.Euler(0, 0, 0));
        } else if (color == 'D') {
            button = (GameObject)Instantiate((GameObject)Resources.Load("brown-B"), position, Quaternion.Euler(0, 0, 0));
        }
        activation = (GameObject)Instantiate((GameObject)Resources.Load("black-B"), new Vector3(position.x,position.y,1), Quaternion.Euler(0, 0, 0));
    }

    public void activateButton(char c) {
        if (colorValid(c) && !activate || !colorValid(c) && activate) {
            activate = !activate;
            Destroy(activation);
            if (activate) {
                activation = (GameObject)Instantiate((GameObject)Resources.Load("white-B"), new Vector3(position.x, position.y, 1), Quaternion.Euler(0, 0, 0));
            } else {
                activation = (GameObject)Instantiate((GameObject)Resources.Load("black-B"), new Vector3(position.x, position.y, 1), Quaternion.Euler(0, 0, 0));
            }
        }
    }

    private bool colorValid(char c) {
        bool colorValid = false;
        if ((c == 'B') && color == 'B') {
            colorValid = true;
        } else if ((c == 'Y') && color == 'Y') {
            colorValid = true;
        } else if ((c == 'R') && color == 'R') {
            colorValid = true;
        } else if ((c == 'O') && color == 'O') {
            colorValid = true;
        } else if ((c == 'G') && color == 'G') {
            colorValid = true;
        } else if ((c == 'P') && color == 'P') {
            colorValid = true;
        } else if ((c == 'D') && color == 'D') {
            colorValid = true;
        }
        return colorValid;
    }

    public bool getActivate() {
        return activate;
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
