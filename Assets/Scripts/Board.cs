using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tile tileEmpty;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileMine;
    public Tile tileUnknown;
    public Tile[] tileNumber = new Tile[9];


    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private Tile getTile(CellData cell)
    {
        if (cell.isRevealed)
        {
            if (cell.cellType == CellData.Type.Number)
            {
                return tileNumber[cell.number];
            }
            else if (cell.cellType == CellData.Type.Empty)
            {
                return tileEmpty;
            }
            else
            {
                return tileExploded;
            }
        }
        else if (cell.isFlag)
        {
            return tileFlag;
        }

        return tileUnknown;
    }
    public void Draw(CellData[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CellData cell = state[i, j];
                tilemap.SetTile(cell.position, getTile(cell));
            }
        }
    }
}
