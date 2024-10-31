using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject linePrefab;
    public float minDistance = 0.1f;

    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentEdgeCollider;
    private List<Vector2> currentPoints;
    public RectTransform containerRect;
    private bool canDraw = true;

    void Update()
    {
        if (Camera.main == null) return;

        Vector2 mousePos = Input.mousePosition;
        if (mousePos.x >= 0 && mousePos.x <= Screen.width && mousePos.y >= 0 && mousePos.y <= Screen.height)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(containerRect, mousePos, Camera.main) && Time.timeScale != 0)
            {
                Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
                transform.position = worldMousePos;
                if (IsMouseOverWall(worldMousePos))
                {
                    FinishDrawing();
                    canDraw = false;
                    return; 
                }
                if (Input.GetMouseButtonDown(0))
                {
                    StartDrawing(worldMousePos);
                    canDraw = true;
                }
                else if (Input.GetMouseButton(0) && canDraw)
                {
                    UpdateLine(worldMousePos);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    canDraw = false;
                    FinishDrawing();
                }
            }
        }
    }

    bool IsMouseOverWall(Vector2 mouseWorldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    void StartDrawing(Vector2 startPosition)
    {
        currentPoints = new List<Vector2> { startPosition };
        GameObject lineObject = Instantiate(linePrefab);
        currentLineRenderer = lineObject.GetComponent<LineRenderer>();
        currentEdgeCollider = lineObject.GetComponent<EdgeCollider2D>();
        currentLineRenderer.positionCount = 1;
        currentLineRenderer.SetPosition(0, startPosition);
        currentEdgeCollider.SetPoints(currentPoints);
    }

    void UpdateLine(Vector2 newPoint)
    {
        if (Vector2.Distance(currentPoints[currentPoints.Count - 1], newPoint) > minDistance)
        {
            currentPoints.Add(newPoint);
            currentLineRenderer.positionCount = currentPoints.Count;
            currentLineRenderer.SetPosition(currentPoints.Count - 1, newPoint);
            currentEdgeCollider.SetPoints(currentPoints);
        }
    }

    void FinishDrawing()
    {
        if (currentLineRenderer != null && currentLineRenderer.positionCount >= 2)
        {
            currentLineRenderer = null;
            currentEdgeCollider = null;
            currentPoints = null;
        }
    }
}
