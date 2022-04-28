using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public enum Equipment { Sword, Net }

    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private float swingRadius = 1f;
    [SerializeField] private float swingRange = 2f;
    [SerializeField] private float swingDelay = 0.5f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private InputActionReference netAction;
    [SerializeField] private TextMeshProUGUI gemUI;
    [SerializeField] private TextMeshProUGUI xpUI;
    [SerializeField] private Sprite blankInv;
    [SerializeField] private Sprite blockedInv;

    [SerializeField] private List<Image> invUI = new List<Image>();
    [SerializeField] private int _inventorySize = 4;
    public int InventorySize { get { return _inventorySize; } set { _inventorySize = value; UpdateInvUI(); } }

    [SerializeField] private int _damage = 1;
    public int Damage { get { return _damage; } set { _damage = value; } }

    public int CurrentInventoryAmount { get; set; }

    private int _money;
    public int Money { get { return _money; } set { _money = value; gemUI.text = "Gems: " + _money; } }

    private int _xp;
    public int XP { get { return _xp; } set { _xp = value; xpUI.text = "XP: " + _xp; } }
    [SerializeField] private float _speed = 5f;
    public float Speed { get { return _speed; } set { _speed = value; } }
    private float _maxSpeed;
    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; Speed = value; } }
    public float WaterModifier { get; set; }

    private bool canSwing = true;
    private Vector2 moveDir = Vector2.zero;
    private Vector2 dir;
    private Rigidbody2D rb;
    private Animator animator;

    public List<Collectable> Inventory { get; set; }

    public bool[] HasEquipment { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        WaterModifier = 2;
        HasEquipment = new bool[2];
        Inventory = new List<Collectable>();
        CurrentInventoryAmount = 0;
        moveAction.action.Enable();
        moveAction.action.performed += HandleMove;
        moveAction.action.canceled += HandleMove;
        interactAction.action.Enable();
        interactAction.action.performed += Interact;
        attackAction.action.Enable();
        attackAction.action.performed += Attack;
        netAction.action.Enable();
        netAction.action.performed += SwingNet;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        MaxSpeed = Speed;

        UpdateInvUI();
    }

    private void OnDestroy()
    {
        moveAction.action.Disable();
        moveAction.action.performed -= HandleMove;
        moveAction.action.canceled -= HandleMove;
        interactAction.action.Disable();
        interactAction.action.performed -= Interact;
        attackAction.action.Disable();
        attackAction.action.performed -= Attack;
        netAction.action.Disable();
        netAction.action.performed -= SwingNet;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb)
        {
            rb.MovePosition((Vector2)transform.position + moveDir * Speed);
        }
    }

    public void AddInventoryItem(Collectable collectable)
    {
        Inventory.Add(collectable);
        CurrentInventoryAmount++;
        UpdateInvUI();
    }

    public void RemoveInventoryItem(Collectable collectable)
    {

        Inventory.Remove(collectable);
        CurrentInventoryAmount--;
        UpdateInvUI();

    }

    public void RemoveInventoryItem(int index)
    {
        if(index < Inventory.Count)
        {
            Inventory.RemoveAt(index);
            CurrentInventoryAmount--;
            UpdateInvUI();

        }
    }

    public void AddEquipment(Equipment equipment)
    {
        HasEquipment[(int)equipment] = true;
        Debug.Log("Equipment " + equipment.ToString() + " " + (int)equipment);
        if(HasEquipment[(int)Equipment.Sword])
        {
            Debug.Log("has sword");
            animator.SetBool("hasSword", true);
        }
    }

    private void UpdateInvUI()
    {
        for(int i = 0; i < invUI.Count; i++)
        {
            if(i < Inventory.Count)
            {
                invUI[i].sprite = Inventory[i].GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else if(i < InventorySize)
            {
                invUI[i].sprite = blankInv;
            }
            else
            {
                invUI[i].sprite = blockedInv;
            }
        }
    }
    private void HandleMove(InputAction.CallbackContext context)
    {

        moveDir = context.ReadValue<Vector2>();
        if( moveDir != Vector2.zero)
        {
            dir = moveDir;
            transform.rotation = Quaternion.LookRotation(transform.forward, -moveDir);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (HasEquipment[(int)Equipment.Sword] && canSwing)
        {
            animator.SetTrigger("attack");
            GetComponent<AudioSource>().Play();
            canSwing = false;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, swingRadius, dir, swingRange);
            foreach (RaycastHit2D hit in hits)
            {
                IChangeableHealth changeable = hit.transform.gameObject.GetComponent<IChangeableHealth>();
                if (changeable != null)
                {
                    changeable.ChangeHealth(-Damage);
                }
            }
            StartCoroutine(SwingDelay());
        }
    }

    private void SwingNet(InputAction.CallbackContext context)
    {
        if (HasEquipment[(int)Equipment.Net])
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, swingRadius, dir, swingRange);
            foreach (RaycastHit2D hit in hits)
            {
                Collectable collectable = hit.transform.gameObject.GetComponent<Collectable>();
                if(hit.transform.gameObject.tag == "Catchable" && collectable != null )
                {
                    collectable.Interact(gameObject);
                    break;
                }
            }
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, interactionDistance);

        foreach(RaycastHit2D hit in hits)
        {
            IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
            if(interactable != null)
            {
                if (interactable.IsCurrentlyInteractable(gameObject))
                {
                    interactable.Interact(gameObject);
                }
                
            }
        }
    }

    private IEnumerator SwingDelay()
    {
        yield return new WaitForSeconds(swingDelay);
        canSwing = true;
    }
}
