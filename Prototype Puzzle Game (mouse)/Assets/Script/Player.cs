using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    private Graph graph;
    private Vector3 position = new Vector3(1, 1, -2);
    private Vector2 faceDir = new Vector2(0, 0);
    private float speed = 5f;
    private Vector2 input;
    private static bool wire = false;
    private List<Wire> allWire = new List<Wire>();
    private Wire aWire = null;
    private static List<Vector2> notPos = new List<Vector2>();
    private bool moving = false;
    private bool priority = false;

    public Camera camera;
    void Start() {
        graph = LevelGenerator.currentLevel.getGraphLevel();
        camera = FindObjectOfType<Camera>();
        this.gameObject.transform.position = position;
    }

    void Update() {
        print(Math.Round(camera.ScreenToWorldPoint(Input.mousePosition).x));
        //print(Input.mousePosition);

        inputManager();
        activateColor();
        if (Input.GetButtonDown("wireDestroy")) {
            if (wire) {
                wire = false;
                aWire.deleteWire();
            }
        }
        if (Input.GetButtonDown("branch")) {
            if ((((graph.getGraph())[(int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y].getType() == 'W' && wire 
                && (!LevelGenerator.currentLevel.listContain(new Vector2((int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y)) || new Vector2(position.x, position.y) == aWire.getPosition()[0])) ||
                ((graph.getGraph())[(int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y].getType() == 'W' && !wire &&
                LevelGenerator.currentLevel.listContain(new Vector2((int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y)))) && !notPos.Contains(position)) {
                wire = !wire;
                if (wire) {
                    aWire = new Wire(position, -faceDir, LevelGenerator.currentLevel.wallAt(new Vector2((int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y)).getColor());
                    graph.findShortestPath(graph.getGraph()[(int)position.x, (int)position.y]);
                } else {
                    if (new Vector2(position.x, position.y) != aWire.getPosition()[0]) {
                        graph.drawWire(aWire);
                        notPos.Add(position);
                        notPos.Add(aWire.getPosition()[0]);
                        graph.reinitialiserDistance();
                        aWire.deleteZoneColorOnWire();
                        aWire.setWireDirection(faceDir);
                        reloadAllWire();
                        allWire.Add(aWire);
                    } else {
                        aWire.deleteWire();
                    }
                }
            } else if ((graph.getGraph())[(int)position.x + (int)faceDir.x, (int)position.y + (int)faceDir.y].getType() == 'W' && !wire) {
                for (int i = 0; i < allWire.Count; i++) {
                    if (new Vector2(position.x, position.y) == allWire[i].getPosition()[0]) {
                        notPos.Remove(allWire[i].getPosition()[0]);
                        notPos.Remove(allWire[i].getPosition()[allWire[i].getPosition().Count - 1]);
                        allWire[i].deleteWire();
                        allWire.RemoveAt(i);
                        reloadAllWire();
                        LevelGenerator.currentLevel.checkButton();
                    } else if (new Vector2(position.x, position.y) == allWire[i].getPosition()[allWire[i].getPosition().Count - 1]) {
                        notPos.Remove(allWire[i].getPosition()[0]);
                        notPos.Remove(allWire[i].getPosition()[allWire[i].getPosition().Count - 1]);
                        allWire[i].removeOnGraph();
                        allWire[i].deleteZoneColor();
                        allWire[i].setIsActive(false);
                        wire = true;
                        aWire = allWire[i];
                        graph.findShortestPath(graph.getGraph()[(int)allWire[i].getPosition()[0].x, (int)allWire[i].getPosition()[0].y]);
                        allWire.RemoveAt(i);
                        reloadAllWire();
                        LevelGenerator.currentLevel.checkButton();
                    }
                }
            }
        } 
        move();
    }

    private void inputManager() {
        input = new Vector2(Input.GetAxis("x"), Input.GetAxis("y"));
        if (priority) {
            Xpriority();
        } else {
            Ypriority();
        }

        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, position, Time.deltaTime * speed);
        if (this.gameObject.transform.position.x - position.x == 0 && this.gameObject.transform.position.y - position.y == 0) {
            moving = false;
        }
    }

    private void Xpriority() {
        if ((input.x > 0.5 || Input.GetButton("right")) && !(input.y == -1 || input.y == 1)) {
            input.x = 1;
        } else if ((input.x < -0.5 || Input.GetButton("left")) && !(input.y == -1 || input.y == 1)) {
            input.x = -1;
        } else {
            input.x = 0;
        }
        if (input.y != 0) {
            priority = !priority;
        }
        if ((input.y > 0.5 || Input.GetButton("down")) && !(input.x == -1 || input.x == 1) ) {
            input.y = 1;
        } else if ((input.y < -0.5 || Input.GetButton("up")) && !(input.x == 1 || input.x == -1)) {
            input.y = -1;
        } else {
            input.y = 0;
        }
    }

    private void Ypriority() {
        if ((input.y > 0.5 || Input.GetButton("down")) && !(input.x == -1 || input.x == 1)) {
            input.y = 1;
        } else if ((input.y < -0.5 || Input.GetButton("up")) && !(input.x == -1 || input.x == 1)) {
            input.y = -1;
        } else {
            input.y = 0;
        }
        if (input.x != 0) {
            priority = !priority;
        }
        if ((input.x > 0.5 || Input.GetButton("right")) && !(input.y == -1 || input.y == 1)) {
            input.x = 1;
        } else if ((input.x < -0.5 || Input.GetButton("left")) && !(input.y == 1 || input.y == -1)) {
            input.x = -1;
        } else {
            input.x = 0;
        }
    }

    private int displayRotation() {
        int rot = 0;
        if (faceDir.x == 1) {
            rot = 270;
        } else if (faceDir.x == -1) {
            rot = 90;
        } else if (faceDir.y == -1) {
            rot = 180;
        }
        return rot;
    }

    private void reloadAllWire() {
        for (int i = 0; i < allWire.Count; i++) {
            graph.drawWire(allWire[i]);
            allWire[i].deleteZoneColor();
        }
        for (int i = 0; i < allWire.Count; i++) {
            if (allWire[i].getIsActive()) {
                allWire[i].switchActivate();
                allWire[i].colorSide();
            }
            allWire[i].deleteZoneColorOnWire();
        }
    }

    void resetWireOfColorN(char color) {
        for (int i = 0; i < allWire.Count; i++) {
            if (allWire[i].colorChar() == color && allWire[i].getZoneColor().Count > 0) {
                allWire[i].switchActivate();
                allWire[i].colorSide();
            }
        }
    }

    bool isWire() {
        bool isWire = false;
        for (int i = 0; i < allWire.Count; i++) {
            if (position.x == allWire[i].getWirePosition()[0].x && position.y == allWire[i].getWirePosition()[0].y ||
                position.x == allWire[i].getWirePosition()[allWire[i].getWirePosition().Count - 1].x && 
                position.y == allWire[i].getWirePosition()[allWire[i].getWirePosition().Count - 1].y) {
                isWire = true;
                aWire = allWire[i];
            }
        }
        return isWire;
    }

    void activateColor() {
        if (Input.GetButtonDown("color") && !wire) {
            for (int i = 0; i < allWire.Count; i++) {
                if (position.x == allWire[i].getWirePosition()[allWire[i].getWirePosition().Count - 1].x &&
                    position.y == allWire[i].getWirePosition()[allWire[i].getWirePosition().Count - 1].y ||
                    position.x == allWire[i].getWirePosition()[0].x && position.y == allWire[i].getWirePosition()[0].y) {
                    allWire[i].colorSide();
                    allWire[i].setIsActive(true);
                }
            }
        }
    }
    
    void move() {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, displayRotation());
        try {
            if (!moving) {
                if (input.x == 1) {
                    faceDir.x = 1;
                    faceDir.y = 0;
                    if (validZone((graph.getGraph())[(int)position.x + 1, (int)position.y].getType())) {
                        position.x++;
                        moving = true;
                        if (wire) {
                            if (graph.getGraph()[(int)position.x, (int)position.y].getDistance() < aWire.getLenght() - 1) {
                                aWire.shortenTheWire(graph.getGraph()[(int)position.x, (int)position.y]);
                            } else {
                                aWire.lengthenWire(position, faceDir);
                            }
                        }
                    }
                }
                if (input.x == -1) {
                    faceDir.x = -1;
                    faceDir.y = 0;
                    if (validZone((graph.getGraph())[(int)position.x - 1, (int)position.y].getType())) {
                        position.x--;
                        moving = true;
                        if (wire) {
                            if (graph.getGraph()[(int)position.x, (int)position.y].getDistance() < aWire.getLenght() - 1) {
                                aWire.shortenTheWire(graph.getGraph()[(int)position.x, (int)position.y]);
                            } else {
                                aWire.lengthenWire(position, faceDir);
                            }
                        }
                    }
                }
                if (input.y == -1) {
                    faceDir.y = 1;
                    faceDir.x = 0;
                    if (validZone((graph.getGraph())[(int)position.x, (int)position.y + 1].getType())) {
                        position.y++;
                        moving = true;
                        if (wire) {
                            if (graph.getGraph()[(int)position.x, (int)position.y].getDistance() < aWire.getLenght() - 1) {
                                aWire.shortenTheWire(graph.getGraph()[(int)position.x, (int)position.y]);
                            } else {
                                aWire.lengthenWire(position, faceDir);
                            }
                        }
                    }
                }
                if (input.y == 1) {
                    faceDir.y = -1;
                    faceDir.x = 0;
                    if (validZone((graph.getGraph())[(int)position.x, (int)position.y - 1].getType())) {
                        position.y--;
                        moving = true;
                        if (wire) {
                            if (graph.getGraph()[(int)position.x, (int)position.y].getDistance() < aWire.getLenght() - 1) {
                                aWire.shortenTheWire(graph.getGraph()[(int)position.x, (int)position.y]);
                            } else {
                                aWire.lengthenWire(position, faceDir);
                            }
                        }
                    }
                }
            }
        } catch (Exception e) {
            print("WIN");
            Destroy(this);
            Destroy(LevelGenerator.player);
        }
    }

    private bool validZone(char c) {
        return c == 'F' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' ||
               c == '6' || c == '7' || c == 'B' || c == 'Y' || c == 'R' || c == 'O' || 
               c == 'P' || c == 'G' || c == 'D' || c == 'V' && LevelGenerator.currentLevel.getDoor() == null;
    }

    public static bool getWire() {
        return wire;
    }

    public Wire getAwire() {
        return aWire;
    }
}
