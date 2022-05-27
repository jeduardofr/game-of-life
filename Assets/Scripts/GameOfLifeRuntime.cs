using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum PlayingState
{
    ComputeChanges = 0,
    ApplyChanges = 1,
    Lose = 2,
    Win = 3
}

public class GameOfLifeRuntime : MonoBehaviour
{
    private bool m_Playing;
    private int m_Width;
    private int m_Height;
    private int m_Generations;

    private int m_RuntimeGenerations;
    private PlayingState m_State;
    private Dictionary<Vector2, Cell> m_Cells;
    private TMP_InputField m_GenerationInputField;
    private TMP_InputField m_GridWidthInputField;
    private TMP_InputField m_GridHeightInputField;
    private TMP_Text m_ResultsText;
    private TMP_Text m_GenerationsText;

    [SerializeField] private GameObject m_ParametersPanel;
    [SerializeField] private GameObject m_ResultsPanel;
    [SerializeField] private GameObject m_GenerationsTextPro;

    [SerializeField] private Cell m_CellPrefab;

    [SerializeField] private Transform m_Player;

    [SerializeField] private GameObject m_StartSimulation;
    [SerializeField] private GameObject m_StopSimulation;
    [SerializeField] private GameObject m_RestartSimulation;
    [SerializeField] private GameObject m_BackButton;

    private void Start()
    {
        m_Generations = 10000;
        
        m_GenerationInputField = m_ParametersPanel.transform.GetChild(1).GetComponent<TMP_InputField>();
        m_GenerationInputField.onValueChanged.AddListener(delegate { OnGenerationInputFieldChange(); });
        
        m_GridWidthInputField = m_ParametersPanel.transform.GetChild(3).GetComponent<TMP_InputField>();
        m_GridWidthInputField.onValueChanged.AddListener(delegate { OnGridWithInputFieldChange(); });
        
        m_GridHeightInputField = m_ParametersPanel.transform.GetChild(5).GetComponent<TMP_InputField>();
        m_GridHeightInputField.onValueChanged.AddListener(delegate { OnGridHeightInputFieldChange(); });

        m_ResultsText = m_ResultsPanel.GetComponentInChildren<TMP_Text>();
        m_GenerationsText = m_GenerationsTextPro.GetComponent<TMP_Text>();
        
        m_StartSimulation.GetComponent<Button>().onClick.AddListener(delegate { OnStartRuntime(); });
        m_StopSimulation.GetComponent<Button>().onClick.AddListener(delegate { OnStopRuntime(); });
        m_RestartSimulation.GetComponent<Button>().onClick.AddListener(delegate { OnRestartRuntime(); });
        m_BackButton.GetComponent<Button>().onClick.AddListener(delegate { OnBack(); });

        m_State = PlayingState.ComputeChanges;
    }

    private void OnGenerationInputFieldChange()
    {
        m_Generations = Int32.Parse(m_GenerationInputField.text);
    }
    
    private void OnGridWithInputFieldChange()
    {
        m_Width = Int32.Parse(m_GridWidthInputField.text);
    }
    
    private void OnGridHeightInputFieldChange()
    {
        m_Height = Int32.Parse(m_GridHeightInputField.text);
    }

    public void GenerateGrid()
    {
        m_Cells = new Dictionary<Vector2, Cell>();

        for (int x = 0; x < m_Width; ++x)
        {
            for (int y = 0; y < m_Height; ++y)
            {
                var spawnedCell = Instantiate(m_CellPrefab, new Vector3(x, y), Quaternion.identity);
                spawnedCell.transform.localScale = new Vector3(0.96f, 0.96f, 0.96f);
                spawnedCell.name = $"Cell {x} {y}";

                m_Cells[new Vector2(x, y)] = spawnedCell;
            }
        }

        var halfX = m_Width / 2;
        var halfY = m_Height / 2;
        m_Player.transform.localPosition = new Vector3(halfX, halfY, 0);
    }

    public void OnBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGeneration()
    {
        GenerateGrid();

        m_ParametersPanel.SetActive(false);
        m_StartSimulation.SetActive(true);
        m_GenerationsTextPro.SetActive(true);
    }

