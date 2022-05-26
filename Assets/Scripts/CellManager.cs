using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private int m_Width, m_Height;

    [SerializeField] private Cell m_CellPrefab;

    [SerializeField] private Transform m_Player;
    
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
                spawnedCell.transform.localScale = new Vector3(0.96f, 0.96f, 0.96f);
                spawnedCell.name = $"Tile {x} {y}";
            }
        }

        var halfX = m_Width / 2;
        var halfY = m_Height / 2;
        m_Player.transform.localPosition = new Vector3(halfX, halfY, 0);
    }
}
