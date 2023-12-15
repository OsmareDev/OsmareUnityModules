using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Grid2D<T>
{
    private T[,] m_grid;
    public int NColumns {get; set;}
    public int NRows {get; set;}
    public float CellHeight {get; set;}
    public float CellWidth {get; set;}

    // decomposed to be able to save the data in a file
    public float OriginPositionX { get; set; }
    public float OriginPositionY { get; set; }
    public float OriginPositionZ { get; set; }
    public Vector3 OriginPosition { 
        get {return new Vector3(OriginPositionX, OriginPositionY, OriginPositionZ);} 
        set {
            OriginPositionX = value.x;
            OriginPositionY = value.y;
            OriginPositionZ = value.z;
        } }

    public Grid2D(int nColumns, int nRows, Vector2 cellSize, Vector3 originPosition) {
        this.NColumns = nColumns;
        this.NRows = nRows;
        this.CellWidth = cellSize.x;
        this.CellHeight = cellSize.y;
        this.OriginPositionX = originPosition.x;
        this.OriginPositionY = originPosition.y;
        this.OriginPositionZ = originPosition.z;

        m_grid = new T[NColumns, NRows];

        for (int i = 0; i < nColumns; ++i) {
            for (int j = 0; j < nRows; ++j) {
                m_grid[i, j] = Activator.CreateInstance<T>();
            }
        }
    }

    public T GetElement(int x, int y) {
        if (IsValidPosition(x, y)) return m_grid[x, y];
        else return default(T);
    }

    public void SetElement(int x, int y, T value) {
        if (IsValidPosition(x, y)) m_grid[x, y] = value;
        else Debug.LogError("Invalid grid position.");
    }

    public T this[int x, int y] {
        get { return GetElement(x, y); }
        set { SetElement(x, y, value); }
    }

    public Vector2Int WorldToGridCoordinates(Vector2 worldPosition) {
        float percentX = (worldPosition.x - OriginPositionX) / (NColumns * CellWidth);
        float percentY = (worldPosition.y - OriginPositionY) / (NRows * CellHeight);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(percentX * (NColumns));
        int y = Mathf.FloorToInt(percentY * (NRows));

        if (x == NColumns) x--;
        if (y == NRows) y--;

        return new Vector2Int(x, y);
    }

    public T GetElementFromWorldCoordinates(Vector3 worldPosition) {
        Vector2Int gridCoords = WorldToGridCoordinates(worldPosition);
        return m_grid[gridCoords.x, gridCoords.y];
    }

    public Vector2 GridToWorldCoordinates(Vector2 gridPosition) {
        float worldX = OriginPositionX + gridPosition.x * CellWidth + CellWidth / 2f;
        float worldY = OriginPositionY + gridPosition.y * CellHeight + CellHeight / 2f;
        return new Vector2(worldX, worldY);
    }

    public bool IsValidPosition(int x, int y) => x >= 0 && x < NColumns && y >= 0 && y < NRows;

    public bool LoadData(string docName) => SaveSystem.Load<T[,]>(docName, ref m_grid);
    public void SaveData(string docName) => SaveSystem.Save<T[,]>(docName, m_grid);

    public int GetLength(int i) => m_grid.GetLength(i);
    public bool ArrayHasBeenDeleted() => (m_grid == null);
}
