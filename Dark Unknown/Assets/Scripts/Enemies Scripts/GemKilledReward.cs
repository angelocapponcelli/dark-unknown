using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemKilledReward : MonoBehaviour
{
    private Vector3 _speed;
    [SerializeField] private float distanceToAttract = 5.0f;
    private bool _initialWait = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-2.0f,2.0f),0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GemMovement();
    }

    private void GemMovement()
    {
        if (_initialWait)
        {
            StartCoroutine(Waiting());
        }
        
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        
        if (Vector3.Distance(playerPosition, transform.position) > distanceToAttract || _initialWait)
        {
            if (_speed.x > 0 || _speed.y > 0)
            {
                _speed -= _speed * Time.deltaTime;
                transform.position += _speed * Time.deltaTime;
            }
        }
        else
        {
            _speed.x = playerPosition.x - transform.position.x;
            _speed.y = playerPosition.y - transform.position.y;
            _speed += _speed * Time.deltaTime;
            transform.position += _speed * Time.deltaTime;
        }
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.75f);
        _initialWait = false;
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
