using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject projector;
    private bool isSelected;

    private Collider closestBuilding; 

    [Header("LineRenderer settings")]
    [SerializeField] private Material linesMaterial;
    private LineRenderer lineRenderer;
    float distanceToDrawLine;

    [Header("UI settings")]
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text metersText;

    // ������������� ���������� ��� ������
    protected virtual void Start()
    {
        InitializeVariables();
    }

    // ���������� ��������� ������ ����
    protected virtual void Update()
    {   
        if (BuildManager.instance.buildings.Count > 1 && isSelected)
        {
            // ���������� ���������� ������
            UpdateClosestBuilding();
        }
        if (closestBuilding)
        {
            // ��������� ����� �� ���������� ������
            DrawLineToClosestBuilding();
        }
        if (!closestBuilding || !isSelected)
        {
            // ���������� ����� � ������
            DisableLineAndText();
        }
    }

    // ������������� ����������
    protected virtual void InitializeVariables()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = linesMaterial;
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = 3;
        distanceToDrawLine = 28.4f;
    }

    // ���������� ���������� ������
    protected virtual void UpdateClosestBuilding()
    {
        float minDistance = Mathf.Infinity;
        float distance;

        foreach (Building building in BuildManager.instance.buildings)
        {
            if (this == building)
            {
                continue;
            }

            distance = Vector3.Distance(transform.position, building.transform.position);

            if (distance < distanceToDrawLine && distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = building.GetComponent<Collider>();
            }
        }

        if (closestBuilding && Vector3.Distance(closestBuilding.transform.position, transform.position) > distanceToDrawLine)
        {
            closestBuilding = null;
        }
    }

    // ��������� ����� �� ���������� ������
    protected virtual void DrawLineToClosestBuilding()
    {
        if (closestBuilding != null)
        {
            metersText.enabled = true;
            lineRenderer.enabled = true;
            Vector3 closestPoint = closestBuilding.ClosestPointOnBounds(GetComponent<BoxCollider>().transform.position);
            float middlePointOnSelectBuilding;
            float distanceBetweenStructures;

            if (BuildManager.instance.buildings.Count > 1 && isSelected)
            {
                // ���� ��������� ������ ����������
                if (closestBuilding != null)
                {
                    // �������� ����� � �����
                    metersText.enabled = true;
                    lineRenderer.enabled = true;
                    // ��������� ��������� ����� �� ������� ���������� ������             
                    // ���� ���������� �� X ������, ��� �� Z
                    if (Mathf.Abs(GetComponent<BoxCollider>().bounds.center.x - closestPoint.x) > Mathf.Abs(GetComponent<BoxCollider>().bounds.center.z - closestPoint.z))
                    {
                        // ��������� ������� �����
                        if (GetComponent<BoxCollider>().transform.position.x > closestBuilding.transform.position.x)
                        {
                            middlePointOnSelectBuilding = GetComponent<BoxCollider>().bounds.center.x - GetComponent<BoxCollider>().bounds.size.x / 2;
                        }
                        else
                        {
                            middlePointOnSelectBuilding = GetComponent<BoxCollider>().bounds.center.x + GetComponent<BoxCollider>().bounds.size.x / 2;
                        }
                        // ������������� ������� ��� �����
                        lineRenderer.SetPosition(0, new Vector3(middlePointOnSelectBuilding, 0.1f, GetComponent<BoxCollider>().bounds.center.z));
                        lineRenderer.SetPosition(1, new Vector3(middlePointOnSelectBuilding, 0.1f, closestBuilding.GetComponent<BoxCollider>().bounds.center.z));
                        lineRenderer.SetPosition(2, new Vector3(closestPoint.x, 0.1f, closestBuilding.GetComponent<BoxCollider>().bounds.center.z));
                        // ������������� ������� ��� ������
                        canvas.transform.position = new Vector3(middlePointOnSelectBuilding, 0.1f, closestBuilding.GetComponent<BoxCollider>().bounds.center.z);
                        // ��������� ���������� ����� �����������
                        distanceBetweenStructures = Vector3.Distance(new Vector3(middlePointOnSelectBuilding, 0.1f, GetComponent<BoxCollider>().bounds.center.z), new Vector3(closestPoint.x, 0.1f, closestBuilding.GetComponent<BoxCollider>().bounds.center.z));
                    }
                    else
                    {
                        // ����������� �������� ��� ������, ����� ���������� �� Z ������
                        if (GetComponent<BoxCollider>().transform.position.z > closestBuilding.transform.position.z)
                        {
                            middlePointOnSelectBuilding = GetComponent<BoxCollider>().bounds.center.z - GetComponent<BoxCollider>().bounds.size.z / 2;
                        }
                        else
                        {
                            middlePointOnSelectBuilding = GetComponent<BoxCollider>().bounds.center.z + GetComponent<BoxCollider>().bounds.size.z / 2;
                        }
                        lineRenderer.SetPosition(0, new Vector3(GetComponent<BoxCollider>().bounds.center.x, 0.1f, middlePointOnSelectBuilding));
                        lineRenderer.SetPosition(1, new Vector3(closestBuilding.GetComponent<BoxCollider>().bounds.center.x, 0.1f, middlePointOnSelectBuilding));
                        lineRenderer.SetPosition(2, new Vector3(closestBuilding.GetComponent<BoxCollider>().bounds.center.x, 0.1f, closestPoint.z));
                        canvas.transform.position = new Vector3(closestBuilding.GetComponent<BoxCollider>().bounds.center.x, 0.1f, middlePointOnSelectBuilding);
                        distanceBetweenStructures = Vector3.Distance(new Vector3(GetComponent<BoxCollider>().bounds.center.x, 0.1f, middlePointOnSelectBuilding), new Vector3(closestBuilding.GetComponent<BoxCollider>().bounds.center.x, 0.1f, closestPoint.z));
                    }
                    // ������������� ����� � �����������
                    metersText.text = ((float)System.Math.Round(distanceBetweenStructures, 1)).ToString() + " �";
                }

            }
        }
    }

    // ���������� ����� � ������
    protected virtual void DisableLineAndText()
    {
        metersText.enabled = false;
        lineRenderer.enabled = false;
    }

    // ��������� ������� ��������� ����
    protected virtual void OnMouseEnter()
    {
        if (!isSelected)
        {
            GetComponent<Outline>().enabled = true;
        }
    }

    // ��������� ������� ������ ����
    protected virtual void OnMouseExit()
    {
        GetComponent<Outline>().enabled = false;
    }

    // ����� �������
    public virtual void Select()
    {
        projector.SetActive(true);
        isSelected = true;
        GetComponent<Outline>().enabled = false;
    }

    // ������ ������ �������
    public virtual void Deselect()
    {
        projector.SetActive(false);
        isSelected = false;
        GetComponent<Outline>().enabled = true;
    }
}
