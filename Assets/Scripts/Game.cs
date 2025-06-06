using System;
using System.Globalization;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public int width = 14;
    public int height = 14;
    public int mineCount;
    public bool isGameOver;
    public bool isWin;
    public int numNotRevealed;
    public int percentMine = 10; 
    private Board board;
    private CellData[,] state;

    void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        if (ButtonFunction.level == 0)
        {
            width = 10;
            height = 10;
            percentMine = 12;
        }
        else if (ButtonFunction.level == 1)
        {
            width = 12;
            height = 12;
            percentMine = 15;
        }
        else
        {
            width = 14;
            height = 14;
            percentMine = 18;
        }

        state = new CellData[width, height];
        isGameOver = false;
        isWin = false;
        numNotRevealed = width * height - width * height * percentMine / 100;

        GenerateCell();
        GenerateMine();
        GenerateNumber();

        board.Draw(state);
        Camera.main.transform.position = new Vector3(width / 2, height / 2, -10f);
        Camera.main.orthographicSize = 8f;
    }

    private void GenerateCell()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CellData cell = new CellData();
                cell.position = new Vector3Int(i, j, 0);
                cell.cellType = CellData.Type.Empty;
                state[i, j] = cell;
            }
        }
    }

    private void GenerateMine()
    {
        mineCount = width * height * percentMine / 100;
        for (int i = 0; i < mineCount; i++)
        {
            int w = UnityEngine.Random.Range(0, width - 1);
            int h = UnityEngine.Random.Range(0, height - 1);
            while (state[w, h].cellType == CellData.Type.Mine)
            {
                w = UnityEngine.Random.Range(0, width - 1);
                h = UnityEngine.Random.Range(0, height - 1);
            }

            state[w, h].cellType = CellData.Type.Mine;
            state[w, h].isRevealed = false;
        }
    }

    private void GenerateNumber()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (state[i, j].cellType != CellData.Type.Mine)
                {
                    int numOfMine = 0;

                    for (int hor = -1; hor <= 1; hor++)
                    {
                        for (int ver = -1; ver <= 1; ver++)
                        {
                            // if (hor == 0 && ver == 0) continue;

                            int x = i + hor;
                            int y = j + ver;

                            if (x < 0 || y < 0 || x >= width || y >= height) continue;

                            if (state[x, y].cellType == CellData.Type.Mine) numOfMine++;
                        }
                    }

                    if (numOfMine == 0)
                    {
                        state[i, j].cellType = CellData.Type.Empty;
                        state[i, j].isRevealed = false;
                        continue;
                    }

                    state[i, j].number = numOfMine;
                    state[i, j].cellType = CellData.Type.Number;
                    state[i, j].isRevealed = false;
                }
            }
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Flag();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Click();
        }

        if (numNotRevealed == 0) isWin = true;

    }

    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePosition = board.tilemap.WorldToCell(worldPosition);

        if (tilePosition.x < 0 || tilePosition.x >= width || tilePosition.y < 0 || tilePosition.y >= height)
        {
            return;
        }
        else
        {
            if (state[tilePosition.x, tilePosition.y].isFlag == true)
            {
                state[tilePosition.x, tilePosition.y].isFlag = false;
                state[tilePosition.x, tilePosition.y].isRevealed = false;
            }
            else if (state[tilePosition.x, tilePosition.y].isRevealed == false)
            {
                state[tilePosition.x, tilePosition.y].isFlag = true;
            }
        }

        board.Draw(state);
    }

    private void Click()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePosition = board.tilemap.WorldToCell(worldPosition);

        if (tilePosition.x < 0 || tilePosition.x >= width || tilePosition.y < 0 || tilePosition.y >= height)
        {
            return;
        }
        else
        {
            if (state[tilePosition.x, tilePosition.y].isFlag == true || state[tilePosition.x, tilePosition.y].isRevealed == false)
            {
                state[tilePosition.x, tilePosition.y].isRevealed = true;
                numNotRevealed--;

                if (state[tilePosition.x, tilePosition.y].cellType == CellData.Type.Mine)
                {
                    isGameOver = true;
                    board.Draw(state);
                    return;
                }

                if (state[tilePosition.x, tilePosition.y].cellType == CellData.Type.Empty)
                {
                    Flood(state[tilePosition.x, tilePosition.y]);
                }
            }
            else if (state[tilePosition.x, tilePosition.y].isRevealed == true)
            {
                return;
            }
        }

        board.Draw(state);
    }

    private void Flood(CellData cell)
    {
        if (cell.cellType != CellData.Type.Empty) return;
        int i = cell.position.x;
        int j = cell.position.y;
        for (int hor = -1; hor <= 1; hor++)
        {
            for (int ver = -1; ver <= 1; ver++)
            {
                if (hor == 0 && ver == 0) continue;

                int x = i + hor;
                int y = j + ver;

                if (x < 0 || y < 0 || x >= width || y >= height) continue;

                if (state[x, y].cellType == CellData.Type.Empty && state[x, y].isRevealed == false)
                {
                    state[x, y].isRevealed = true;
                    numNotRevealed--;
                    Flood(state[x, y]);
                }
                else if (state[x, y].cellType == CellData.Type.Number && state[x, y].isRevealed == false)
                {
                    state[x, y].isRevealed = true;
                    numNotRevealed--;
                }
            }
        }

    }

}
