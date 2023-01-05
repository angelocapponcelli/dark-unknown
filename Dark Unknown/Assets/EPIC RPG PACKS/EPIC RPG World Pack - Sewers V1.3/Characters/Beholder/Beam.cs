using System;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float laserBeamLength;
    private LineRenderer _lineRenderer;
    [SerializeField] private float rotationSpeed = 50f;
    /*private Transform _beamColliderPos;*/
    private BoxCollider2D _beamCollider;
    private Transform _impactPoint;
    private RaycastHit2D _hit;

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
        /*_beamColliderPos = transform.GetChild(1).transform;
        _beamCollider = GetComponentInChildren<BoxCollider2D>();*/
        _beamCollider = GetComponent<BoxCollider2D>();
        beginPos = transform.position;
    }

    private void Start()
    {
        //beginPos = transform.position;
        endPos = beginPos + transform.right * laserBeamLength; // transform.right is normalized vector 1 to right
        _lineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
        _impactPoint.position = endPos;
        var temp = new Vector2(endPos.x, 0);
        _beamCollider.offset = new Vector2(endPos.x/2, 0);
        _beamCollider.size = new Vector2(endPos.x, 1);
    }

    // Update is called once per frame
    private void Update()
    {
        // modify position of impactpoint to pos of transform of beam?
        transform.Rotate(transform.forward * (rotationSpeed * Time.deltaTime));
        /*_hit = Physics2D.Raycast((Vector2)beginPos, transform.right);

        if (_hit)
        {
            endPos = _hit.point;
        }
        else
        {
            endPos = beginPos + transform.right * laserBeamLength;
        }*//*
        endPos = beginPos + transform.right * laserBeamLength;
        _lineRenderer.SetPosition(1, endPos);
        _impactPoint.position = endPos;
        var temp = new Vector2(endPos.x, 0);
        _beamCollider.offset = temp/2;
        _beamCollider.size = temp;
        */

        /*//Calculate new position 
        Vector3 newBeginPos = transform.localToWorldMatrix * new Vector4(beginPos.x, beginPos.y, beginPos.z, 1);
        Vector3 newEndPos   = transform.localToWorldMatrix * new Vector4(endPos.x, endPos.y, endPos.z, 1);

        //Apply new position
        _lineRenderer.SetPosition(0, newBeginPos);
        _lineRenderer.SetPosition(1, newEndPos);*/
        
        _fpsCounter += Time.deltaTime;
        if (_fpsCounter >= 1f / fps)
        {
            _animationStep++;
            if (_animationStep == textures.Length) _animationStep = 0;
        
            _lineRenderer.material.SetTexture("_MainTex", textures[_animationStep]);

            _fpsCounter = 0f;
        }
    }
}
