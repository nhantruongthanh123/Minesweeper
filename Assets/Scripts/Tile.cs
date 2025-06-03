using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject tileObj;
    public bool isBomb = false;
    public bool revealed = false;
    public int vicinityBombs = 0;
}
