using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Beam : MonoBehaviour
{
    public float laserBeamLength;
    [SerializeField] private float beamDamage;
    [SerializeField] private float rotationSpeed = 50f;
    private float _travelTime;
    private float _travelCounter;

    public Vector3 beginPos = new Vector3(0, 0, 0);
    public Vector3 endPos = new Vector3(0, 0, 0);

    private LineRenderer _lineRenderer;
    private LineRenderer _fadeLineRenderer;
    private BoxCollider2D _beamCollider;
    [SerializeField] private Texture[] textures;
    [SerializeField] private Texture[] texturesFade;
    private int _animationStep;
    private int _fadeAnimationStep;
    private bool _isFading;
    [SerializeField] private float fps = 30f;
    private float _fpsCounter;
    
    private Transform _impactPoint;
    [SerializeField] private GameObject impactGameObject;
    private Animator _impactAnimator;
    private static readonly int Fade = Animator.StringToHash("Fade");

    // Start is called before the first frame update
    public void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _fadeLineRenderer = GetComponentInChildren<LineRenderer>();
        _impactPoint = transform.GetChild(0).transform;
        _impactAnimator = impactGameObject.GetComponent<Animator>();
        _beamCollider = GetComponent<BoxCollider2D>();
        _travelTime = 360 / rotationSpeed;
        Debug.Log(_travelTime);
        _travelCounter = 0;
        //beginPos = transform.position;
        endPos = beginPos + transform.right * laserBeamLength; // transform.right is normalized vector 1 to right
        _lineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _fadeLineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _impactPoint.position = endPos;
        _beamCollider.offset = new Vector2(endPos.x/2, 0);
        _beamCollider.size = new Vector2(endPos.x, 1);
    }

    // Update is called once per frame
    private void Update()
    {
        //Quaternion.RotateTowards(_)
        /*var angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, 450, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, angle);*/
        if (_travelCounter <= _travelTime)
        {
            _travelCounter += Time.deltaTime;
            Debug.Log(_travelCounter);
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));

            var hit = Physics2D.Raycast((Vector2)beginPos,
                _lineRenderer.transform.right, laserBeamLength);

            if (hit)
            {
                var mod = Mathf.Sqrt(hit.point.x * hit.point.x + hit.point.y * hit.point.y);
                endPos = new Vector3(mod, 0, 0);
            }
            else
            {
                endPos = beginPos + Vector3.right * laserBeamLength;
            }

            _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs(endPos.x), 0, 0));
            _fadeLineRenderer.SetPosition(1, new Vector3(Mathf.Abs(endPos.x), 0, 0));
            _impactPoint.position = _impactPoint.position.normalized * endPos.x;
            _beamCollider.offset = new Vector2(Mathf.Abs(endPos.x) / 2, 0);
            _beamCollider.size = new Vector2(Mathf.Abs(endPos.x), 1);



            // Animates the beam
            _fpsCounter += Time.deltaTime;
            if (!(_fpsCounter >= 1f / fps)) return;
            _animationStep++;
            if (_animationStep == textures.Length) _animationStep = 0;

            _lineRenderer.material.SetTexture("_MainTex", textures[_animationStep]);

            _fpsCounter = 0f;
        }
        else
        {
            if (_isFading) return;
            _fpsCounter += Time.deltaTime;
            if (!(_fpsCounter >= 1f / fps)) return;
            _fadeAnimationStep++;
            if (_fadeAnimationStep == texturesFade.Length)
            {
                _isFading = true;
                StartCoroutine(FadeBeam());
                return;
            }

            _fadeLineRenderer.material.SetTexture("_MainTex", texturesFade[_fadeAnimationStep]);

            _fpsCounter = 0f;
        }
    }

    private IEnumerator FadeBeam()
    {
        _lineRenderer.SetPosition(1, beginPos);
        _impactAnimator.SetTrigger(Fade);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("EnemyCollider") ||
            collision.gameObject.CompareTag("Player")) return;*/
        if (collision.gameObject.CompareTag("EnemyCollider")) return;
        if(collision.GetComponentInParent<Player>()!=null)
            collision.GetComponentInParent<Player>().TakeDamage(beamDamage);
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyCollider")) return;
        if(collision.GetComponentInParent<Player>()!=null)
            collision.GetComponentInParent<Player>().TakeDamage(beamDamage);
    }*/
}
