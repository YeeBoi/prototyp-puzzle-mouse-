using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

    private int length = 0;
    private Vector2 direction;
    private string color;
    private List<Vector2> wirePosition;
    private List<GameObject> wireDrawn;
    private GameObject straightWire;
    private GameObject curveWire;
    private GameObject zone;
    private List<Vector2> zoneColor;
    private List<Vector2> positionZoneColor = new List<Vector2>();
    private Vector2 wireDirection = new Vector2();
    private bool activate = false;
    private bool mixColorChange = true;
    private bool isActive = false;


    public Wire() {
        length = 0;
        wirePosition = new List<Vector2>();
        wireDrawn = new List<GameObject>();
        zoneColor = new List<Vector2>();
    }

    public Wire(Vector2 firstPosition, Vector2 direction, char c) : this () {
        string color;
        if (c == 'B') {
            color = "blue";
        } else if (c == 'Y') {
            color = "yellow";
        } else {
            color = "red";
        }
        length++;
        this.color = color;
        wirePosition.Add(firstPosition);
        changeColor(color);
        this.direction = direction;
        drawWire();
    }

    public void lengthenWire(Vector2 position, Vector2 direction) {
        length++;
        wirePosition.Add(position);
        this.direction = direction;
        drawWire();
    }

    public void deleteWire() {
        removeOnGraph();
        wirePosition.Clear();
        for (int i = 0; i < wireDrawn.Count; i++) {
            Destroy(wireDrawn[i]);
        }
        deleteZoneColor();
        length = 0;
    }

    public void removeOnGraph() {
        for (int i = 0; i < wirePosition.Count; i++) {
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)wirePosition[i].x, (int)wirePosition[i].y].setType('F');
        }
    }
    
    private bool XYValid(bool minus, bool plus, int i) {
        bool valid = true;
        if (minus && (int)wirePosition[i].x > (int)wirePosition[i - 1].x) {
            valid = false;
        } else if (plus && (int)wirePosition[i].x < (int)wirePosition[i - 1].x) {
            valid = false;
        }
        if (minus && (int)wirePosition[i].y > (int)wirePosition[i - 1].y) {
            valid = false;
        } else if (plus && (int)wirePosition[i].y < (int)wirePosition[i - 1].y) {
            valid = false;
        }
        return valid;
    }
    
    public void colorSide() {
        deleteZoneColor();
        List<Vector2> positionDone = new List<Vector2>();
        bool XMinus = false;
        bool XPlus = false;
        bool YMinus = false;
        bool YPlus = false;
        bool straightLine = false;
        mixColorChange = true;
        for (int i = 0; i < wirePosition.Count; i++) {
            positionDone.Add(wirePosition[i]);
            if (i != 0) {
                if (!XMinus && !XPlus && (int)wirePosition[i].x > (int)wirePosition[i - 1].x) {
                    XPlus = true;
                } else if (!XMinus && !XPlus && (int)wirePosition[i].x < (int)wirePosition[i - 1].x) {
                    XMinus = true;
                }

                if (!YMinus && !YPlus && (int)wirePosition[i].y > (int)wirePosition[i - 1].y) {
                    YPlus = true;
                } else if (!YMinus && !YPlus && (int)wirePosition[i].y < (int)wirePosition[i - 1].y) {
                    YMinus = true;
                }
            }
        }
        if (!XMinus && !XPlus && !YMinus || !XMinus && !YPlus && !YMinus ||
            !XPlus && !YPlus && !YMinus || !XMinus && !XPlus && !YPlus ) {
            straightLine = true;
        }
        if (!straightLine) {
            if ((LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)wirePosition[0].x, (int)wirePosition[0].y - 1].getType() == 'W' ||
                LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)wirePosition[0].x, (int)wirePosition[0].y + 1].getType() == 'W') && 
                wirePosition.Contains(new Vector2 ((int)wirePosition[0].x, (int)wirePosition[0].y - 1)) ||
                wirePosition.Contains(new Vector2((int)wirePosition[0].x, (int)wirePosition[0].y + 1))) {
                if (XPlus && YPlus || XMinus && YMinus) {
                    if (activate) {
                        fillColor(new Vector2((int)wirePosition[0].x - 1, (int)wirePosition[0].y), positionDone);
                    } else {
                        fillColor(new Vector2((int)wirePosition[0].x + 1, (int)wirePosition[0].y), positionDone);
                    }
                } else {
                    if (activate) {
                        fillColor(new Vector2((int)wirePosition[0].x + 1, (int)wirePosition[0].y), positionDone);
                    } else {
                        fillColor(new Vector2((int)wirePosition[0].x - 1, (int)wirePosition[0].y), positionDone);
                    }
                }
            } else {
                if (activate) {
                    fillColor(new Vector2((int)wirePosition[0].x, (int)wirePosition[0].y + 1), positionDone);
                } else {
                    fillColor(new Vector2((int)wirePosition[0].x, (int)wirePosition[0].y - 1), positionDone);
                }

            }
        }

        for (int i = 1; i < wirePosition.Count; i++) {
            if (!straightLine) {
                if ((int)wirePosition[i].x != (int)wirePosition[i - 1].x) {
                    if (XYValid(XMinus, XPlus, i)) {
                        if (activate) {
                            fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y + 1), positionDone);
                        } else {
                            fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y - 1), positionDone);
                        }
                    } else {
                        if (activate) {
                            fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y - 1), positionDone);
                        } else {
                            fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y + 1), positionDone);
                        }
                    }
                } else {
                    if (XYValid(YMinus, YPlus, i)) {
                        if (activate) {
                            if (YPlus && XPlus || YMinus && XMinus) {
                                fillColor(new Vector2((int)wirePosition[i].x - 1, (int)wirePosition[i].y), positionDone);
                            } else {
                                fillColor(new Vector2((int)wirePosition[i].x + 1, (int)wirePosition[i].y), positionDone);
                            }
                        } else {
                            if (YPlus && XPlus || YMinus && XMinus) {
                                fillColor(new Vector2((int)wirePosition[i].x + 1, (int)wirePosition[i].y), positionDone);
                            } else {
                                fillColor(new Vector2((int)wirePosition[i].x - 1, (int)wirePosition[i].y), positionDone);
                            }
                        }
                    } else {
                        if (activate) {
                            if (YPlus && XPlus || YMinus && XMinus) {
                                fillColor(new Vector2((int)wirePosition[i].x + 1, (int)wirePosition[i].y), positionDone);
                            } else {
                                fillColor(new Vector2((int)wirePosition[i].x - 1, (int)wirePosition[i].y), positionDone);
                            }
                        } else {
                            if (YPlus && XPlus || YMinus && XMinus) {
                                fillColor(new Vector2((int)wirePosition[i].x - 1, (int)wirePosition[i].y), positionDone);
                            } else {
                                fillColor(new Vector2((int)wirePosition[i].x + 1, (int)wirePosition[i].y), positionDone);
                            }
                        }
                    }
                }
            } else {
                if (XPlus || XMinus) {
                    if (activate) {
                        fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y - 1), positionDone);
                    } else {
                        fillColor(new Vector2((int)wirePosition[i].x, (int)wirePosition[i].y + 1), positionDone);
                    }
                } else {
                    if (activate) {
                        fillColor(new Vector2((int)wirePosition[i].x - 1, (int)wirePosition[i].y), positionDone);
                    } else {
                        fillColor(new Vector2((int)wirePosition[i].x + 1, (int)wirePosition[i].y), positionDone);
                    }
                }
            }
        }
        activate = !activate;
        LevelGenerator.currentLevel.checkButton();
    }

    private void drawWire() {
        if (direction.x != 0) {
            GameObject wire = (GameObject)Instantiate(straightWire, wirePosition[wirePosition.Count - 1], Quaternion.Euler(0, 0, 90));
            wireDrawn.Add(wire);
        } else if (direction.y != 0) {
            GameObject wire = (GameObject)Instantiate(straightWire, wirePosition[wirePosition.Count - 1], Quaternion.Euler(0, 0, 0));
            wireDrawn.Add(wire);
        }        
    }
    
    public void shortenTheWire(Node position) {
        List<Node> nodeToFind = new List<Node>();
        Vector2 remove;
        nodeToFind.Add(position);
        List<Node> newPosition = findClosestWire(position, nodeToFind, 0);
        
        int index = wirePosition.IndexOf(newPosition[0].getPosition());
        while (wireDrawn.Count > index) {
            Destroy(wireDrawn[wireDrawn.Count-1]);
            wireDrawn.RemoveAt(wireDrawn.Count - 1);
            remove = wirePosition[wirePosition.Count - 1];
            wirePosition.RemoveAt(wirePosition.Count - 1);
        }
        for (int i = 0; i < newPosition.Count; i++) {
            wirePosition.Add(newPosition[i].getPosition());
        }
        wirePosition.Add(position.getPosition());
        length = wirePosition.Count + 1;
        newPosition.Add(position);
        redraw(newPosition);
    }

    public void redraw (List<Node> newPosition) {
        for (int i = 0; i < newPosition.Count; i++) {
            GameObject wire = (GameObject)Instantiate(straightWire, newPosition[i].getPosition(), Quaternion.Euler(0, 0, 0));
            wireDrawn.Add(wire);
        }
    }

    public List<Node> findClosestWire(Node position, List<Node> nodeToFind, int compteur) {
        List<Node> path = new List<Node>();
        for (int i = 0; i < position.findSmallestNeighbour().Count; i++) {
            if (!nodeToFind.Contains(position.findSmallestNeighbour()[i])) {
                nodeToFind.Add(position.findSmallestNeighbour()[i]);
            }
        }
        if (wirePosition.Contains(nodeToFind[compteur].getPosition())) {
            path.Add(nodeToFind[compteur]);
        } else {
            compteur++;
            path = findClosestWire(nodeToFind[compteur], nodeToFind,compteur);
            if (nodeToFind[compteur].getNeighbour().Contains(path[path.Count -1])) {
                path.Add(nodeToFind[compteur]);
            }
        }
        
        return path;
    }

    public void changeColor(string color) {
        if (color.Equals("blue")) {
            straightWire = (GameObject)Resources.Load("blue");
            curveWire = (GameObject)Resources.Load("Wire Curve-B");
            zone = (GameObject)Resources.Load("blue-T");
        } else if (color.Equals("red")) {
            straightWire = (GameObject)Resources.Load("red");
            curveWire = (GameObject)Resources.Load("Wire Curve-R");
            zone = (GameObject)Resources.Load("red-T");
        } else if (color.Equals("yellow")) {
            straightWire = (GameObject)Resources.Load("yellow");
            curveWire = (GameObject)Resources.Load("Wire Curve-Y");
            zone = (GameObject)Resources.Load("yellow-T");
        }
    }

    public List<Vector2> getPosition() {
        return wirePosition;
    }

    public int getLenght() {
        return length;
    }

    private void fillColor(Vector2 position, List<Vector2> positionDone) {
        char color = colorChar();
        char c = LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType();
        List<Node> neighbour;

        if (c != 'W' && c != 'V' && !positionDone.Contains(position) && !checkColor(c,color)) {
            if (c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && c != '6' && c != '7' && c != color) {
                if (c != 'F') {
                    color = mixColor(position, color);
                }
                LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].setType(color);
                GameObject colorZone = (GameObject)Instantiate(zone, new Vector3(position.x, position.y, -1), Quaternion.Euler(0, 0, 0));
                zoneColor.Add(new Vector2(position.x,position.y));
                LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].setColorOfPosition(colorZone);
            }
            positionDone.Add(position);
            changeColor(this.color);
            neighbour = LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getNeighbour();
            for (int i = 0; i < neighbour.Count; i++) {
                if (!positionDone.Contains(neighbour[i].getPosition())) {
                    fillColor(neighbour[i].getPosition(), positionDone);
                }
            }
        }
    }

    private bool checkColor(char c, char color) {
        bool same = false;
        if ((c == '1' || c == '4' || c == '5' || c == '7') && color == 'B') {
            same = true;
        } else if ((c == '2' || c == '4' || c == '6' || c == '7') && color == 'Y') {
            same = true;
        } else if ((c == '3' || c == '5' || c == '6' || c == '7') && color == 'R') {
            same = true;
        }
        return same;
    }

    public void deleteZoneColor() {
        char color;
        while(zoneColor.Count != 0) {
            color = LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].getType();
            if (color == colorChar()) {
                LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('F');
                LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].destroyColorOfPosition();
            } else if (color == 'O' || color == 'P' || color == 'G' || color == 'D') {
                replaceZoneColor(color);
            }
            zoneColor.RemoveAt(0);
        }
    }

    private void replaceZoneColor(char color) {
        if (mixColorChange) {
            if (color == 'O') {
                if (colorChar() == 'Y') {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('R');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("red-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                } else {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('Y');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("yellow-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                }
            } else if (color == 'P') {
                if (colorChar() == 'R') {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('B');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("blue-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                } else {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('R');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("red-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                }
            } else if (color == 'G') {
                if (colorChar() == 'B') {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('Y');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("yellow-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                } else {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('B');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("blue-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                }
            } else if (color == 'D') {
                if (colorChar() == 'B') {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('O');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("orange-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                } else if (colorChar() == 'Y') {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('P');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("purple-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                } else {
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setType('G');
                    GameObject colorZone = (GameObject)Instantiate((GameObject)Resources.Load("green-T"), new Vector3(zoneColor[0].x,
                                                        zoneColor[0].y, -1), Quaternion.Euler(0, 0, 0));
                    LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)zoneColor[0].x, (int)zoneColor[0].y].setColorOfPosition(colorZone);
                }
            }
        }
    }

    public void deleteZoneColorOnWire() {
        for (int i = 0;  i < wirePosition.Count; i ++) {
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)wirePosition[i].x, (int)wirePosition[i].y].destroyColorOfPosition();
        }
    }

    public char mixColor(Vector2 position, char color) {
        if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'B' && color == 'Y' ||
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'Y' && color == 'B') {
            zone = (GameObject)Resources.Load("green-T");
            color = 'G';
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'B' && color == 'R' ||
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'R' && color == 'B') {
            zone = (GameObject)Resources.Load("purple-T");
            color = 'P';
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'Y' && color == 'R' ||
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'R' && color == 'Y') {
            zone = (GameObject)Resources.Load("orange-T");
            color = 'O';
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'G' && color == 'R' ||
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'P' && color == 'Y' ||
            LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'O' && color == 'B') {
            zone = (GameObject)Resources.Load("brown-T");
            color = 'D';
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'P') {
            color = 'P';
            zone = (GameObject)Resources.Load("purple-T");
            mixColorChange = false;
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'O') {
            color = 'O';
            mixColorChange = false;
            zone = (GameObject)Resources.Load("orange-T");
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'G') {
            color = 'G';
            mixColorChange = false;
            zone = (GameObject)Resources.Load("green-T");
        } else if (LevelGenerator.currentLevel.getGraphLevel().getGraph()[(int)position.x, (int)position.y].getType() == 'D') {
            color = 'D';
            mixColorChange = false;
            zone = (GameObject)Resources.Load("brown-T");
        }
        return color;
    }

    public void switchActivate() {
        activate = !activate;
    }

    public char colorChar() {
        char c;
        if (color == "blue") {
            c = 'B';
        } else if (color == "yellow") {
            c = 'Y';
        } else {
            c = 'R';
        }
        return c;
    }

    public List<Vector2> getWirePosition() {
        return wirePosition;
    }

    public void setWireDirection(Vector2 wireDirection) {
        this.wireDirection = wireDirection;
    }

    public Vector2 getWireDirection() {
        return wireDirection;
    }

    public void setActivate (bool activate) {
        this.activate = activate;
    }

    public bool getActivate() {
        return activate;
    }

    public List<Vector2> getZoneColor() {
        return zoneColor;
    }

    public string getColor() {
        return color;
    }

    public void setIsActive(bool isActive) {
        this.isActive = isActive;
    }

    public bool getIsActive() {
        return isActive;
    }
}
