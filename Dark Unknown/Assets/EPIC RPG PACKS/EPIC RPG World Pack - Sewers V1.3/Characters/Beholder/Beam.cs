using System;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float laserBeamLength;
    private LineRenderer _lineRenderer;
    [SerializeField] private float rotationSpeed = 50f;
    
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
    }

    private void Start()
    {
        beginPos = transform.position;
        endPos = beginPos + transform.right * laserBeamLength;
        _lineRenderer.SetPositions(new Vector3[] {beginPos, endPos });
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        
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
