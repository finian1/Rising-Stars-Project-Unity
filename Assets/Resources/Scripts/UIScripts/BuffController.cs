using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffController : MonoBehaviour
{
    
    [SerializeField] private UI _ui;
    [Header("Level bars")]
    [SerializeField] private LevelBar lvlBar_Health;
    [SerializeField] private LevelBar lvlBar_Speed;
    [SerializeField] private LevelBar lvlBar_Jump;
    [SerializeField] private LevelBar lvlBar_CrystalDrop;
    [SerializeField] private LevelBar lvlBar_CrystalWorth;

    [Header("Cost button texts")]
    [SerializeField] private Text healthCostText;
    [SerializeField] private Text jumpCostText;
    [SerializeField] private Text speedCostText;
    [SerializeField] private Text crystalWorthCostText;
    [SerializeField] private Text crystalDropCostText;

    private void Update()
    {
        SetLevelText(ref healthCostText, PlayerStats.healthLevel);
        SetLevelText(ref speedCostText, PlayerStats.speedLevel);
        SetLevelText(ref jumpCostText, PlayerStats.jumpLevel);
        SetLevelText(ref crystalWorthCostText, PlayerStats.crystalWorthLevel);
        SetLevelText(ref crystalDropCostText, PlayerStats.crystalDropLevel);
    }
    

    public void PurchaseHealth()
    {
        AttemptBuff(ref PlayerStats.healthLevel);
        lvlBar_Health.val = PlayerStats.healthLevel;
    }
    public void PurchaseSpeed()
    {
        AttemptBuff(ref PlayerStats.speedLevel);
        lvlBar_Speed.val = PlayerStats.speedLevel;
    }
    public void PurchaseJump()
    {
        AttemptBuff(ref PlayerStats.jumpLevel);
        lvlBar_Jump.val = PlayerStats.jumpLevel;
    }
    public void PurchaseCrystalDrop()
    {
        AttemptBuff(ref PlayerStats.crystalDropLevel);
        lvlBar_CrystalDrop.val = PlayerStats.crystalDropLevel;
    }
    public void PurchaseCrystalWorth()
    {
        AttemptBuff(ref PlayerStats.crystalWorthLevel);
        lvlBar_CrystalWorth.val = PlayerStats.crystalWorthLevel;
    }
    private void AttemptBuff(ref int levelToBuff)
    {
        if (levelToBuff < PlayerStats.maxBuffLevel && PlayerStats.currency >= PlayerStats.GetBuffPrice(levelToBuff))
        {
            PlayerStats.currency -= PlayerStats.GetBuffPrice(levelToBuff);
            levelToBuff++;
        }
    }
    private void SetLevelText(ref Text textToSet, int level)
    {
        textToSet.text = PlayerStats.GetBuffPrice(level).ToString();
    }
    

    public void Done()
    {
        _ui.HideBuffs();
        _ui.ShowShop();
    }
}