    public void OnStartRuntime()
    {
        m_Playing = true;
        m_RuntimeGenerations = 0;

        m_StartSimulation.SetActive(false);
        m_StopSimulation.SetActive(true);
    }

    public void OnRestartRuntime()
    {
        m_Playing = false;
        m_State = PlayingState.ComputeChanges;
        m_RuntimeGenerations = 0;
        m_GenerationsText.text = "Gen: 0";
        
        m_ResultsPanel.SetActive(false);
        m_ParametersPanel.SetActive(true);
        m_GenerationsTextPro.SetActive(false);

        foreach (var cell in m_Cells)
        {
            Destroy(cell.Value.gameObject);
        }
        m_Cells.Clear();
    }

    public void OnStopRuntime()
    {
        m_Playing = false;
        
        m_StartSimulation.SetActive(true);
        m_StopSimulation.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (m_Playing)
        {
            switch (m_State)
            {
                case PlayingState.ComputeChanges:
                {
                    foreach (KeyValuePair<Vector2, Cell> entry in m_Cells)
                    {
                        var coords = entry.Value.GetCoords();
                        var vec2List = GetListOfCloseNeighboords((int)coords.x, (int)coords.y);
                        var neighboords = new List<Cell>();
                        foreach (Vector2 vec in vec2List)
                        {
                            neighboords.Add(m_Cells[vec]);
                        }

                        var actived = neighboords.FindAll(n => n.GetIsAlive());
                        if (!entry.Value.GetIsAlive() && actived.Count == 3)
                        {
                            entry.Value.SetWillBeAlive(true);
                        }

                        if (entry.Value.GetIsAlive() && (actived.Count == 2 || actived.Count == 3))
                        {
                            entry.Value.SetWillBeAlive(true);
                        }

                        m_State = PlayingState.ApplyChanges;
                    }
                } break;
                case PlayingState.ApplyChanges:
                {
                    bool changed = false;
                    foreach (KeyValuePair<Vector2, Cell> entry in m_Cells)
                    {
                        if (entry.Value.ApplyChanges())
                        {
                            changed = true;
                        }
                    }

                    m_RuntimeGenerations += 1;
                    m_GenerationsText.text = $"Gen: {m_RuntimeGenerations}";
                    if (!changed)
                    {
                        m_State = PlayingState.Lose;
                    } else if (m_RuntimeGenerations == m_Generations)
                    {
                        m_State = PlayingState.Win;
                    }
                    else
                    {
                        m_State = PlayingState.ComputeChanges;
                    }
                } break;
                case PlayingState.Lose:
                case PlayingState.Win:
                    m_ResultsText.text = m_State == PlayingState.Win ? "Has ganado!" : "Has perdido!";
                    m_ResultsText.color = m_State == PlayingState.Win
                        ? new Color(0.1843f, 0.7490f, 0.4431f, 1f)
                        : new Color(0.9372f, 0.1764f, 0.3372f, 1f);
                    m_ResultsPanel.SetActive(true);
                    m_StartSimulation.SetActive(false);
                    m_StopSimulation.SetActive(false);
                    break;
            }
        }
    }
    
    private List<Vector2> GetListOfCloseNeighboords(int x, int y)
    {
        List<Vector2> list = new List<Vector2>();

        // Upper left corner
        { var v = new Vector2(x - 1, y - 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        // Upper
        { var v = new Vector2(x, y - 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        // Upper right corner
        { var v = new Vector2(x + 1, y - 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        
        // Left
        { var v = new Vector2(x - 1, y); if (m_Cells.ContainsKey(v)) list.Add(v); }
        // Right
        { var v = new Vector2(x + 1, y); if (m_Cells.ContainsKey(v)) list.Add(v); }

        // Upper left corner
        { var v = new Vector2(x - 1, y + 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        // Upper
        { var v = new Vector2(x, y + 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        // Upper right corner
        { var v = new Vector2(x + 1, y + 1); if (m_Cells.ContainsKey(v)) list.Add(v); }
        
        return list;
    }
}