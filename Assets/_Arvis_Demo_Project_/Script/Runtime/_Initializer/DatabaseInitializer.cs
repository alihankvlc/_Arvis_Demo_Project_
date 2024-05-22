using _Arvis_Demo_Project_.Common._Database;
using UnityEngine;

namespace _Arvis_Demo_Project_Runtime.Initializer
{
    public class DatabaseInitializer : MonoBehaviour
    {
        [SerializeField] private BuildingDatabase _buildingDatabase;

        private void Awake()
        {
            _buildingDatabase ??= Resources.Load<BuildingDatabase>("BuildingDatabase");
            _buildingDatabase?.Init();
        }
    }
}