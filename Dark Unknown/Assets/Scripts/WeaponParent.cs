
using System.Collections;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
   [SerializeField] private SpriteRenderer charRenderer, weaponRenderer;
   [SerializeField] private Animator _weaponAnimator;
   [SerializeField] private Animator _effectAnimator;
    public Vector2 PointerPosition { get; set; }
    
    private float _delay = 1f;
    private bool _attackBlocked;
    
    [SerializeField] private float _swordDamage;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 1f;

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
        /*
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = charRenderer.sortingOrder - 1;    
        }
        else
        {
            weaponRenderer.sortingOrder = charRenderer.sortingOrder + 1;
        }*/
    }

    public void Attack()
    {
        if (_attackBlocked)
            return;
        _weaponAnimator.SetTrigger("Attack");
        _effectAnimator.SetTrigger("Attack");
        _attackBlocked = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                //TODO bisogna generalizzare non solo per skeletonController ma per tutti i nemici
                enemy.GetComponent<SkeletonController>().TakeDamage(Random.Range(Mathf.Max(2, _swordDamage-5), Mathf.Max(5, _swordDamage + 5))); 
            }
        }

        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(_delay);
        _attackBlocked = false;
    }

    //ToDisplaycircleOfAttack
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
