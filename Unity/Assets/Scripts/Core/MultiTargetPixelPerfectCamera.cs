using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapMode { Pixel, Screen}

[RequireComponent(typeof(Camera))]
public class MultiTargetPixelPerfectCamera : MonoBehaviour
{
    public Camera Camera { get; private set; }

    public bool FollowIndividual = false;
    public SnapMode SnapMode = SnapMode.Pixel;
    private float Zoom = 1f;

    public float PixelToUnits = 100f;
    public bool PixelPerfectEnabled = true;

    public Vector2 LowerCameraCorner;
    public Vector2 UpperCameraCorner;
    private Rect BoundingBox;
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

        BoundingBox.Set(LowerCameraCorner.x, LowerCameraCorner.y,
            UpperCameraCorner.x - LowerCameraCorner.x,
            UpperCameraCorner.y - LowerCameraCorner.y);
    }

    void LateUpdate()
    {
        if (Camera != null)
        {
            UpdateZoomData();

            if (KeepInsideBoundingBox)
            {
                DoKeepInsideBounds();
            }
            UpdatePosition();
        }
    }

    private void UpdateZoomData()
    {
        
    }

    void UpdatePosition()
    {
        Vector3 destination = FollowIndividual ? GameObject.Find("PlayerEntity").transform.position : ComputePlayerMiddle();
        destination.z = transform.position.z;
        //destination.y -= 5;
        if (destination == lastPlayerPos)
            return;
        lastPlayerPos = destination;
        if(!InsignifiantMove(transform.position, destination))
            transform.position = RoundToScreenPixelGrid(Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime));
    }

    bool InsignifiantMove(Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin, destination) <= DampDelta;
    }


    //Snapping function 1, snaps Camera to Screen Pixels
    public Vector3 RoundToScreenPixelGrid(Vector3 worldPos)
    {
        float subPixelFactor = 2;    //1 means 1ArtPixel = 1 ScreenPixel, 2 means 1 Art = 2x2 Screen  
        if (SnapMode == SnapMode.Pixel)
        {
            float snapArt = 1F / PixelToUnits / subPixelFactor;
            return new Vector3(Mathf.Round(worldPos.x / snapArt) * snapArt,
                                Mathf.Round(worldPos.y / snapArt) * snapArt,
                                Mathf.Round(worldPos.z / snapArt) * snapArt);
        }
        else
        {
            float snapArt = 1F / PixelToUnits * subPixelFactor;
            return new Vector3(Mathf.Round(worldPos.x / snapArt) * snapArt,
                                Mathf.Round(worldPos.y / snapArt) * snapArt,
                                Mathf.Round(worldPos.z / snapArt) * snapArt);
        }
    }

    private Vector3 ComputePlayerMiddle()
    {
        PlayerMgr pmgr = FindObjectOfType<PlayerMgr>();
        if (!pmgr)
            return transform.position;
        Player[] players = pmgr.Players;
        if (players.Length == 0)
            return transform.position;
        Vector3 middle = players[0].transform.position;
        for(int i = 1;i<players.Length; ++i)
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
        if(!InsignifiantMove(Camera.transform.position, vCamPos))
            Camera.transform.position = RoundToScreenPixelGrid(vCamPos);
    }
}
