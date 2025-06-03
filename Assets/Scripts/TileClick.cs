//using UnityEngine;

//public class TileClick : MonoBehaviour
//{
//    private mapGeneration generator;
//    private int x, y;

//    public void Init(mapGeneration gen, int ix, int iy)
//    {
//        generator = gen;
//        x = ix;
//        y = iy;
//    }

//    void OnMouseDown()
//    {
//        if (!generator.bombsGenerated)
//        {
//            generator.GenerateBombs(x, y);
//        }

//        generator.RevealTile(x, y);
//    }
//}
