using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemReward : MonoBehaviour
{
    private Vector3 _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-2.0f,2.0f),0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _speed -= _speed * Time.deltaTime;
        transform.position += _speed * Time.deltaTime;
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //save the taken gems in the UI

            LevelManager.Instance.killedRewards.Remove(gameObject);
            GameManager.Instance.player.ModifyKilledReward(1);
            
            Destroy(gameObject);
        }
    }

}
