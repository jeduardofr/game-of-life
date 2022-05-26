using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    private Dictionary<Vector2, Cell> m_Cells;


    Dictionary<Vector2, Cell> GetCells()
    {
        return m_Cells;
    }
}