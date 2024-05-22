using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Building
{
    public enum ShapeType
    {
        None,
        Shape_Letter_I,
        Shape_Letter_L,
        Shape_Letter_O,
        Shape_Reverse_Letter_L,
        Shape_Reverse_Letter_R,
        Shape_Letter_R,
        Shape_Line_Down,
    }

    [Serializable]
    public static class BuildingShapeData
    {
        public static readonly Dictionary<ShapeType, Vector2Int[]> Cells = new Dictionary<ShapeType, Vector2Int[]>
        {
            { ShapeType.None, new Vector2Int[] { new Vector2Int(0, 0) } },
            { ShapeType.Shape_Letter_I, new Vector2Int[] { new (0, 0),new(0,1) } },
            { ShapeType.Shape_Letter_R, new Vector2Int[] { new (0, 0),new(0,1),new(1,1) } },
            { ShapeType.Shape_Letter_O, new Vector2Int[] { new (0, 0),new(1,0),new(0,1),new(1,1) } },
            { ShapeType.Shape_Letter_L, new Vector2Int[] { new (0, 0),new(0,1),new(1,0) } },
            { ShapeType.Shape_Line_Down, new Vector2Int[] { new (0, 0),new(1,0) } },
            { ShapeType.Shape_Reverse_Letter_R, new Vector2Int[] { new (1, 0),new(1,1),new(0,1) } },
            { ShapeType.Shape_Reverse_Letter_L, new Vector2Int[] { new (1, 1),new(1,0),new(0,0) } },
        };
    }
}