using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            float angle =
                Mathf.Atan2(GetComponentInParent<WormController>().GetVelocity().y,
                    GetComponentInParent<WormController>().GetVelocity().x) +
                Random.Range(-Mathf.PI / 6, +Mathf.PI / 6);
            var direction =  - new Vector2 (Mathf.Cos(angle), Mathf.Sin(angle));
            GetComponentInParent<Rigidbody2D>().velocity = direction * 10f;
            GetComponentInParent<WormController>().SetVelocity(GetComponentInParent<Rigidbody2D>().velocity);
        }
    }
}
