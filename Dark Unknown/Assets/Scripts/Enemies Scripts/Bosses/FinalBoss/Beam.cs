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

    // Start initializes the beam and all its components with the position parameters
    public void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _fadeLineRenderer = GetComponentInChildren<LineRenderer>();
        _impactPoint = transform.GetChild(0).transform;
        _impactAnimator = impactGameObject.GetComponent<Animator>();
        _beamCollider = GetComponent<BoxCollider2D>();
        // Duration of the beam set to complete a full 360 spin according to its velocity
        _travelTime = 360 / rotationSpeed;
        _travelCounter = 0;
        // Since the transform of the beam and its endpoint are rotated at a different angle from each other,
        // it is necessary to calculate 2 different points for it to not flicker in a different direction
        // when it is initialized
        endPos = beginPos + transform.up * laserBeamLength; // transform.right is normalized vector 1 to right
        var endPosImpact = beginPos + transform.right * laserBeamLength;
        _lineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _fadeLineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _impactPoint.position = endPosImpact;
        _beamCollider.offset = new Vector2(endPos.x/2, 0);
        _beamCollider.size = new Vector2(endPos.x, 1);
    }

    // Update is called once per frame calculates the rotation of the beam and animates it
    private void Update()
    {
        if (_travelCounter <= _travelTime)
        {
            _travelCounter += Time.deltaTime;
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
            // When the beam has expired its time duration, it starts fading away
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
        // Uncomment if we want the beam to only hit the player feet collider
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
