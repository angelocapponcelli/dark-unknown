using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create a new type: SymbolType
/*[System.Serializable]
public class SymbolType
{
    //define all of the values for the class
    //[SerializeField] public RoomLogic.Type type;
    [SerializeField] public Sprite sprite;

    //define a constructor for the class
    public SymbolType(RoomLogic.Type newTypeitem, Sprite newSpriteItem)
    {
        type = newTypeitem;
        sprite = newSpriteItem;
    }
}*/

public class BossDoor : MonoBehaviour
{
    [Range(1, 3)]
    public int myIndex;
    private SpriteRenderer _mySpriteRender;

    //[SerializeField] private Sprite _opendDoorSprite;

    /*[Header ("Symbols")]
    [SerializeField] private SpriteRenderer _symbolAboveDoor;    
    [SerializeField] private List<SymbolType> _possibleSymbols = new List<SymbolType>();*/

    //private SymbolType _actualDoorSymbol;
    private Animator _animator;
    //private bool _isClose;
    private BoxCollider2D _myBoxCollider;
    
    private static readonly int Opening = Animator.StringToHash("Opening");
    private static readonly int IsClosed = Animator.StringToHash("isClosed");
    private static readonly int Closing = Animator.StringToHash("Closing");

    private bool _canOpen = false;

    //TODO Change start in awake
    private void Start()
    {
        /*_actualDoorSymbol = _possibleSymbols[Random.Range(0, _possibleSymbols.Count)];
        
        _symbolAboveDoor.sprite = _actualDoorSymbol.sprite;*/

        //_isClose = true;
        //_mySpriteRender = GetComponent<SpriteRenderer>();
        _myBoxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsClosed, true);
        _myBoxCollider.enabled = false;
    }

    void Update()
    {
        if (_canOpen && Input.GetKey(KeyCode.E))
        {
            //LevelManager.Instance.SetRoom(myIndex, _actualDoorSymbol.type);
            Open();
            Player.Instance.SetTargetIndicatorActive(false);
        }
        
        // TODO: add auto closing of the door after the player has passed it
        // and has activated the boss
        
        // Testing code
        /*if (Input.GetKeyDown("o"))
        {
            Open();
        }
        if (Input.GetKeyDown("c"))
        {
            Close();
        }*/
    }
    
    public void Open()
    {
        //_isClose = false;
        _animator.SetTrigger(Opening);
        _animator.SetBool(IsClosed, false);
        //_mySpriteRender.sprite = _opendDoorSprite;
        _myBoxCollider.enabled = true;
    }

    public void Close()
    {
        //_isClose = true;
        _animator.SetTrigger(Closing);
        _animator.SetBool(IsClosed, true);
        //_mySpriteRender.sprite = _opendDoorSprite;
        _myBoxCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //set room colliders to false
            //useful mainly because we can't delete the serialized room from the GameManager
            //TODO: could do a room just for it with a special class, ...
            Player.Instance.ShowPlayerUI(true, "Press F to enter the door");
            _canOpen = true;
        }
    }
    public void OnTriggerExit2D (Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            _canOpen = true;
        }
    }
}
