using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    
    public static Level currentLevel;
    private static char[,] levelTest = new char[12, 12] { { 'W', 'W', 'V', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'W', 'F', 'W', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'F', 'W' },
                                                       { 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W' } };

    [SerializeField]
    private int level;


    void Start() {
        if(level == 1) {
            initiateLevelTest();
        }
    }

    void initiateLevelTest() {
        currentLevel = new Level(levelTest, initiateButtonLevelTest(), initiateColorWallLevelTest());
    }

    private List<Button> initiateButtonLevelTest() {
        List<Button> buttonList = new List<Button>();
        Button button1 = new Button(new Vector2(2, 8), 'R');
        Button button3 = new Button(new Vector2(4, 2), 'G');
        Button button4 = new Button(new Vector2(7, 7), 'O');
        buttonList.Add(button1);
        buttonList.Add(button3);
        buttonList.Add(button4);
        return buttonList;
    }

    private List<Wall> initiateColorWallLevelTest() {
        List<Wall> listWall = new List<Wall>();
        Wall wall1 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(2, 11));
        Wall wall2 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(3, 11));
        Wall wall3 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(4, 11));
        Wall wall4 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(5, 11));
        Wall wall5 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(6, 11));
        Wall wall6 = new Wall(true, 'B', new Vector2(0, -1), new Vector2(7, 11));
        Wall wall7 = new Wall(true, 'R', new Vector2(1, 0), new Vector2(0, 5));
        Wall wall9 = new Wall(true, 'B', new Vector2(1, 0), new Vector2(0, 6));
        Wall wall10 = new Wall(true, 'Y', new Vector2(-1, 0), new Vector2(11, 4));
        listWall.Add(wall1);
        listWall.Add(wall2);
        listWall.Add(wall3);
        listWall.Add(wall4);
        listWall.Add(wall5);
        listWall.Add(wall6);
        listWall.Add(wall7);
        listWall.Add(wall9);
        listWall.Add(wall10);
        return listWall;
    }
}


