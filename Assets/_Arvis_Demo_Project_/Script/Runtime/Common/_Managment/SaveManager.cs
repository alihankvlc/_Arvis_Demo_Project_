using _Arvis_Demo_Project_.Common._Framework;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

namespace _Arvis_Demo_Project_.Common._Managment
{
    public sealed class SaveManager : Singleton<SaveManager>
    {

        public void Save<T>(T data, string dataName = "playerData")
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                string path = $"{Application.persistentDataPath}/{dataName}.json";

                File.WriteAllText(path, json);

                Debug.Log("Data saved successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }

        public T Load<T>(string dataName = "playerData")
        {
            try
            {
                string path = $"{Application.persistentDataPath}/{dataName}.json";

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    return JsonUtility.FromJson<T>(json);
                }

                return default(T);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load data: {ex.Message}");
                return default(T);
            }
        }

        [Button("Clear")]
        public void ClearAllData()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);

                foreach (FileInfo file in directory.GetFiles())
                    file.Delete();

                Debug.Log("All data cleared successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to clear data: {ex.Message}");
            }
        }
    }
}
