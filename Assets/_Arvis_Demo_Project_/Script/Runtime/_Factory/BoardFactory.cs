using _Arvis_Demo_Project_.Common._Board;
using _Arvis_Demo_Project_.Common._Building;
using System.Collections.Generic;
using UnityEngine;

namespace _Arvis_Demo_Project_Runtime._Factory
{
    public sealed class BoardFactory
    {
        public Board CreateBoard(GameObject tile, List<IDropable> buildingTileAreas)
        {
            return new(tile, buildingTileAreas);
        }
    }
}