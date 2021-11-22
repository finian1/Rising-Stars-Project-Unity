using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum Event { ClickedBlank, ClickedNearDanger, ClickedDanger, Win };

    [SerializeField] private Box BoxPrefab;
    [SerializeField] private int Width = 10;
    [SerializeField] private int Height = 10;
    [SerializeField] private int NumberOfDangerousBoxes = 10;

    //Grid is stored in a 1D array
    private Box[] _grid;
    private Vector2Int[] _neighbours;
    private RectTransform _rect;
    private Action<Event> _clickEvent;
    private Vector3 boardTopRightPosition;
    private Vector3 boardBottomLeftPosition;

    public void Setup(Action<Event> onClickEvent)
    {
        _clickEvent = onClickEvent;
        Clear();
    }

    public void Clear()
    {
        for (int row = 0; row < Height; ++row)
        {
            for (int column = 0; column < Width; ++column)
            {
                int index = row * Width + column;
                _grid[index].StandDown();
            }
        }
    }

    public void RechargeBoxes()
    { 
        //Get the number of items on the board and create a boolean list containing if the item is dangerous or not
        int numberOfItems = Width * Height;
        List<bool> dangerList = new List<bool>(numberOfItems);

        for (int count = 0; count < numberOfItems; ++count)
        {
            //Add a boolean to the list if the current count is lower than the number of dangerous boxes
            dangerList.Add(count < NumberOfDangerousBoxes);
        }

        //Shuffle the danger list so that the order is randomised
        dangerList.RandomShuffle();

        //For each row and column of the grid
        for (int row = 0; row < Height; ++row)
        {
            for (int column = 0; column < Width; ++column)
            {
                //Get the index current cell within the "dangerList"
                int index = row * Width + column;
                //Populate and reset the cell
                _grid[index].Charge(CountDangerNearby(dangerList, index), dangerList[index], OnClickedBox);
            }
        }
    }

    private void Awake()
    {
        //Create a grid of boxes
        _grid = new Box[Width * Height];

        //Do a bunch of transform stuff to find the start position of the grid
        _rect = transform as RectTransform;
        RectTransform boxRect = BoxPrefab.transform as RectTransform;
        _rect.sizeDelta = new Vector2(boxRect.sizeDelta.x * Width, boxRect.sizeDelta.y * Height);
        Vector2 startPosition = _rect.anchoredPosition - (_rect.sizeDelta * 0.5f) + (boxRect.sizeDelta * 0.5f);
        startPosition.y *= -1.0f;

        //Neighbour mask to help with finding all of the neighbours to a cell
        //Numbers look odd because it is working with a 1D array, so to find the upp and lower neighbours it must add/subtract a row
        _neighbours = new Vector2Int[8]
        {
            new Vector2Int(-Width - 1, -1),
            new Vector2Int(-Width, -1),
            new Vector2Int(-Width + 1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(Width - 1, 1),
            new Vector2Int(Width, 1 ),
            new Vector2Int(Width + 1, 1)
        };

        for (int row = 0; row < Width; ++row)
        {
            GameObject rowObj = new GameObject(string.Format("Row{0}", row), typeof(RectTransform));
            RectTransform rowRect = rowObj.transform as RectTransform;
            rowRect.SetParent(transform);
            rowRect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, startPosition.y - (boxRect.sizeDelta.y * row));
            rowRect.sizeDelta = new Vector2(boxRect.sizeDelta.x * Width, boxRect.sizeDelta.y);
            rowRect.localScale = Vector2.one;

            for (int column = 0; column < Height; ++column)
            {
                int index = row * Width + column;
                _grid[index] = Instantiate(BoxPrefab, rowObj.transform);
                _grid[index].Setup(index, row, column);
                RectTransform gridBoxTransform = _grid[index].transform as RectTransform;
                _grid[index].name = string.Format("ID{0}, Row{1}, Column{2}", index, row, column);
                gridBoxTransform.anchoredPosition = new Vector2( startPosition.x + (boxRect.sizeDelta.x * column), 0.0f);
            }
        }

        boardTopRightPosition = new Vector3(_grid[Width - 1].transform.position.x + (boxRect.sizeDelta.x*transform.parent.lossyScale.x)/2, _grid[Width - 1].transform.position.y + (boxRect.sizeDelta.x * transform.parent.lossyScale.y)/2, 0.0f);
        boardBottomLeftPosition = new Vector3(_grid[Width * Height - Width].transform.position.x - (boxRect.sizeDelta.x * transform.parent.lossyScale.x) / 2, _grid[Width * Height - Width].transform.position.y - (boxRect.sizeDelta.x * transform.parent.lossyScale.y) / 2, 0.0f);



        // Sanity check... Failed
        for(int count = 0; count < _grid.Length; ++count)
        {
            Debug.LogFormat("Count: {0}  ID: {1}  Row: {2}  Column: {3}", count, _grid[count].ID, _grid[count].RowIndex, _grid[count].ColumnIndex);
        }
    }

    private int CountDangerNearby(List<bool> danger, int index)
    {
        int result = 0;
        int boxRow = index / Width;

        if (!danger[index])
        {
            for (int count = 0; count < _neighbours.Length; ++count)
            {
                int neighbourIndex = index + _neighbours[count].x;
                int expectedRow = boxRow + _neighbours[count].y;
                int neighbourRow = neighbourIndex / Width;
                if(expectedRow == neighbourRow && neighbourIndex >= 0 && neighbourIndex < danger.Count && danger[neighbourIndex])
                {
                    result++;
                }
                //result += (expectedRow == neighbourRow && neighbourIndex >= 0 && neighbourIndex < danger.Count && danger[neighbourIndex]) ? 1 : 0;
            }
        }

        return result;
    }

    private void OnClickedBox(Box box)
    {
        Event clickEvent = Event.ClickedBlank;

        if(box.IsDangerous)
        {
            clickEvent = Event.ClickedDanger;
        }
        else if(box.DangerNearby > 0)
        {
            clickEvent = Event.ClickedNearDanger;
        }
        else
        {
            ClearNearbyBlanks(box);
        }

        if(CheckForWin())
        {
            clickEvent = Event.Win;
        }

        _clickEvent?.Invoke(clickEvent);
    }

    private bool CheckForWin()
    {
        bool Result = true;

        for( int count = 0; Result && count < _grid.Length; ++count)
        {
            if(!_grid[count].IsDangerous && _grid[count].IsActive)
            {
                Result = false;
            }
        }

        return Result;
    }

    private void ClearNearbyBlanks(Box box)
    {
        RecursiveClearBlanks(box);
    }

    private void RecursiveClearBlanks(Box box)
    {
        if (!box.IsDangerous)
        {
            box.Reveal();

            if (box.DangerNearby == 0)
            {
                for (int count = 0; count < _neighbours.Length; ++count)
                {
                    int neighbourIndex = box.ID + _neighbours[count].x;
                    int expectedRow = box.RowIndex + _neighbours[count].y;
                    int neighbourRow = neighbourIndex / Width;
                    bool correctRow = expectedRow == neighbourRow;
                    bool active = neighbourIndex >= 0 && neighbourIndex < _grid.Length && _grid[neighbourIndex].IsActive;

                    if (correctRow && active)
                    {
                        RecursiveClearBlanks(_grid[neighbourIndex]);
                    }
                }
            }
        }
    }

    public int GetWidth()
    {
        return Width;
    }
    public int GetHeight()
    {
        return Height;
    }

    public bool CheckIfDanger(int index)
    {
        return _grid[index].IsDangerous;
    }
}

static public class RectTransformExt
{
    /// <summary>
    /// Converts RectTransform.rect's local coordinates to world space
    /// Usage example RectTransformExt.GetWorldRect(myRect, Vector2.one);
    /// </summary>
    /// <returns>The world rect.</returns>
    /// <param name="rt">RectangleTransform we want to convert to world coordinates.</param>
    /// <param name="scale">Optional scale pulled from the CanvasScaler. Default to using Vector2.one.</param>
    static public Rect GetWorldRect(RectTransform rt, Vector2 scale)
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }
}
