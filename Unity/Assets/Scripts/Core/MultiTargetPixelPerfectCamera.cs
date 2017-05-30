using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetPixelPerfectCamera : MonoBehaviour
{
    public Camera Camera { get; private set; }
   
    [Serializable]
    public class ZoomAdapt { public float zoom; public int minWidth; public int maxWidth; }

    public bool FollowIndividual = false;

    private float Zoom = 1f;
    public List<ZoomAdapt> Zooms = new List<ZoomAdapt>();


    public float PixelToUnits = 100f;
    public bool PixelPerfectEnabled = true;

    public Rect BoundingBox;
    public bool KeepInsideBoundingBox = false;

    public float DampTime = 0.15f;
    public float RotationDampTime = 0.25f;
    public float DampDelta = 0.01f;

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
            UpdateZoom();

            if (KeepInsideBoundingBox)
            {
                DoKeepInsideBounds();
            }
            UpdatePosition();
        }
    }

    private void UpdateZoom()
    {
        foreach( ZoomAdapt za in Zooms)
        {
            if (Screen.width >= za.minWidth && Screen.width <= za.maxWidth)
            {
                Zoom = za.zoom;
            }

        }
    }

    void UpdatePosition()
    {
        Vector3 destination = FollowIndividual ? GameObject.Find("PlayerEntity").transform.position : ComputePlayerMiddle();
        destination.z = transform.position.z;
        destination.y -= 5;
        if (destination == lastPlayerPos)
            return;
        lastPlayerPos = destination;
      //  if(!InsignifiantMove(transform.position, destination))
        transform.position = RoundCameraPosition(Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime));
    }

    bool InsignifiantMove(Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin, destination) <= DampDelta;
    }

    Vector3 RoundCameraPosition(Vector3 cam)
    {
        Vector3 pos = cam;
        pos.x = Mathf.RoundToInt(pos.x * 100) / 100f;
        pos.y = Mathf.RoundToInt(pos.y * 100) / 100f;
        return pos;
    }

    private Vector3 ComputePlayerMiddle()
    {
        PlayerMgr pmgr = FindObjectOfType<PlayerMgr>();
        if (!pmgr)
            return transform.position;
        List<Player> players = pmgr.Players;
        if (players.Count == 0)
            return transform.position;
        Vector3 middle = players[0].transform.position;
        for(int i = 1;i<players.Count; ++i)
        {
            Vector3 screenPoint = Camera.WorldToViewportPoint(players[i].transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (onScreen)
            {
                middle = Vector3.Lerp(middle, players[i].transform.position, 0.5f);
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
        //if(!InsignifiantMove(Camera.transform.position, vCamPos))
        Camera.transform.position = RoundCameraPosition(vCamPos);
    }
}