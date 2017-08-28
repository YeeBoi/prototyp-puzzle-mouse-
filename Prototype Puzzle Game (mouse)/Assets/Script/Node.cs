using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    private char type;
    private List<Node> neighbour;
    private Vector2 position;
    private int distance = 0;
    private Graph graph;
    private GameObject colorOfPosition;

    public Node() {
        type = '\0';
        neighbour = new List<Node>();
        position = new Vector2(0, 0);
        graph = null;
        colorOfPosition = null;
    }

    public Node(char type, Vector2 position, Graph graph) : this() {
        this.position = position;
        this.type = type;
        this.graph = graph;
    }

    public void addNeighbour() {
        for (int i = (position.x > 0 ? -1 : 0); i <= (position.x < graph.getRow() ? 1 : 0); ++i) {
            for (int j = (position.y > 0 ? -1 : 0); j <= (position.y < graph.getColumn() ? 1 : 0); ++j) {
                if ((i != 0 || j != 0) && !(j != 0 && i != 0)) {
                    if ((graph.getGraph())[(int)position.x + i, (int)position.y + j].getType() != 'W' &&
                        (graph.getGraph())[(int)position.x + i, (int)position.y + j].getType() != 'V') {
                        neighbour.Add((graph.getGraph())[(int)position.x + i, (int)position.y + j]);
                    }
                }
            }
        }
    }

    public List<Node> findSmallestNeighbour() {
        List<Node> smallestNeighbour = new List<Node>();
        for (int i = 0; i < neighbour.Count; i++) {
            if (i == 0) {
                smallestNeighbour.Add(neighbour[0]);
            } else {
                if (neighbour[i].getDistance() < smallestNeighbour[0].getDistance()) {
                    smallestNeighbour = new List<Node>();
                    smallestNeighbour.Add(neighbour[i]);
                } else if (neighbour[i].getDistance() == smallestNeighbour[0].getDistance()) {
                    smallestNeighbour.Add(neighbour[i]);
                }
            }
        }
        return smallestNeighbour;
    }

    public void destroyColorOfPosition() {
        if (colorOfPosition != null) {
            Destroy(colorOfPosition);
            colorOfPosition = null;
        }
    }

    public override bool Equals(object other) {
        bool isEquals = true;
        if (type != ((Node)other).type) {
            isEquals = false;
        }
        if (!position.Equals(((Node)other).position)) {
            isEquals = false;
        }
        return isEquals;
    }

    public void reinitialise() {
        distance = 0;
    }

    public char getType() {
        return type;
    }

    public Vector2 getPosition() {
        return position;
    }

    public void setType(char type) {
        this.type = type;
    }

    public int getDistance() {
        return distance;
    }

    public void setDistance(int distance) {
        this.distance = distance;
    }

    public void setColorOfPosition(GameObject colorOfPosition) {
        if (colorOfPosition != null) {
            Destroy(this.colorOfPosition);
        }
        this.colorOfPosition = colorOfPosition;
    }

    public List<Node> getNeighbour() {
        return neighbour;
    }
}
