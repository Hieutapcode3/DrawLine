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
    public bool canDraw = true;
    public static PlayerController Instance;
    private bool isDrawingInterrupted = false; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

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

                if (Input.GetMouseButtonDown(0) && canDraw)
                {
                    StartDrawing(worldMousePos);
                    isDrawingInterrupted = false; 
                }
                else if (Input.GetMouseButton(0) && canDraw && !isDrawingInterrupted)
                {
                    UpdateLine(worldMousePos);
                    if (IsMouseOverWall(worldMousePos))
                    {
                        FinishDrawing();
                        isDrawingInterrupted = true; 
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    FinishDrawing();
                    canDraw = true; 
                    isDrawingInterrupted = false; 
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
