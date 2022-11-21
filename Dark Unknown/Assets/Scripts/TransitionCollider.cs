using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCollider : MonoBehaviour
{
    private LevelManager _levelManager;
    
    [Range(1,3)] //done like this to have a convention
    [SerializeField] private int doorNumber;

    void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //set room colliders to false
            //useful mainly because we can't delete the serialized room from the GameManager
            //TODO: could do a room just for it with a special class, ...
            GameObject[] elements = GameObject.FindGameObjectsWithTag("TransitionCollider");
            foreach(GameObject el in elements)
            {
                el.GetComponent<Collider2D>().isTrigger = false;
            }
            
            GameManager.Instance.ResetPlayerPosition();
            _levelManager.SetRoom(doorNumber);
        }
    }
}
