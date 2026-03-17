using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GridConfigData gridConfigData = null;

    [SerializeField]
    private GameObject gridCellPrefab = null;

    [SerializeField]
    private GameObject gridWallPrefab = null;

    [SerializeField]
    private Transform startPosition = null;

    [SerializeField]
    private Transform cellsContainer = null;

    [SerializeField]
    private Transform wallsContainer = null;

    public int RowMax
    {
        get
        {
            return gridConfigData.rows;
        }
    }

    public int ColumnMax
    {
        get
        {
            return gridConfigData.colunms;
        }
    }

    private List<GridCellController> gridList = new List<GridCellController>();

    private List<WallScript> wallList = new List<WallScript>();

    public void Start ()
    {
        EventsManager.GameOver += RemoveLevelSetup;
    }

    public void GenerateGrid ()
    {
        if ( gridConfigData != null && gridConfigData.rows > 3 && gridConfigData.colunms > 3)
        {
            for ( int i = 0 ; i < gridConfigData.rows ; i++ )
            {
                for ( int j = 0 ; j < gridConfigData.colunms ; j++ )
                {
                    // setup cell
                    GameObject cell = Instantiate ( gridCellPrefab, cellsContainer );
                    GridCellController gridCell = cell.GetComponent<GridCellController>();

                    bool isCorner = isCornerCell ( i, j );
                    bool isEdge = isEdgeCell ( i, j );
                    gridCell.SetupCell ( i, j, isEdge, isCorner );
                    Vector3 cellPos = startPosition.position;
                    cellPos.z += i;
                    cellPos.x += j;
                    gridCell.transform.position = cellPos;

                    gridList.Add ( gridCell );

                    // setup wall
                    generateWall ( gridCell.transform, i, j, isEdge, isCorner );
                }
            }
        }
    }

    private bool isCornerCell ( int rowIndex, int colIndex )
    {
        bool isCorner = false;

        if ( (rowIndex == 0 && colIndex == 0) 
            || (rowIndex + 1 == gridConfigData.rows && colIndex + 1 == gridConfigData.colunms) 
            || (rowIndex + 1 == gridConfigData.rows && colIndex == 0)
            || (rowIndex == 0 && colIndex + 1 == gridConfigData.colunms))
        {
            isCorner = true;
        }

        return isCorner;
    }

    private bool isEdgeCell ( int rowIndex, int colIndex )
    {
        bool isEdge = false;

        if (rowIndex == 0 
            || colIndex == 0
            || rowIndex + 1 == gridConfigData.rows
            || colIndex + 1 == gridConfigData.colunms) 
        {
            isEdge = true;
        }

        return isEdge;
    }

    private void generateWall ( Transform gridCell, int row, int column, bool isEdge, bool isCorner )
    {
        if ( isEdge )
        {
            GameObject wallObj1 = Instantiate (gridWallPrefab, wallsContainer);
            WallScript wall1 = wallObj1.GetComponent<WallScript>();
            Vector3 wallPos1 = gridCell.position;

            if ( row == 0 || row + 1 == gridConfigData.rows )
            {
                wallPos1.z += (row == 0) ? -0.5f : 0.5f;
                wall1.transform.position = wallPos1;
                wall1.SetupWall ( row, column, true );
                wallList.Add ( wall1 );

                if ( isCorner )
                {
                    Vector3 wallPos2 = gridCell.position;
                    GameObject wallObj2 = Instantiate (gridWallPrefab, wallsContainer);
                    WallScript wall2 = wallObj2.GetComponent<WallScript>();
                    wall2.transform.eulerAngles = new Vector3 ( 0, 90, 0);
                    wallPos2.x += (column == 0) ? -0.5f : 0.5f;
                    wall2.transform.position = wallPos2;
                    wall2.SetupWall ( row, column, false );
                    wallList.Add ( wall2 );
                }
            }
            else if ( column == 0 || column + 1 == gridConfigData.colunms )
            {
                wall1.transform.eulerAngles = new Vector3 ( 0, 90, 0);
                wallPos1.x += (column == 0) ? -0.5f : 0.5f;
                wall1.transform.position = wallPos1;
                wall1.SetupWall ( row, column, false );
                wallList.Add ( wall1 );
            }
        }
    }

    public void SpanwObjectAtGridIndex ( GameObject gridObject, int row, int col )
    {
        if ( gridList != null && row > -1 && col > -1 )
        {
            Vector3 objPosition = gridList.Find ( x => x.RowIndex == row && x.ColumnIndex == col ).transform.position;
            objPosition.y += 1;
            gridObject.transform.position = objPosition;
        }
    }

    public Transform GetCellAtIndex (int row, int col)
    {
        Transform cellTransform = null;

        if ( gridList != null && row > -1 && col > -1 )
        {
            cellTransform = gridList.Find ( x => x.RowIndex == row && x.ColumnIndex == col ).transform;
        }

        return cellTransform;
    }

    public void RemoveLevelSetup ()
    {
        if ( gridList != null && wallList != null )
        {
            while ( gridList.Count > 0 )
            {
                GameObject cell = gridList [0].gameObject;
                gridList.RemoveAt ( 0 );

                Destroy (cell);
            }

            while ( wallList.Count > 0 )
            {
                GameObject wall = wallList [0].gameObject;
                wallList.RemoveAt ( 0 );
                
                Destroy (wall);
            }
        }
    }
}
