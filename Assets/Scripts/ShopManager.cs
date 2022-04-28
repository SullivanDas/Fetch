using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IInteractable
{
    [SerializeField] private int strengthIncreaseAmount = 1;
    [SerializeField] private float speedIncreaseAmount = 0.1f;
    [SerializeField] private int invSizeIncreaseAmount = 2;
    [SerializeField] private int xpIncreaseAmount;
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private CanvasGroup hudGroup;
    
    public PlayerController PlayerRef { get; set; }

    [System.Serializable]
    public class ShopItem 
    {
        public UnityEvent function;
        public int Cost;
        public int Stock;
    }

    [SerializeField] private List<ShopItem> items = new List<ShopItem>();
    [SerializeField] private List<Button> buttons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        DisableMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeMenu(PlayerController player)
    {
        PlayerRef = player;
        uiGroup.alpha = 1;
        uiGroup.interactable = true;
        uiGroup.blocksRaycasts = true;

        hudGroup.alpha = 0;
        hudGroup.interactable = false;
        hudGroup.blocksRaycasts = false;

        CheckCosts();
    }

    private void CheckCosts()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (PlayerRef.Money < items[i].Cost || items[i].Stock <= 0)
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }
        }

    }

    public void BuyItem(int item)
    {
        if(item < items.Count)
        {
            ShopItem selected = items[item];
            if(PlayerRef.Money >= selected.Cost)
            {
                PlayerRef.Money -= selected.Cost;
                selected.Stock--;
                items[item] = selected;
                if(selected.Stock <= 0)
                {
                    buttons[item].interactable = false;
                }
                CheckCosts();
                selected.function.Invoke();
            }
        }
    }

    public void DisableMenu()
    {
        uiGroup.alpha = 0;
        uiGroup.interactable = false;
        uiGroup.blocksRaycasts = false;

        hudGroup.alpha = 1;
        hudGroup.interactable = true;
        hudGroup.blocksRaycasts = true;
    }

    public void StrengthUp()
    {
        if (PlayerRef)
        {
            PlayerRef.Damage += strengthIncreaseAmount;
        }
    }

    public void SpeedUp()
    {
        if (PlayerRef)
        {
            PlayerRef.MaxSpeed += speedIncreaseAmount;
        }
    }

    public void GiveNet()
    {
        if (PlayerRef)
        {
            PlayerRef.HasEquipment[(int)PlayerController.Equipment.Net] = true;
        }
    }

    public void IncreaseInventorySize()
    {
        if (PlayerRef)
        {
            PlayerRef.InventorySize += invSizeIncreaseAmount;
        }
    }

    public void GiveXP()
    {
        PlayerRef.XP += xpIncreaseAmount;
    }

    public void WaterModifierUp()
    {
        PlayerRef.WaterModifier = 1;
    }

    public bool IsCurrentlyInteractable(GameObject interactor)
    {
        PlayerController player = interactor.GetComponent<PlayerController>();
        
        return player != null;
    }

    public void Interact(GameObject interactor)
    {
        if (IsCurrentlyInteractable(interactor))
        {
            InitializeMenu(interactor.GetComponent<PlayerController>());
        }
    }
}
