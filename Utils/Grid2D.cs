using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T>
{
    private T[,] m_grid;
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private Vector3 originPosition;
    private Vector3 OriginPosition { get {return originPosition;} set {originPosition = value;}}

    public Grid2D(int width, int height, float cellSize, Vector3 originPosition) {
        this.m_width = width;
        this.m_height = height;
        this.m_cellSize = cellSize;
        this.originPosition = originPosition;

        m_grid = new T[width, height];
    }

    public T GetElement(int x, int y) {
        if (IsValidPosition(x, y)) return m_grid[x, y];
        else return default(T);
    }

    public void SetElement(int x, int y, T value) {
        if (IsValidPosition(x, y)) m_grid[x, y] = value;
        else Debug.LogError("Invalid grid position.");
    }

    public Vector2 WorldToGridCoordinates(Vector2 worldPosition) {
        float percentX = (worldPosition.x - originPosition.x) / (m_width * m_cellSize);
        float percentY = (worldPosition.y - originPosition.y) / (m_height * m_cellSize);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(percentX * m_width);
        int y = Mathf.FloorToInt(percentY * m_height);

        return new Vector2(x, y);
    }

    public Vector2 GridToWorldCoordinates(Vector2 gridPosition) {
        float worldX = originPosition.x + gridPosition.x * m_cellSize + m_cellSize / 2f;
        float worldY = originPosition.y + gridPosition.y * m_cellSize + m_cellSize / 2f;
        return new Vector2(worldX, worldY);
    }

    private bool IsValidPosition(int x, int y) => x >= 0 && x < m_width && y >= 0 && y < m_height;
}
