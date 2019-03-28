
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Private Members

    private AnimationController _animControl;
    //private Animator _animator;
    private Rigidbody _rigidbody;

    private NavMeshAgent _navMeshAgent;

    private CharacterController _characterController;

    private float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private InventoryItemBase _InventoryItemBase = null;

    private HealthBar mHealthBar;

    private HealthBar mFoodBar;

    private WCLevel mWCLevel;

    private int startHealth;

    private int startLevel;

    private int startFood;


    public InventoryItemBase TESTGATHER;


    #endregion

    #region Public Members

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    public Inventory Inventory;

    public GameObject Hand;

    public GameObject _selectedUnit;

    public InteractableItemBase _rightClickItemToPickUp;

    

    public HUD Hud;

    public ButtonListController _btnlstctrl;



    public float JumpSpeed = 7.0f;

    #endregion
    // Use this for initialization
    void Start()
    {

        HealthBarFindandSetValue();
        WcSkillFindandSetValue();
        FoodBarFindandSetValue();

        _rigidbody = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animControl = GameObject.FindObjectOfType<AnimationController>();
        //_navMeshAgent.updatePosition = false;
        //_navMeshAgent.updateRotation = false;

        
        Inventory.ItemUsed += Inventory_ItemUsed; //Subscribing to the event. When ItemUsed is called it invokes the Inventory_ItemUsed method.
        Inventory.ItemRemoved += Inventory_ItemRemoved; //Subscribing to the event.
        InvokeRepeating("IncreaseHunger", 0, HungerRate);
    }



    private void FoodBarFindandSetValue()
    {
        mFoodBar = Hud.transform.Find("Bars_Panel/FoodBar").GetComponent<HealthBar>();
        mFoodBar.Min = 0;
        mFoodBar.Max = Food;
        startFood = Food;
        mFoodBar.SetValue(Food);
    }

    private void WcSkillFindandSetValue()
    {
        mWCLevel = Hud.transform.Find("InventPanelButtons/SkillsPane/WoodcuttingSkill").GetComponent<WCLevel>();
        mWCLevel.Min = 1;
        mWCLevel.Max = 99;
        startLevel = Level;
        mWCLevel.SetValue(1);
    }

    private void HealthBarFindandSetValue()
    {
        mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Min = 0;
        mHealthBar.Max = Health;
        startHealth = Health;
        mHealthBar.SetValue(Health);
    }

    #region Inventory



    public void WeildItem(InventoryItemBase item, bool active)
    {

        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? Hand.transform : null;
    }

                                //Using the events syetem
                            private void Inventory_ItemRemoved(object sender, InventoryItemEventArgs e)
                            {
                                InventoryItemBase item = e.Item;
                                
                                GameObject goItem = (item as MonoBehaviour).gameObject;
                                goItem.SetActive(true);
                                goItem.transform.parent = null;

                            }


                             private void Inventory_ItemUsed(object sender, InventoryItemEventArgs e)
                             {
                                if (e.Item.ItemType != EItemType.Consumable)//If the item is not consumable
                                {
                                    // If the player carries an item, un-use it (remove from player's hand)
                                    if (_InventoryItemBase != null)
                                    {
                                        WeildItem(_InventoryItemBase, false);
                                    }

                                    InventoryItemBase item = e.Item;

                                    // Use item (put it to hand of the player)
                                    WeildItem(item, true);

                                    _InventoryItemBase = e.Item;
                                    
                                }

                             }

    private int Attack_1_Hash = Animator.StringToHash("Base Layer.Attack_1");

    public bool IsAttacking
    {
        get
        {

            
            if (_animControl.GetStateFullHashPath() == Attack_1_Hash)
            {
                Debug.Log("ISAttacking Method");
                return true;
            }
            Debug.Log("Not ISAttacking Method");
            return false;
           
        }
    }

    //???
    public void DropCurrentItem()
    {
        _animControl.SetTrigger("tr_drop");
        //_animator.SetTrigger("tr_drop");

        GameObject goItem = (_InventoryItemBase as MonoBehaviour).gameObject;

        Inventory.RemoveItem(_InventoryItemBase);


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
        Destroy((_InventoryItemBase as MonoBehaviour).GetComponent<Rigidbody>());

        _InventoryItemBase = null;
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

        //added
        if (Food > 100)
        {
            mFoodBar.SetValue(Food);
        }

        if (IsDead)
        {
            CancelInvoke();
            _animControl.SetTrigger("death");
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
            if (_InventoryItemBase == null)
                return false;
            return _InventoryItemBase.ItemType == EItemType.Weapon;
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
            _animControl.SetTrigger("death");
        }

    }

    #endregion
    //testing access from buttonlistbutton
    public void printa()
    {
        Debug.Log("worked");
    }

    void FixedUpdate()
    {
        if (!IsDead)
        {
            // Drop item
            if (_InventoryItemBase != null && Input.GetKeyDown(KeyCode.R))
            {
                DropCurrentItem();
            }
        }
    }

    private bool mIsControlEnabled = true;


    // Update is called once per frame
    void Update()
    {
        #region TestingReigon
        TestingGatheringByPressingV();
        TestingCastingByPressingL();
        //start attackEnemyCoroutine when H is Pressed
        TestAttackingEnemyByPressingH();
        if (Input.GetKeyDown(KeyCode.C))
        {
            List<int> m = new List<int>();
            m.Add(5);

            Debug.Log("m contains" + m[0]);

            m.Clear();
            Debug.Log("Cleared");
            if (m.Count == 0)
            {
                Debug.Log("m is zero");
            }


        }
        #endregion 

        //Close Right Click Menu if not hovering over it
        if (!Hud._isMouseOverRightClickMenu())
        {
            Hud.CloseRightClickMenu();
            //Debug.Log("Closing Menu off hover");
        }


        if (Input.GetMouseButtonDown(1))
        {
            SelectTargetUsingRaycast();

            Hud.RightClickMenu();
        }



        if (!IsDead && mIsControlEnabled)
        {
            // Interact with the item
            if (mInteractItem != null && Input.GetKeyDown(KeyCode.F))
            {
                // Interact animation
                mInteractItem.OnInteractAnimation(_animControl._animator);
            }

            // Execute action with item
            if (_InventoryItemBase != null && Input.GetMouseButtonDown(0))
            {
                // Dont execute click if mouse pointer is over uGUI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // TODO: Logic which action to execute has to come from the particular item
                    _animControl.SetTrigger("attack_1");

                }
            }


            // Get Input for axis
            float _horizontalAxis = Input.GetAxis("Horizontal");//forward & backward
            float _verticalaxis = Input.GetAxis("Vertical");//Left & Right
            // Calculate the forward vector
            Vector3 _camerafacingdirection = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            


            Vector3 move = _verticalaxis * _camerafacingdirection + _horizontalAxis * Camera.main.transform.right;

            if (move.magnitude > 1f)
            {
                move.Normalize();
            }

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
                    _animControl.SetState("is_in_air", true);
                    //_animator.SetBool("is_in_air", true);
                    _moveDirection.y = JumpSpeed;

                }
                else
                {
                    _animControl.SetState("is_in_air", false);
                    _animControl.SetState("run", move.magnitude > 0);

                }
            }

            _moveDirection.y -= Gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);

            #region Enable NavMesh Agent Code (commented out)
            //***************************************Enable Navmesh Agent***********************************

            //Click to move script
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if(Input.GetMouseButtonDown(0))
            //{
            //    if(Physics.Raycast(ray,out hit, 100))
            //    {
            //        //_navMeshAgent.updatePosition = true;
            //        //_navMeshAgent.updateRotation = true;
            //        _navMeshAgent.destination = hit.point;
            //        //_navMeshAgent.updatePosition = false;
            //        //_navMeshAgent.updateRotation = false;
            //    }
            //}

            //if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            //{
            //    _animator.SetBool("run",false);
            //}
            //else
            //{
            //    _animator.SetBool("run", true);
            //}
            #endregion

        }
    }




    public void EnableControl()
    {
        mIsControlEnabled = true;
    }

    public void DisableControl()
    {
        mIsControlEnabled = false;
    }


    private void TestAttackingEnemyByPressingH()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            bool t = true;
            if (t)
            {
                StartCoroutine(AttackEnemyThenWaitForSeconds(3));
            }
        }
    }

    public void TestingCastingByPressingL()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _animControl.PlayAnim("CastSpell");
        }
    }

    private void TestingGatheringByPressingV()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {

            //Debug.Log("v was pressed");
            var TG = Instantiate(TESTGATHER, transform.position, transform.rotation);
            Debug.Log("TESTGATHER = " + TESTGATHER);
            Debug.Log("TG = " + TG);
            Debug.Log("Char Local Pos = " + transform.localPosition);

            Inventory.AddItem(TG);
            Object.Destroy(TG.transform.gameObject);

            Destroy(TG, 1);
            DestroyImmediate(TG, true);
        }
    }



    public void InteractWithItemAnimEvent() //Being called in the animation events
    {
        if (mInteractItem != null)
        {
            mInteractItem.OnInteract();
            Debug.Log(mInteractItem + " Selected interactanimevent");

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


    //Opens the message Panel "Press F" if Characters walks into the collider of an item
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
    //Opens the message Panel "Press F" if Characters walks out of the collider of an item
    private void OnTriggerExit(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            Hud.CloseMessagePanel();
            mInteractItem = null;
        }
    }

   void SelectTargetUsingRaycast()
    {

        //Sends a raycast out, if it hits an object with the tag (switch statement) it creates specalised buttons

        int size;
        //string[] MenuArray = new string[];
        string type;
        EnemyScript _enemyScript;
        //InteractableItemBase i = GetComponent<InteractableItemBase>(); ;
        InteractableItemBase item;
        // = new ButtonListController();
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 10000))
        {
            string _tag = _hit.transform.tag;
            switch (_tag)
            {
                case "Enemy":
                    Hud.SetSelectedText("Enemy");
                    type = "Enemy";
                    
                    _enemyScript = GameObject.FindObjectOfType<EnemyScript>();
                    _btnlstctrl._Menuitemlist.Add("Attack");
                    _btnlstctrl._Menuitemlist.Add("Examine"); 
                    //Debug.Log("added Examine");
                    //Debug.Log("Health of this object is: " + _enemyScript.Health);
                    _btnlstctrl.GenerateList();
                    _btnlstctrl.ClearList();              
                    break;
                case "UsableObject":
                    Hud.SetSelectedText("UsableObject");
                    type = "UsableObject";
                    _btnlstctrl._Menuitemlist.Add("Pick Up");
                    _btnlstctrl._Menuitemlist.Add("Examine");
                    _btnlstctrl.GenerateList();
                    _btnlstctrl.ClearList();
                    _btnlstctrl._item = _hit.collider.GetComponent<InteractableItemBase>();
                    //_btnlstctrl.ButtonClicked("Pick Up");
                    //_rightClickItemToPickUp = _hit.collider.GetComponent<InteractableItemBase>();
                    //Debug.Log(_btnlstctrl._item.name + " Selected");
                    item = _hit.collider.GetComponent<InteractableItemBase>();
                    //RightClickInteractWithAnItem(item);


                    break;
                case "InteractableObject":
                    Hud.SetSelectedText("InteractableObject");
                    type = "InteractableObject";
                    //_genericItemScript = GameObject.FindObjectOfType<GenericItem>();
                    //_btnlstctrl._Menuitemlist.Add("Interact " + _genericItemScript.name);
                    _btnlstctrl._Menuitemlist.Add("Examine");
                    _btnlstctrl.GenerateList();
                    _btnlstctrl.ClearList();
                    break;
                default:
                    _btnlstctrl._Menuitemlist.Add("Walk Here");
                    _btnlstctrl._Menuitemlist.Add("Cancel");
                    _btnlstctrl.GenerateList();
                    _btnlstctrl.ClearList();
                    break;
            }

            

            //switch ()
            //{
            //    case :

            //        break;
            //}


        }
    }



    IEnumerator AttackEnemyThenWaitForSeconds(int _wait_time_seconds)
    {
        for (int i = 0; i < 5; i++)
        {
            _animControl.PlayAnim("Attack_1");
            yield return new WaitForSeconds(_wait_time_seconds);
        }
        
    }

    private void OnRightClickTriggerClicked(Collider other)
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

    public void RightClickInteractWithAnItem(InteractableItemBase mInteractItem) //Exact same method as InteractWithItemAnimEvent() which is Being called in the animation events, this is for rightclickmenu
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


}
