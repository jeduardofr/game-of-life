using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private int m_Width, m_Height;

    [SerializeField] private Cell m_CellPrefab;

    [SerializeField] private Transform m_Camera;
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < m_Width; ++x)
        {
            for (int y = 0; y < m_Height; ++y)
            {
                var spawnedCell = Instantiate(m_CellPrefab, new Vector3(x, y), Quaternion.identity);
                spawnedCell.name = $"Tile {x} {y}";
            }
        }

        // m_Camera.transform.position = new Vector3((float)m_Width / 2 - 0.5f, (float)m_Width / 2 - 0.5f, 10);
    }
}
