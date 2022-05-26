using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Material m_Material;
    private bool m_IsAlive;
    private bool m_WillBeAlive;

    private readonly Color m_OnEnterColor = new Color(0.1843f, 0.7490f, 0.4431f, 1f);
    private readonly Color m_OnExitColor = new Color(0.121568f, 0.160784f, 0.258823f, 1);

    private void Start()
    {
        var mesh = gameObject.GetComponent<MeshRenderer>();
        m_Material = mesh.material;
        m_IsAlive = false;
        m_WillBeAlive = false;
    }

    private void OnMouseEnter()
    {
        if (!m_IsAlive)
            m_Material.color = m_OnEnterColor;
    }

    private void OnMouseExit()
    {
        if (!m_IsAlive)
            m_Material.color = m_OnExitColor;
    }

    private void OnMouseDown()
    {
        SetIsAlive(!m_IsAlive);
    }

    public Vector2 GetCoords()
    {
        return new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
    }

    public void SetIsAlive(bool isAlive)
    {
        m_IsAlive = isAlive;
        m_Material.color = m_IsAlive ? m_OnEnterColor : m_OnExitColor;
    }

    public void SetWillBeAlive(bool willBeAlive)
    {
        m_WillBeAlive = willBeAlive;
    }

    public bool GetIsAlive()
    {
        return m_IsAlive;
    }

    public bool ApplyChanges()
    {
        bool changed = m_WillBeAlive != m_IsAlive;

        if (m_WillBeAlive)
        {
            SetIsAlive(true);
            m_WillBeAlive = false;
        }
        else
        {
            SetIsAlive(false);
        }

        return changed;
    }
}