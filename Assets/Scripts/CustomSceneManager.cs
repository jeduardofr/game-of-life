using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    private CustomSceneManager m_Instance;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(m_Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}
