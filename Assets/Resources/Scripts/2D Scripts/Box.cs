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

    private float difficultyInc = 0.5f;

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


    public void Setup(int id, int row, int column, Board board)
    {
        ID = id;
        RowIndex = row;
        ColumnIndex = column;
        _board = board;
    }

    public void Charge(int dangerNearby, bool danger, Action<Box> onChange)
    {
        _changeCallback = onChange;
        DangerNearby = dangerNearby;
        IsDangerous = danger;
        ResetState();
    }

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

    public void Selected()
    {
        if (_board.gameStarted)
        {
            if (_button != null)
            {
                _button.interactable = false;
            }

            if (IsDangerous && Danger != null)
            {
                Danger.enabled = true;
            }
            else if (_textDisplay != null)
            {
                _textDisplay.enabled = true;
            }
            PlayerStats.difficulty += difficultyInc;
            _changeCallback?.Invoke(this);
        }
        else
        {
            
            _board.gameStarted = true;
            _board.BeginFPSPlay();
            _board.SetPlayerSpawnPoint(ID);
        }
    }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _textDisplay = GetComponentInChildren<TMP_Text>(true);
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Selected);

        ResetState();
    }

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
