using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellController : MonoBehaviour
{
    [SerializeField]
    private int rowIndex = -1;
    public int RowIndex
    {
        get
        {
            return rowIndex;
        }

        private set
        {
            int temp = value;

            if ( temp > -1 )
            {
                rowIndex = temp;
            }
        }
    }

    [SerializeField]
    private int columnIndex = -1;
    public int ColumnIndex
    {
        get
        {
            return columnIndex;
        }

        private set
        {
            int temp = value;

            if ( temp > -1 )
            {
                columnIndex = temp;
            }
        }
    }

    [SerializeField]
    private bool isCornerCell = false;
    public bool IsCornerCell
    {
        get
        {
            return isCornerCell;
        }

        private set
        {
            isCornerCell = value;
        }
    }

    [SerializeField]
    private bool isEdgeCell = false;
    public bool IsEdgeCell
    {
        get
        {
            return isEdgeCell;
        }
        
        private set
        {
            isEdgeCell = value;
        }
    }

    public void SetupCell ( int row, int column, bool isEdge, bool isCorner )
    {
        RowIndex = row;
        ColumnIndex = column;
        IsEdgeCell = isEdge;
        IsCornerCell = isCorner;
    }
}
