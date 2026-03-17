using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
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
    private bool isVerticalDirectionWall = false;
    public bool IsVericalDirectionWall
    {
        get 
        {
            return isVerticalDirectionWall;
        }

        private set
        {
            isVerticalDirectionWall = value;
        }
    }

    public void SetupWall ( int row, int col, bool isVerticalDirection = false )
    {
        RowIndex = row;
        ColumnIndex = col;
        IsVericalDirectionWall = isVerticalDirection;
    }
}
