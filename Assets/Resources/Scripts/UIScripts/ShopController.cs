using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UI _ui;
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private Text statsText;
    [SerializeField] private GameObject listContentObject;
    [SerializeField] private ScrollRect shopList;
    [SerializeField] private Text crystalText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Dropdown primaryWeaponSelection;
    [SerializeField] private Dropdown secondaryWeaponSelection;
    private WeaponStatHolderBase currentSelectedWeapon;
    private List<GameObject> currentButtonList;

    void Start()
    {
        RefreshList();
        UpdateCrystalCount();
    }

    public void RefreshList()
    {
        int numOfButtons = 0;
        int wepNum = 0;
        float buttonSize = 150;

        if(currentButtonList != null)
        {
            foreach(GameObject button in currentButtonList)
            {
                Destroy(button);
            }
        }
        currentButtonList = new List<GameObject>();

        foreach (WeaponStatHolderBase weapon in ShopContainer.weaponsForSale)
        {
            if (!PlayerStats.weaponsOwned.Contains(weapon))
            {
                GameObject temp = Instantiate(buttonTemplate, listContentObject.transform);
                currentButtonList.Add(temp);

                temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, -numOfButtons * buttonSize - (buttonSize / 2), temp.transform.localPosition.z);
                temp.transform.GetChild(0).gameObject.GetComponent<Text>().text = weapon.weaponNickname;


                int tempInt = wepNum;
                temp.GetComponent<Button>().onClick.AddListener(delegate { SelectWeapon(tempInt); });
                numOfButtons++;
                wepNum++;
            }
            else
            {

            }
        }
        primaryWeaponSelection.options.Clear();
        secondaryWeaponSelection.options.Clear();
        foreach (WeaponStatHolderBase weapon in PlayerStats.weaponsOwned)
        {
            primaryWeaponSelection.options.Add(new Dropdown.OptionData(weapon.weaponNickname));
            secondaryWeaponSelection.options.Add(new Dropdown.OptionData(weapon.weaponNickname));
        }


        if (PlayerStats.primaryWeapon != null)
        {
            primaryWeaponSelection.value = PlayerStats.weaponsOwned.IndexOf(PlayerStats.primaryWeapon);
        }
        if (PlayerStats.secondaryWeapon != null)
        {
            secondaryWeaponSelection.value = PlayerStats.weaponsOwned.IndexOf(PlayerStats.secondaryWeapon);
        }
    }

    public void EquipPrimaryWeapon(int index)
    {
        PlayerStats.primaryWeapon = PlayerStats.weaponsOwned[index];
        Debug.Log("Equipped Primary " + PlayerStats.primaryWeapon.weaponNickname);
    }
    public void EquipSecondaryWeapon(int index)
    {
        PlayerStats.secondaryWeapon = PlayerStats.weaponsOwned[index];
        Debug.Log("Equipped Secondary " + PlayerStats.secondaryWeapon.weaponNickname);
    }

    public void SwitchedTo()
    {
        statsText.text = "Welcome to the shop!";
        RefreshList();
        UpdateCrystalCount();
    }

    public void PurchaseWeapon()
    {
        if(currentSelectedWeapon != null && currentSelectedWeapon.weaponValue <= PlayerStats.currency)
        {
            ShopContainer.weaponsForSale.Remove(currentSelectedWeapon);
            PlayerStats.weaponsOwned.Add(currentSelectedWeapon);
            PlayerStats.currency -= currentSelectedWeapon.weaponValue;
            currentSelectedWeapon = null;
            statsText.text = "Purchased!";
            RefreshList();
            UpdateCrystalCount();
        }
    }

    public void UpdateCrystalCount()
    {
        crystalText.text = "Crystals: " + PlayerStats.currency;
    }

    void SelectWeapon(int id)
    {
        Debug.Log(id);
        currentSelectedWeapon = ShopContainer.weaponsForSale[id];
        if (currentSelectedWeapon != null) {
            statsText.text = "Price: " + currentSelectedWeapon.weaponValue + "\n"
                + '"' + currentSelectedWeapon.weaponNickname + '"' + "\n";
        }

        if(currentSelectedWeapon.weaponValue > PlayerStats.currency)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public void Done()
    {
        _ui.HideShop();
        _ui.ShowBoard();
        _ui.ShowMenu();
    }
}
