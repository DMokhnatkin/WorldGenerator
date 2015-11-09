using UnityEngine;
using System.Collections;
using System.IO;

namespace Map.Generator
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            DiamondSquare sq = new DiamondSquare();
            sq.strength = 1;
            Texture2D text = new Texture2D(513, 513);
            HeightMap res = sq.Generate(513);
            res.Normilize(1);
            for (int i = 0; i < res.val.GetLength(0); i++)
                for (int j = 0; j < res.val.GetLength(1); j++)
                {
                    text.SetPixel(i, j, new Color(res.val[i,j], res.val[i, j], res.val[i, j], 0));
                }
            text.Apply();
            File.WriteAllBytes(Application.dataPath + "/../test.jpg", text.EncodeToJPG(75));
        }

    }
}

