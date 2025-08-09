using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Managing;


public class RTSSelector : NetworkBehaviour
{
    [Header("Selection Settings")]
    public LayerMask armyGroupLayerMask;
    public LayerMask groundLayerMask;
    public int teamId = 0;

    [Header("Visual Settings")]
    public Color boxFillColor = new Color(0f, 1f, 0f, 0.25f);
    public Color boxBorderColor = Color.green;
    public float boxBorderThickness = 2f;

    private Camera _cam;
    private Vector2 _dragStart;
    private Vector2 _dragEnd;
    private bool _isDragging;

    private readonly List<ArmyGroup> _selectedGroups = new List<ArmyGroup>();

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }
    public void SetTeam(int id)
    {
        teamId = id; 
    }

    private void Update()
    {
        // Start drag on left button down
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _dragStart = Mouse.current.position.ReadValue();
            _dragEnd = _dragStart;
            ClearSelection();
        }

        // Update drag end while holding
        if (_isDragging && Mouse.current.leftButton.isPressed)
        {
            _dragEnd = Mouse.current.position.ReadValue();
        }

        // End drag on left button release
        if (_isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isDragging = false;
            _dragEnd = Mouse.current.position.ReadValue();

            if (Vector2.Distance(_dragStart, _dragEnd) < 5f)
                SelectSingle(_dragEnd);
            else
                SelectInBox(_dragStart, _dragEnd);
        }

        // Handle right-click movement
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            IssueMoveCommand();
        }
    }

    private void OnGUI()
    {
        if (!_isDragging) return;

        Rect rect = GetScreenRect(_dragStart, _dragEnd);
        DrawRectFilled(rect, boxFillColor);
        DrawRectBorder(rect, boxBorderThickness, boxBorderColor);
    }

    private void SelectSingle(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, armyGroupLayerMask))
        {
            var grp = hit.collider.GetComponent<ArmyGroup>();
            if (grp != null && grp.RequestTeam() == teamId)
                AddSelection(grp);
        }
    }

    private void SelectInBox(Vector2 start, Vector2 end)
    {
        Rect box = GetScreenRect(start, end);
        foreach (var grp in FindObjectsByType<ArmyGroup>(FindObjectsSortMode.None))
        {
            if (grp.RequestTeam() != teamId) continue;

            Vector3 screenPt = _cam.WorldToScreenPoint(grp.transform.position);
            screenPt.y = Screen.height - screenPt.y;

            if (box.Contains(screenPt))
                AddSelection(grp);
        }
    }

    private void IssueMoveCommand()
    {
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayerMask))
        {
            if (!IsServerInitialized)
            {
                foreach (var grp in _selectedGroups)
                    grp.SetDestination(hit.point);
            }
            else
            {
                foreach (var grp in _selectedGroups)
                    grp.ai.destination= hit.point;
            }
                
        }
    }

    private Rect GetScreenRect(Vector2 a, Vector2 b)
    {
        a.y = Screen.height - a.y;
        b.y = Screen.height - b.y;
        Vector2 min = Vector2.Min(a, b);
        Vector2 max = Vector2.Max(a, b);
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private void DrawRectFilled(Rect rect, Color col)
    {
        GUI.color = col;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    private void DrawRectBorder(Rect rect, float thickness, Color col)
    {
        DrawRectFilled(new Rect(rect.xMin, rect.yMin, rect.width, thickness), col);
        DrawRectFilled(new Rect(rect.xMin, rect.yMin, thickness, rect.height), col);
        DrawRectFilled(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), col);
        DrawRectFilled(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), col);
    }

    private void ClearSelection()
    {
        _selectedGroups.RemoveAll(g => g == null);
        foreach (var grp in _selectedGroups)
            Highlight(grp, false);
        _selectedGroups.Clear();
    }

    private void AddSelection(ArmyGroup grp)
    {
        _selectedGroups.Add(grp);
        Highlight(grp, true);
    }

    private void Highlight(ArmyGroup grp, bool on)
    {
        if (grp == null) return;
        

        Renderer rend = grp.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = on ? Color.green : Color.white;
       
    }
}
