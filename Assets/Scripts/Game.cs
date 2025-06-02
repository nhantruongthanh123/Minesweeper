using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
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
        int mineCount = width + height;
        for (int i = 0; i < mineCount; i++)
        {
        }
    }

}
