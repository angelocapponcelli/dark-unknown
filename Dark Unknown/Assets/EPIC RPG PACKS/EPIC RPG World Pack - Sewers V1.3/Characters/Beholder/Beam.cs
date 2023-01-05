using System;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float laserBeamLength;
    private LineRenderer _lineRenderer;
    [SerializeField] private float rotationSpeed = 50f;
    private BoxCollider2D _beamCollider;
    private Transform _impactPoint;

    public Vector3 beginPos = new Vector3(0, 0, 0);
    public Vector3 endPos = new Vector3(0, 0, 0);

    [SerializeField] private Texture[] textures;

    private int _animationStep;

    [SerializeField] private float fps = 30f;
    private float _fpsCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _impactPoint = transform.GetChild(0).transform;
        _beamCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        //beginPos = transform.position;
        endPos = beginPos + transform.right * laserBeamLength; // transform.right is normalized vector 1 to right
        _lineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _impactPoint.position = endPos;
        _beamCollider.offset = new Vector2(endPos.x/2, 0);
        _beamCollider.size = new Vector2(endPos.x, 1);
    }

    // Update is called once per frame
    private void Update()
    {
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
        _lineRenderer.SetPosition(1, new Vector3(Mathf.Abs(endPos.x),0,0));
        _impactPoint.position = _impactPoint.position.normalized * endPos.x;
        _beamCollider.offset = new Vector2(Mathf.Abs(endPos.x)/2, 0);
        _beamCollider.size = new Vector2(Mathf.Abs(endPos.x), 1);

        
        
        // Animates the beam
        _fpsCounter += Time.deltaTime;
        if (!(_fpsCounter >= 1f / fps)) return;
        _animationStep++;
        if (_animationStep == textures.Length) _animationStep = 0;
        
        _lineRenderer.material.SetTexture("_MainTex", textures[_animationStep]);

        _fpsCounter = 0f;
    }
}
