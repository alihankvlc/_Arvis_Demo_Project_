using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Arvis_Demo_Project_.Common._Managment
{
    public sealed class GameManager : MonoBehaviour
    {
        public void RestartGame()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}