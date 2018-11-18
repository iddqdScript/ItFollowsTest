using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Private Members

    private Animator _animator;

    private Rigidbody _rigidbody;

    private CharacterController _characterController;

    public InventoryItemBase TESTGATHER;

    private float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private InventoryItemBase mCurrentItem = null;

    private HealthBar mHealthBar;

    private HealthBar mFoodBar;

    private WCLevel mWCLevel;

    private int startHealth;

    private int startLevel;

    private int startFood;



 

    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public Inventory Inventory;

    public GameObject Hand;

    public HUD Hud;

    public float JumpSpeed = 7.0f;

    #endregion

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        Inventory.ItemUsed += Inventory_ItemUsed;
        Inventory.ItemRemoved += Inventory_ItemRemoved;

        mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Min = 0;
        mHealthBar.Max = Health;
        startHealth = Health;
        mHealthBar.SetValue(Health);

        mWCLevel = Hud.transform.Find("WoodcuttingSkill").GetComponent<WCLevel>();
        mWCLevel.Min = 1;
        mWCLevel.Max = 99;
        startLevel = Level;
        mWCLevel.SetValue(12);

        mFoodBar = Hud.transform.Find("Bars_Panel/FoodBar").GetComponent<HealthBar>();
        mFoodBar.Min = 0;
        mFoodBar.Max = Food;
        startFood = Food;
        mFoodBar.SetValue(Food);

        InvokeRepeating("IncreaseHunger", 0, HungerRate);
    }

    #region Inventory

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
        goItem.transform.parent = null;

    }

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        Debug.Log("Picked up " + item);
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? Hand.transform : null;
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.Item.ItemType != EItemType.Consumable)
        {
            // If the player carries an item, un-use it (remove from player's hand)
            if (mCurrentItem != null)
            {
                SetItemActive(mCurrentItem, false);
            }

            InventoryItemBase item = e.Item;

            // Use item (put it to hand of the player)
            SetItemActive(item, true);

            mCurrentItem = e.Item;
        }

    }

    private int Attack_1_Hash = Animator.StringToHash("Base Layer.Attack_1");

    public bool IsAttacking
    {
        get
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.fullPathHash == Attack_1_Hash)
            {
                //Debug.Log("Not attacking");
                return true;
            }
            //Debug.Log("Not attacking");
            return false;
           
        }
    }

    public void DropCurrentItem()
    {
        _animator.SetTrigger("tr_drop");

        GameObject goItem = (mCurrentItem as MonoBehaviour).gameObject;

        Inventory.RemoveItem(mCurrentItem);

        // Throw animation
        // Throw animation
        Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
        if (rbItem != null)
        {
            rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

            Invoke("DoDropItem", 0.25f);
        }

    }

    public void DoDropItem()
    {

        // Remove Rigidbody
        Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

        mCurrentItem = null;
    }

    #endregion

    #region Health & Hunger

    [Tooltip("Amount of health")]
    public int Health = 100;

    [Tooltip("Amount of food")]
    public int Food = 100;

    public int Level = 1;

    [Tooltip("Rate in seconds in which the hunger increases")]
    public float HungerRate = 0.5f;

    public void IncreaseHunger()
    {
        Food--;
        if (Food < 0)
            Food = 0;

        mFoodBar.SetValue(Food);

        if (IsDead)
        {
            CancelInvoke();
            _animator.SetTrigger("death");
        }
    }

    private bool IsPlayerMoving
    {
        get
        {
            return _rigidbody.velocity.magnitude > 0.1f;
        }
    }

    public bool IsDead
    {
        get
        {
            return Health == 0 || Food == 0;
        }
    }

    public bool IsArmed
    {
        get
        {
            if (mCurrentItem == null)
                return false;
            //Debug.Log("IsArmed = " + mCurrentItem);
            return mCurrentItem.ItemType == EItemType.Weapon;
        }
    }


    public void Eat(int amount)
    {
        Food += amount;
        if (Food > startFood)
        {
            Food = startFood;
        }

        mFoodBar.SetValue(Food);

    }

    public void Rehab(int amount)
    {
        Health += amount;
        if (Health > startHealth)
        {
            Health = startHealth;
        }

        mHealthBar.SetValue(Health);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0)
            Health = 0;

        mHealthBar.SetValue(Health);

        if (IsDead)
        {
            _animator.SetTrigger("death");
        }

    }

    #endregion


    void FixedUpdate()
    {
        //if (!IsDead)
        //{
        //    // Drop item
        //    if (mCurrentItem != null && Input.GetKeyDown(KeyCode.R))
        //    {
        //        DropCurrentItem();
        //    }
        //}
    }

    private bool mIsControlEnabled = true;

    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input for axis
        float h = Input.GetAxis("Horizontal");//forward & backward
        float v = Input.GetAxis("Vertical");//Left & Right

        //Debug.Log("h = " + h);
        //Debug.Log("v = " + v);
        //Debug.Log("_movedirection = " + _moveDirection);

                                                                 //Testing section
                                                                if (Input.GetKeyDown(KeyCode.V))
                                                                {
                                                                    
                                                                    //Debug.Log("v was pressed");
                                                                    var TG = Instantiate(TESTGATHER,transform.position,transform.rotation);
                                                                        Debug.Log("TESTGATHER = " + TESTGATHER);
                                                                        Debug.Log("TG = " + TG);
                                                                        Debug.Log("Char Local Pos = " + transform.localPosition);

                                                                        Inventory.AddItem(TG);
                                                                        Object.Destroy(TG.transform.gameObject);

                                                                        Destroy(TG,1);
                                                                        DestroyImmediate(TG, true);




                                                                }



        if (!IsDead && mIsControlEnabled)
        {
            // Interact with the item
            if (mInteractItem != null && Input.GetKeyDown(KeyCode.F))
            {
                // Interact animation
                mInteractItem.OnInteractAnimation(_animator);
            }

            // Execute action with item
            if (mCurrentItem != null && Input.GetMouseButtonDown(0))
            {
                // Dont execute click if mouse pointer is over uGUI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // TODO: Logic which action to execute has to come from the particular item
                    _animator.SetTrigger("attack_1");
                }
            }

           

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            if (_characterController.isGrounded)
            {
                _moveDirection = transform.forward * move.magnitude;

                _moveDirection *= Speed;

                if (Input.GetButton("Jump"))
                {
                    _animator.SetBool("is_in_air", true);
                    _moveDirection.y = JumpSpeed;

                }
                else
                {
                    _animator.SetBool("is_in_air", false);
                    _animator.SetBool("run", move.magnitude > 0);

                }
            }

            _moveDirection.y -= Gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }

    public void InteractWithItem() //Being called in the animation events
    {
        if (mInteractItem != null)
        {
            mInteractItem.OnInteract();
            
            if (mInteractItem is InventoryItemBase) //If the interactable item is an InventoryItemBase (like axe)
            {
                InventoryItemBase inventoryItem = mInteractItem as InventoryItemBase;// inventoryItem becomes the interacted with item if it is an Instance of InventoryItemBase? (as is like is keyword + cast) 
                Inventory.AddItem(inventoryItem);
                
                inventoryItem.OnPickup();

                if (inventoryItem.UseItemAfterPickup)
                {
                    Inventory.UseItem(inventoryItem);
                }
            }
        }
        else
        {
            Debug.Log("mInteractItem is null");
        }

        Hud.CloseMessagePanel();

        mInteractItem = null;
    }

    private InteractableItemBase mInteractItem = null;

    private void OnTriggerEnter(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();

        if (item != null)
        {
            if (item.CanInteract(other))
            {

                mInteractItem = item;

                Hud.OpenMessagePanel(mInteractItem);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            Hud.CloseMessagePanel();
            mInteractItem = null;
        }
    }
}
