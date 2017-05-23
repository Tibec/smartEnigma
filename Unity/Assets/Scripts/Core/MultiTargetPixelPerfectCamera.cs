using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetPixelPerfectCamera : MonoBehaviour
{
    public Camera Camera { get; private set; }
   
    public float Zoom = 1f;
    public float PixelToUnits = 100f;
    public bool PixelPerfectEnabled = true;

    public Rect BoundingBox;
    public bool KeepInsideBoundingBox = false;

    public float DampTime = 0.15f;
    public float RotationDampTime = 0.25f;


    private Vector3 velocity = Vector3.zero;
    private Vector3 lastPlayerPos;
    // Use this for initialization
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (Camera != null)
        {
            if (KeepInsideBoundingBox)
            {
                DoKeepInsideBounds();
            }
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {

        Vector3 destination = ComputePlayerMiddle();
        destination.z = transform.position.z;
        if (destination == lastPlayerPos)
            return;
        lastPlayerPos = destination;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
    }

    private Vector3 ComputePlayerMiddle()
    {
        PlayerMgr pmgr = FindObjectOfType<PlayerMgr>();
        if (!pmgr)
            return transform.position;
        List<Player> players = pmgr.Players;
        if (players.Count == 0)
            return transform.position;
        Vector3 middle = Vector3.zero;
        for(int i = 0;i<players.Count; ++i)
        {
            Vector3 screenPoint = Camera.WorldToViewportPoint(players[i].transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen)
            {
                if (middle == Vector3.zero)
                    middle = players[i].transform.position;
                else
                    middle = Vector3.Lerp(middle, players[i].transform.position, 0.5f);
                players[i].OutOfCameraBound = false;
            }
            else
            {
                players[i].OutOfCameraBound = true;
            }
        }

        return middle;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            if (Camera == null) Camera = GetComponent<Camera>();
            OnPreCull();
        }
    }

    Vector3 m_vCamRealPos;
    void OnPreCull()
    {
        if (Camera != null)
        {
            if (KeepInsideBoundingBox)
            {
                DoKeepInsideBounds();
            }

            //Note: ViewCamera.orthographicSize is not a real zoom based on pixels. This is the formula to calculate the real zoom.
            Camera.orthographicSize = (Camera.pixelRect.height) / (2f * Zoom * PixelToUnits);

            m_vCamRealPos = Camera.transform.position;

            if (PixelPerfectEnabled)
            {
                Vector3 vPos = Camera.transform.position;
                float mod = (1f / (Zoom * PixelToUnits));
                float modX = vPos.x > 0 ? vPos.x % mod : -vPos.x % mod;
                float modY = vPos.y > 0 ? vPos.y % mod : -vPos.y % mod;
                vPos.x -= modX;
                vPos.y -= modY;

                Camera.transform.position = vPos;
            }
        }
    }

    void OnPostRender()
    {
        if (Camera != null)
        {
            Camera.transform.position = m_vCamRealPos;
        }
    }

    void DoKeepInsideBounds()
    {
        Rect rCamera = new Rect();
        rCamera.width = Camera.pixelRect.width / (PixelToUnits * Zoom);
        rCamera.height = Camera.pixelRect.height / (PixelToUnits * Zoom);
        rCamera.center = Camera.transform.position;

        Vector3 vOffset = Vector3.zero;

        float right = (rCamera.x < BoundingBox.x) ? BoundingBox.x - rCamera.x : 0f;
        float left = (rCamera.xMax > BoundingBox.xMax) ? BoundingBox.xMax - rCamera.xMax : 0f;
        float down = (rCamera.y < BoundingBox.y) ? BoundingBox.y - rCamera.y : 0f;
        float up = (rCamera.yMax > BoundingBox.yMax) ? BoundingBox.yMax - rCamera.yMax : 0f;

        Vector3 vCamPos = Camera.transform.position;
        vOffset.x = right + left;
        vOffset.y = up + down;
        vCamPos += vOffset;
        if (rCamera.width >= Mathf.Abs(BoundingBox.width)) vCamPos.x = BoundingBox.center.x;
        if (rCamera.height >= Mathf.Abs(BoundingBox.height)) vCamPos.y = BoundingBox.center.y;
        Camera.transform.position = vCamPos;
    }
}