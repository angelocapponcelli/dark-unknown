using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Range(1, 3)]
    public int myIndex;
    private SpriteRenderer _mySpriteRender;
    [SerializeField] private Sprite _opendDoorSprite;
    [SerializeField] private SpriteRenderer _simbol;
    [SerializeField] private Sprite[] _simbolsPossible;
    private BoxCollider2D _myBoxCollider;


    private bool _isClose;
    private void Awake()
    {
        _simbol.sprite = _simbolsPossible[Random.Range(0, _simbolsPossible.Length)];
        _isClose = true;
        _mySpriteRender = GetComponent<SpriteRenderer>();
        _myBoxCollider = GetComponent<BoxCollider2D>();
        _myBoxCollider.enabled = false;

        //TODO Change door symbols here at run time to make random generetion starting when the room is initialized
    }


    public void Open()
    {
        _isClose = false;
        _mySpriteRender.sprite = _opendDoorSprite;
        _myBoxCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //set room colliders to false
            //useful mainly because we can't delete the serialized room from the GameManager
            //TODO: could do a room just for it with a special class, ...
            
            LevelManager.Instance.SetRoom(myIndex, RoomLogic.Type.HARD);
        }
    }
}
