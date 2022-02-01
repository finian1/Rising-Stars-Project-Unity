using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Box : MonoBehaviour
{
    [SerializeField] private Color[] DangerColors = new Color[8];
    [SerializeField] private Image Danger;

    private TMP_Text _textDisplay;
    private Button _button;
    private Action<Box> _changeCallback;
    private Board _board;
    

    public int RowIndex { get; private set; }
    public int ColumnIndex { get; private set; }
    public int ID { get; private set; }
    public int DangerNearby { get; private set; }
    public bool IsDangerous { get; private set; }
    public bool IsActive { get { return _button != null && _button.interactable; } }

    private float difficultyInc = 0.1f;

    private void OnDrawGizmos()
    {
        if (_board != null && _board.gameStarted)
        {
            if(ID == 0)
            {
                Gizmos.color = Color.green;
            }
            //Gizmos.DrawSphere(_board.chunkController.GetCellPosition(ID), 1);
        }
    }

    /// <summary>
    /// Initializing variables
    /// </summary>
    /// <param name="id"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="board"></param>
    public void Setup(int id, int row, int column, Board board)
    {
        ID = id;
        RowIndex = row;
        ColumnIndex = column;
        _board = board;
    }
    /// <summary>
    /// Sets up the box for the round
    /// </summary>
    /// <param name="dangerNearby"></param>
    /// <param name="danger"></param>
    /// <param name="onChange"></param>
    public void Charge(int dangerNearby, bool danger, Action<Box> onChange)
    {
        _changeCallback = onChange;
        DangerNearby = dangerNearby;
        IsDangerous = danger;
        ResetState();
    }
    /// <summary>
    /// Reveals box with text
    /// </summary>
    public void Reveal()
    {
        if (_button != null)
        {
            _button.interactable = false;
        }

        if (_textDisplay != null)
        {
            _textDisplay.enabled = true;
        }
    }
    /// <summary>
    /// Disable box
    /// </summary>
    public void StandDown()
    {
        if (_button != null)
        {
            _button.interactable = false;
        }

        if (Danger != null)
        {
            Danger.enabled = false;
        }

        if (_textDisplay != null)
        {
            _textDisplay.enabled = false;
        }
    }

    //public void OnClick()
    //{
    //    if(_button != null)
    //    {
    //        _button.interactable = false;
    //    }

    //    if(IsDangerous && Danger != null)
    //    {
    //        Danger.enabled = true;
    //    }
    //    else if(_textDisplay != null)
    //    {
    //        _textDisplay.enabled = true;
    //    }

    //    _changeCallback?.Invoke(this);
    //}

    /// <summary>
    /// Triggers when the box is selected via any means
    /// </summary>
    public void Selected()
    {
        //If a game is already in progress
        if (_board.gameStarted)
        {
            if (_button != null)
            {
                //Disable the button
                _button.interactable = false;
            }
            //If the box is dangerous, enable the "danger" image
            if (IsDangerous && Danger != null)
            {

                Danger.enabled = true;
            }
            //Otherwise display the text image
            else if (_textDisplay != null)
            {
                _textDisplay.enabled = true;
            }
            //Increase the current player difficulty
            PlayerStats.difficulty += difficultyInc;
            _changeCallback?.Invoke(this);
        }
        //If a game is not in progress
        else
        {
            //Prevent the player from clicking on a dangerous cell
            if (IsDangerous)
            {
                IsDangerous = false;
            }
            //Hide the board and spawn instructions, and set game started to true
            _board._ui.HideBoard();
            _board._ui.HideSpawnInstructions();
            _board.gameStarted = true;
            
            _board.BeginFPSPlay();

            //Set the player's spawn point onto the given cell
            _board.SetPlayerSpawnPoint(ID);

        }
    }

    private void Awake()
    {
        Initialize();
    }
    /// <summary>
    /// Initializes text and button items
    /// </summary>
    public void Initialize()
    {
        _textDisplay = GetComponentInChildren<TMP_Text>(true);
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Selected);

        ResetState();
    }
    /// <summary>
    /// Resets the box state to empty and not clicked
    /// </summary>
    private void ResetState()
    {
        if (Danger != null)
        {
            Danger.enabled = false;
        }

        if (_textDisplay != null)
        {
            if (DangerNearby > 0)
            {
                _textDisplay.text = DangerNearby.ToString("D");
                _textDisplay.color = DangerColors[DangerNearby-1];
            }
            else
            {
                _textDisplay.text = string.Empty;
            }

            _textDisplay.enabled = false;
        }

        if (_button != null)
        {
            _button.interactable = true;
        }
    }
}
