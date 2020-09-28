using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseGenerator : MonoBehaviour
{
    public GameObject player;
    GameObject gui;

    public GameObject startSquare;
    public GameObject endSquare;

    public GameObject[] squareOnes;
    public GameObject[] squareTwoStraights;
    public GameObject[] squareTwoBents;
    public GameObject[] squareThrees;
    public GameObject[] squareFours;

    private const int UP = 10;
    private const int RIGHT = 11;
    private const int DOWN = 12;
    private const int LEFT = 13;

    private const int START = 2;
    private const int END = 3;

    private const int OCC = 1; // Occupied

    private const int GRIDSIZE = 11;
    private const int GRIDSIZEMULTIPLIER = 10;
    private float[] GRIDOFFSET = new float[] {-5.5f, 0};

    public int[,] grid = new int[11, 11];
    private int[,] moveGrid = new int[11, 11];

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gui = GameObject.FindWithTag("GUI");
        generateGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void regenerateGame()
    {
        gui.GetComponent<GuiController>().setLatestHit("");
        gui.GetComponent<GuiController>().resetHitCount();
        deleteScene();
        deleteGrid();
        generateGame();
    }

    void generateGame()
    {
        generateCourse();
        fillScene();
        placeStart();
        placeEnd();
        resetPlayer();
    }

    public void resetPlayer()
    {
        player.transform.position = new Vector3(((int)Mathf.Floor(GRIDSIZE / 2.0f) * 10) - 5, 2, 0);
    }

    void deleteScene()
    {
        var clones = GameObject.FindGameObjectsWithTag ("GridItem");
        foreach (var clone in clones){
            Destroy(clone);
        }
    }

    void deleteGrid()
    {
        for(int i = 0; i < GRIDSIZE; i++) {
            for(int j = 0; j < GRIDSIZE; j++) {
                grid[i, j] = 0;
            }
        }
    }

    void generateCourse()
    {
        grid[(int)Mathf.Floor(GRIDSIZE / 2.0f), 0] = START;
        grid[(int)Mathf.Floor(GRIDSIZE / 2.0f), GRIDSIZE - 1] = END;

        for(int i = 0; i < GRIDSIZE; i++) {
            for(int j = 0; j < GRIDSIZE; j++) {

                int[] up = new int[] { i, j+1 };
                int[] right = new int[] { i+1, j };
                int[] down = new int[] { i, j-1 };
                int[] left = new int[] { i-1, j };

                int bestDistance = 999999;
                int distance;
                int direction = RIGHT;

                distance = (int)Mathf.Sqrt(Mathf.Pow(((int)Mathf.Floor(GRIDSIZE / 2.0f) - up[0]), 2) + Mathf.Pow((GRIDSIZE - 1 - up[1]), 2));
                if(distance < bestDistance) {
                    bestDistance = distance;
                    direction = UP;
                }

                distance = (int)Mathf.Sqrt(Mathf.Pow(((int)Mathf.Floor(GRIDSIZE / 2.0f) - right[0]), 2) + Mathf.Pow((GRIDSIZE - 1 - right[1]), 2));
                if(distance < bestDistance) {
                    bestDistance = distance;
                    direction = RIGHT;
                }

                distance = (int)Mathf.Sqrt(Mathf.Pow(((int)Mathf.Floor(GRIDSIZE / 2.0f) - down[0]), 2) + Mathf.Pow((GRIDSIZE - 1 - down[1]), 2));
                if(distance < bestDistance) {
                    bestDistance = distance;
                    direction = DOWN;
                }

                distance = (int)Mathf.Sqrt(Mathf.Pow(((int)Mathf.Floor(GRIDSIZE / 2.0f) - left[0]), 2) + Mathf.Pow((GRIDSIZE - 1 - left[1]), 2));
                if(distance < bestDistance) {
                    bestDistance = distance;
                    direction = LEFT;
                }

                moveGrid[i, j] = direction;
            }
        }

        int[] pos = new int[] { (int)Mathf.Floor(GRIDSIZE / 2.0f), 1 };
        int nextDirection;
        int wrongDirection = -1;
        double num;
        Random rand = new Random(); 

        while(!(pos[0] == (int)Mathf.Floor(GRIDSIZE / 2.0f) && pos[1] == GRIDSIZE - 1)) {

            nextDirection = moveGrid[pos[0], pos[1]];
            grid[pos[0], pos[1]] = OCC;

            num = Random.Range(0.0f, 1.0f);

            wrongDirection = -1;

            if(num < 0.25) {
                while(wrongDirection == -1) {
                    num = Random.Range(0.0f, 1.0f);

                    if(num < 0.25 && nextDirection != UP) {
                        wrongDirection = UP;
                    } else if(num >= 0.25 && num < 0.5 && nextDirection != RIGHT) {
                        wrongDirection = RIGHT;
                    } else if(num >= 0.5 && num < 0.75 && nextDirection != DOWN) {
                        wrongDirection = DOWN;
                    } else if(nextDirection != LEFT) {
                        wrongDirection = LEFT;
                    }
                }

                nextDirection = wrongDirection;
            }

            if(nextDirection == UP && pos[1] < GRIDSIZE - 1) {
                pos[1]++;
            } else if(nextDirection == RIGHT && pos[0] < GRIDSIZE - 1) {
                pos[0]++;
            } else if(nextDirection == DOWN && pos[1] > 0) {
                pos[1]--;
            } else if(nextDirection == LEFT && pos[0] > 0) {
                pos[0]--;
            }
        }
    }

    void fillScene()
    {
        for(int i=0; i<GRIDSIZE; i++) {
            for(int j=0; j<GRIDSIZE; j++) {

                int gridNum = grid[i,j];

                if(gridNum == 1) {

                    int[] neighbors = getBorders(i, j);

                    GameObject newObj = getCorrectGameobject(neighbors);
                    Quaternion rotation = getCorrectRotation(neighbors);

                    Instantiate(newObj, new Vector3((i * GRIDSIZEMULTIPLIER + GRIDOFFSET[0]), 0, (j * GRIDSIZEMULTIPLIER + GRIDOFFSET[1])), rotation);
                }
            }
        }
    }

    /**
    This function gets the grid object with the correct number of sides open
    */
    GameObject getCorrectGameobject(int[] borderArray)
    {
        int sum = borderArray[0] + borderArray[1] + borderArray[2] + borderArray[3];
        GameObject gridItem = null;

        if(sum == 1) {
            gridItem = chooseRandom(squareOnes);
        } else if(sum == 2) {
            if((borderArray[0] != 0 && borderArray[2] != 0) || (borderArray[1] != 0 && borderArray[3] != 0)) {
                gridItem = chooseRandom(squareTwoStraights);
            } else {
                gridItem = chooseRandom(squareTwoBents);
            }
        } else if(sum == 3) {
            gridItem = chooseRandom(squareThrees);
        } else if(sum == 4) {
            gridItem = chooseRandom(squareFours);
        }

        return gridItem;
    }

    /**
    This function gets the correct rotation for a grid object 
    */
    Quaternion getCorrectRotation(int[] borderArray)
    {
        int sum = borderArray[0] + borderArray[1] + borderArray[2] + borderArray[3];
        float rotation = 5;

        if(sum == 1) {
            if(borderArray[0] != 0) {
                rotation = 180;
            } else if(borderArray[1] != 0) {
                rotation = 270;
            } else if(borderArray[2] != 0) {
                rotation = 0;
            } else {
                rotation = 90;
            }
        } else if(sum == 2) {
            if((borderArray[0] != 0 && borderArray[2] != 0) || (borderArray[1] != 0 && borderArray[3] != 0)) {
                if(borderArray[0] != 0) {
                    rotation = 0;
                } else {
                    rotation = 90;
                }
            } else {
                if(borderArray[0] != 0 && borderArray[1] != 0) {
                    rotation = 180;
                } if(borderArray[1] != 0 && borderArray[2] != 0) {
                    rotation = -90;
                } if(borderArray[2] != 0 && borderArray[3] != 0) {
                    rotation = 0;
                } if(borderArray[3] != 0 && borderArray[0] != 0) {
                    rotation = 90;
                }
            }
        } else if(sum == 3) {
            if(borderArray[0] == 0) {
                rotation = 0;
            } else if(borderArray[1] == 0) {
                rotation = 90;
            } else if(borderArray[2] == 0) {
                rotation = 180;
            } else {
                rotation = 270;
            }        
        } else if(sum == 4) {
            rotation = 0;
        }

        return Quaternion.Euler(0, rotation, 0);
    }

    void placeStart()
    {
        int[] point = new int[] {(int)Mathf.Floor(GRIDSIZE / 2.0f), 0};
        int[] borders = getBorders(point[0], point[1]);
        Quaternion rotation = getCorrectRotation(borders);

        Instantiate(startSquare, new Vector3((point[0] * GRIDSIZEMULTIPLIER + GRIDOFFSET[0]), 0, (point[1] * GRIDSIZEMULTIPLIER + GRIDOFFSET[1])), rotation);
    }

    void placeEnd()
    {
        int[] point = new int[] {(int)Mathf.Floor(GRIDSIZE / 2.0f), GRIDSIZE - 1};
        int[] borders = getBorders(point[0], point[1]);
        Quaternion rotation = getCorrectRotation(borders);

        Instantiate(endSquare, new Vector3((point[0] * GRIDSIZEMULTIPLIER + GRIDOFFSET[0]), 0, (point[1] * GRIDSIZEMULTIPLIER + GRIDOFFSET[1])), rotation);
    }

    int[] getBorders(int i, int j)
    {
        return new int[] {
            j + 1 < GRIDSIZE && grid[i,j+1] > 0 ? 1 : 0,
            i + 1 < GRIDSIZE && grid[i+1,j] > 0 ? 1 : 0,
            j - 1 >= 0 && grid[i,j-1] > 0 ? 1 : 0,
            i - 1 >= 0 && grid[i-1,j] > 0 ? 1 : 0
        };
    }

    GameObject chooseRandom(GameObject[] items)
    {
        if(items.Length > 1) {
            Random rand = new Random(); 
            float num = Random.Range(0.0f, 1.0f);

            if(num > 0.5f) {
                num = Random.Range(1.0f, items.Length);

                return items[(int)num];
            } else {
                return items[0];
            }

        } else {
            return items[0];
        }
    }
}
