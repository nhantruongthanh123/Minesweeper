using UnityEngine;

public struct CellData
{
    public enum Type
    {
        Empty,
        Mine,
        Number
    }

    Vector3Int position;
    public Type CellType;
    public int Number; 
    public bool IsFlag;
    public bool IsRevealed;
    public bool IsExploded;
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
