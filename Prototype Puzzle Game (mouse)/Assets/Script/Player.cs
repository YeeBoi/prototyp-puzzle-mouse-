using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    private Graph graph;
    private Vector3 position = new Vector3(1, 1, -2);
    private static bool wire = false;
    private List<Wire> allWire = new List<Wire>();
    private Wire aWire = null;
    private static List<Vector2> notPos = new List<Vector2>();
    private bool moving = false;

    public Camera camera;
    void Start() {
        graph = LevelGenerator.currentLevel.getGraphLevel();
        camera = FindObjectOfType<Camera>();
        this.gameObject.transform.position = position;
    }

    void Update() {
        print(Math.Round(camera.ScreenToWorldPoint(Input.mousePosition).x));
        //print(Input.mousePosition);

        activateColor();
        if (Input.GetButtonDown("wireDestroy")) {
            if (wire) {
                wire = false;
                aWire.deleteWire();
            }
        }
        if (Input.GetButtonDown("branch")) {
                wire = !wire;
                if (wire) {
                    aWire = new Wire(position, LevelGenerator.currentLevel.wallAt(new Vector2((int)position.x, (int)position.y)).getColor());
                    graph.findShortestPath(graph.getGraph()[(int)position.x, (int)position.y]);
                } else {
                    if (new Vector2(position.x, position.y) != aWire.getPosition()[0]) {
                        graph.drawWire(aWire);
                        notPos.Add(position);
                        notPos.Add(aWire.getPosition()[0]);
                        graph.reinitialiserDistance();
                        aWire.deleteZoneColorOnWire();
                        reloadAllWire();
                        allWire.Add(aWire);
                    } else {
                        aWire.deleteWire();
                    }
                }
            } else if ((graph.getGraph())[(int)position.x, (int)position.y].getType() == 'W' && !wire) {
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
