using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class WeaponParent : MonoBehaviour
{
   [SerializeField] private SpriteRenderer charRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // variable needed to track the rotation the weapon would have done
        float actualRotation = rotation_z; // variable that will contain the admissible rotation

        if(Mathf.Abs(rotation_z) < 90)
        {
            actualRotation = Mathf.Clamp(rotation_z, -10, 25);
        }else if(Mathf.Abs(rotation_z) > 90)
        {
            //actualRotation = Mathf.Clamp(rotation_z, 155, 180);
            // Clamping manually since using the above function it creates problems
            if (rotation_z >0 && rotation_z < 155)
            {
                actualRotation = 155f;
            }else if (rotation_z < 0 && rotation_z > -170)
            {
                actualRotation = -170f;
            }
        }
        
        transform.rotation = Quaternion.Euler(0f, 0f, actualRotation); // Rotation method, the weapon is rotated of "actualRotation" degrees
        
        Vector2 scale = transform.localScale;
        
        // Swapping y scale to mirror the weapon sprite
        if(Mathf.Abs(rotation_z) > 90)
        {
            scale.y = -1;
        }else if(Mathf.Abs(rotation_z) < 90)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
        
        // Rendering changes: when the weapon is in the lower part of the character sprite it will be rendered above
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = charRenderer.sortingOrder - 1;    
        }
        else
        {
            weaponRenderer.sortingOrder = charRenderer.sortingOrder + 1;
        }
    }
}
