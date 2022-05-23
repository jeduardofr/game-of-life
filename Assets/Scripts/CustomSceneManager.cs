using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public void LoadSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}
