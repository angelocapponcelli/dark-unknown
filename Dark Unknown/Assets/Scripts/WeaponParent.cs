using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class WeaponParent : MonoBehaviour
{
    
    public Vector2 PointerPosition { get; set; }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        } else if (direction.x > 0)
        {
            scale.y = 1;
        }

        transform.localScale = scale;
    }
}
