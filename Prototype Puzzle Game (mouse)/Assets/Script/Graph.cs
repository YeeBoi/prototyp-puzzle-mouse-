using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    private Node [,] graph;
    private int row;
    private int column;

    public Graph() {
        graph = null;
        row = 0;
        column = 0;
    }

    public Graph(char[,] charGraph) {
        Node node;
        this.row = charGraph.GetLength(0);
        this.column = charGraph.GetLength(1);
        graph = new Node[charGraph.GetLength(0), charGraph.GetLength(1)];
        for (int i = 0; i < charGraph.GetLength(0); i++) {
            for (int j = 0; j < charGraph.GetLength(1); j++) {
                node = new Node(charGraph[i, j], new Vector2(i, j), this);
                graph[i, j] = node;
            }
        }

        for (int i = 0; i < charGraph.GetLength(0); i++) {
            for (int j = 0; j < charGraph.GetLength(1); j++) {
                if (graph[i, j].getType() != 'W' && graph[i, j].getType() != 'V') {
                    graph[i, j].addNeighbour();
                }
            }
        }
    }

    public void drawWire(Wire wire) {
        /* wire color 1: blue
                      2: yellow
                      3: red 
        */
        char color = wire.colorChar();
        char c;
        List<Vector2> wirePosition = wire.getPosition();
        if (wire.getColor().Equals("blue")) {
            color = '1';
        } else if (wire.getColor().Equals("yellow")) {
            color = '2';
        } else {
            color = '3';
        }
        for (int i = 0; i < wirePosition.Count; i++) {
            c = combineChar(color, graph[(int)wirePosition[i].x, (int)wirePosition[i].y].getType());
            graph[(int)wirePosition[i].x, (int)wirePosition[i].y].setType(c);
        }
    }

    public char combineChar (char colorOfWire, char colorOfZone) {
        char c;
        if (colorOfWire == '1') {
            if (colorOfZone == '2' || colorOfZone == '4') {
                c = '4';
            } else if (colorOfZone == '3' || colorOfZone == '5') {
                c = '5';
            } else if (colorOfZone == '6' || colorOfZone == '7') {
                c = '7';
            } else {
                c = '1';
            }
        } else if (colorOfWire == '2') {
            if (colorOfZone == '1' || colorOfZone == '4') {
                c = '4';
            } else if (colorOfZone == '3' || colorOfZone == '6') {
                c = '6';
            } else if (colorOfZone == '5' || colorOfZone == '7') {
                c = '7';
            } else {
                c = '2';
            }
        } else {
            if (colorOfZone == '1' || colorOfZone == '5') {
                c = '5';
            } else if (colorOfZone == '2' || colorOfZone == '6') {
                c = '6';
            } else if (colorOfZone == '4' || colorOfZone == '7') {
                c = '7';
            } else {
                c = '3';
            }
        }
        return c;
    }

    public Node [,] getGraph() {
        return graph;
    }

    public void reinitialiserDistance() {
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < column; j++) {
                graph[i, j].reinitialise();
            }
        }
    }

    public void findShortestPath(Node depart) {
        List<Node> nodeToDo = new List<Node>();
        List<Node> nodeDone = new List<Node>();
        nodeDone.Add(depart);
        nodeToDo.AddRange(depart.getNeighbour());
        for (int i = 0; i < nodeToDo.Count; i++) {
            nodeToDo[i].setDistance(1);
        }
        while (nodeToDo.Count > 0) {
            for (int i = 0; i < nodeToDo[0].getNeighbour().Count; i++) {
                if (!(nodeToDo.Contains(nodeToDo[0].getNeighbour()[i])) && !(nodeDone.Contains(nodeToDo[0].getNeighbour()[i]))) {
                    nodeToDo.Add(nodeToDo[0].getNeighbour()[i]);
                    nodeToDo[0].getNeighbour()[i].setDistance(nodeToDo[0].getDistance() + 1);
                }
            }
            nodeDone.Add(nodeToDo[0]);
            nodeToDo.RemoveAt(0);
        }
    }

    public int getRow() {
        return row;
    }

    public int getColumn() {
        return column;
    }
}
