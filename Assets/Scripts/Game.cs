using System;
using System.Globalization;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Game : MonoBehaviour
{
    public int width = 14;
    public int height = 14;
    public int mineCount;
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
        state = new CellData[width, height];

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
                cell.CellType = CellData.Type.Empty;
                state[i, j] = cell;
            }
        }
    }

    private void GenerateMine()
    {
        mineCount = width * height * 15 / 100;
        for (int i = 0; i < mineCount; i++)
        {
            int w = UnityEngine.Random.Range(0, width - 1);
            int h = UnityEngine.Random.Range(0, height - 1);
            while (state[w, h].CellType == CellData.Type.Mine)
            {
                w = UnityEngine.Random.Range(0, width - 1);
                h = UnityEngine.Random.Range(0, height - 1);
            }

            state[w, h].CellType = CellData.Type.Mine;
            state[w, h].IsRevealed = true;
        }
    }

    private void GenerateNumber()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (state[i, j].CellType != CellData.Type.Mine)
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

                            if (state[x, y].CellType == CellData.Type.Mine) numOfMine++;
                        }
                    }

                    if (numOfMine == 0)
                    {
                        state[i, j].CellType = CellData.Type.Empty;
                        state[i, j].IsRevealed = false;
                        continue;
                    }

                    state[i, j].Number = numOfMine;
                    state[i, j].CellType = CellData.Type.Number;
                    state[i, j].IsRevealed = false;
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
            if (state[tilePosition.x, tilePosition.y].IsRevealed == false)
            {
                state[tilePosition.x, tilePosition.y].IsFlag = true;
            }
        }

        board.Draw(state);


    }



}
