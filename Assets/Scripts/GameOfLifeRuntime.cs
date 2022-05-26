using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum UIState
{
    Initial,
    OpenMenu,
}

enum PlayingState
{
    Update,
    ComputeChanges,
    ApplyChanges
}

public class GameOfLifeRuntime : MonoBehaviour
{
    private bool m_Playing;
    private int m_Width;
    private int m_Height;
    private int m_Generations;
    private Dictionary<Vector2, Cell> m_Cells;
    private TMP_InputField m_GenerationInputField;
    private TMP_InputField m_GridWidthInputField;
    private TMP_InputField m_GridHeightInputField;

    [SerializeField] private GameObject m_ParametersPanel;

    [SerializeField] private Cell m_CellPrefab;

    [SerializeField] private Transform m_Player;

    private void Start()
    {
        m_Generations = 10000;
        
        m_GenerationInputField = m_ParametersPanel.transform.GetChild(1).GetComponent<TMP_InputField>();
        m_GenerationInputField.onValueChanged.AddListener(delegate { OnGenerationInputFieldChange(); });
        
        m_GridWidthInputField = m_ParametersPanel.transform.GetChild(3).GetComponent<TMP_InputField>();
        m_GridWidthInputField.onValueChanged.AddListener(delegate { OnGridWithInputFieldChange(); });
        
        m_GridHeightInputField = m_ParametersPanel.transform.GetChild(5).GetComponent<TMP_InputField>();
        m_GridHeightInputField.onValueChanged.AddListener(delegate { OnGridHeightInputFieldChange(); });
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
                spawnedCell.name = $"Tile {x} {y}";

                m_Cells[new Vector2(x, y)] = spawnedCell;
            }
        }

        var halfX = m_Width / 2;
        var halfY = m_Height / 2;
        m_Player.transform.localPosition = new Vector3(halfX, halfY, 0);
    }

    public void StartGeneration()
    {
        m_Width = 80;
        m_Height = 80;

        GenerateGrid();

        m_ParametersPanel.SetActive(false);
    }

    public void StartRuntime()
    {
        m_Playing = true;
    }

    public void StopRuntime()
    {
        m_Playing = false;
    }
}