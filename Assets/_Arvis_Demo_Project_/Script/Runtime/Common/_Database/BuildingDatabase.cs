using _Arvis_Demo_Project_.Common._Building;
using UnityEditor;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Database
{
    [CreateAssetMenu(fileName = "BuildingDatabase", menuName = "Arvis_Project/Create Building Database")]
    public sealed class BuildingDatabase : Database<BuildingData>
    {
        private static BuildingDatabase _instance;
        public static BuildingDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<BuildingDatabase>("BuildingDatabase");
                    if (_instance == null)
                    {
                        string path = $"Assets/Resources/BuildingDatabase.asset";

                        _instance = CreateInstance<BuildingDatabase>();
                        AssetDatabase.CreateAsset(_instance, path);
                    }
                }
                return _instance;
            }
        }

        public override void Init()
        {
            base.Init();
        }

        public BuildingData GetData(int id)
        {
            if (Cache.TryGetValue(id, out var outputData))
                return outputData;

            return null;
        }
    }
}
