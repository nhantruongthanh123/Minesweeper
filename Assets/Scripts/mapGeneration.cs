using UnityEngine;
using System.Collections.Generic;

public class mapGeneration : MonoBehaviour
{
    public int xAxis = 10;
    public int yAxis = 10;
    public int numBomb = 12;
    public GameObject unknown;
    public GameObject known;
    public GameObject bomb;

    private Tile[,] mapTable;
    public bool bombsGenerated = false;

    void Start()
    {
        mapTable = new Tile[xAxis, yAxis];

        for (int x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++)
            {
                GameObject obj = Instantiate(unknown, new Vector3(x, y, 0), Quaternion.identity);
                obj.name = $"Tile_{x}_{y}";

                mapTable[x, y] = new Tile { tileObj = obj };
            }
        }
    }

    public void GenerateBombs(int clickedX, int clickedY)
    {
        bombsGenerated = true;
        int safeZoneSize = Mathf.Max(xAxis, yAxis) / 10;

        List<Vector2Int> possibleSpots = new List<Vector2Int>();

        for (int x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++)
            {
                if (Mathf.Abs(x - clickedX) > safeZoneSize || Mathf.Abs(y - clickedY) > safeZoneSize)
                {
                    possibleSpots.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < numBomb && possibleSpots.Count > 0; i++)
        {
            int idx = Random.Range(0, possibleSpots.Count);
            Vector2Int pos = possibleSpots[idx];
            mapTable[pos.x, pos.y].isBomb = true;
            possibleSpots.RemoveAt(idx);
        }

        for (int x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++)
            {
                mapTable[x, y].vicinityBombs = CountAdjacentBombs(x, y);
            }
        }

        Debug.Log("Bombs generated.");
    }

    public int CountAdjacentBombs(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < xAxis && ny < yAxis && mapTable[nx, ny].isBomb)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void RevealTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= xAxis || y >= yAxis) return;

        Tile tile = mapTable[x, y];
        if (tile.revealed) return;

        tile.revealed = true;
        Destroy(tile.tileObj);

        if (tile.isBomb)
        {
            tile.tileObj = Instantiate(bomb, new Vector3(x, y, 0), Quaternion.identity);
            return;
        }

        tile.tileObj = Instantiate(known, new Vector3(x, y, 0), Quaternion.identity);
        // Set bomb count if using number script
        number numberScript = tile.tileObj.GetComponentInChildren<number>();
        if (numberScript != null)
        {
            numberScript.SetBombCount(tile.vicinityBombs);
        }


        // Recursive revealing
        if (tile.vicinityBombs == 0)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    RevealTile(x + dx, y + dy);
                }
            }
        }
    }
    private void ToggleFlag(int x, int y)
    {
        Tile tile = mapTable[x, y];
        if (tile.revealed || tile.tileObj == null) return;

        Animator animator = tile.tileObj.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            bool current = animator.GetBool("flag");
            animator.SetBool("flag", !current);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouse2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            for (int x = 0; x < xAxis; x++)
            {
                for (int y = 0; y < yAxis; y++)
                {
                    Vector2 tilePos = new Vector2(x, y);
                    if (Mathf.Abs(mouse2D.x - tilePos.x) <= 0.9f && Mathf.Abs(mouse2D.y - tilePos.y) <= 0.9f)
                    {
                        if (Input.GetMouseButtonDown(0)) // Left-click
                        {
                            if (!bombsGenerated)
                            {
                                GenerateBombs(x, y);
                            }

                            RevealTile(x, y);
                        }
                        else if (Input.GetMouseButtonDown(1)) // Right-click
                        {
                            ToggleFlag(x, y);
                        }
                        return; // One tile per click
                    }
                }
            }
        }
    }

}
