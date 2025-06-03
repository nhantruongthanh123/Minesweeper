using UnityEngine;

public struct CellData
{
    public enum Type
    {
        Empty,
        Mine,
        Number
    }

    public Vector3Int position;
    public Type cellType;
    public int number; 
    public bool isFlag;
    public bool isRevealed;
    public bool isExploded;
}

public class Cell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
