using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public List<Building> buildings { private set; get; } // ������ ���� ������
    private Building flyingBuilding; // ������, ������� � ������ ������ ������������
    private Camera mainCamera; // ������� ������ �����
    private int layerMask; // ����� ���� ��� ����������� �����������

    [SerializeField] private GameObject fencePrefab; // ������ ������

    public static BuildManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<BuildManager>();
        }
       
    }

    private void Start()
    {
        string terrainLayerName = "Terrain"; // ��� ���� Terrain
        int terrainLayer = LayerMask.NameToLayer(terrainLayerName);
        layerMask = ~(1 << terrainLayer); // ��������� ���� Terrain �� ����� ����
        buildings = new List<Building>(2);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            PlaceBuilding(); // ���� ���� "��������" ������, �������� ��� ����������
        }
        else
        {
            SelectBuilding(); // ���� ��� "���������" ������, �������� ������� ������
        }

    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            buildings.Remove(flyingBuilding); // ������� ������� "��������" ������ �� ������ ������
            Destroy(flyingBuilding.gameObject); // ���������� ������� "��������" ������
        }

        flyingBuilding = Instantiate(buildingPrefab); // ������� ����� "��������" ������
        buildings.Add(flyingBuilding); // ��������� ����� "��������" ������ � ������ ������
        flyingBuilding.Select(); // �������� ����� "��������" ������
    }


    private void PlaceBuilding()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero); // ��������� �����
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // ���, ��������� �� ������� ������� ����

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position); // ������� � ����, ���� ��������� ������ ����
            flyingBuilding.transform.position = worldPosition; // ���������� "��������" ������ �� ������� ������� ����

            // ���������� ������� ������ ������
            Vector3 boxCenter = flyingBuilding.GetComponent<BoxCollider>().bounds.center;
            Vector3 boxSize = flyingBuilding.GetComponent<BoxCollider>().bounds.extents;
            Quaternion boxRotation = flyingBuilding.transform.rotation;

            // ���������, ���� �� ����������� � ������� ���������
            Collider[] overlaps = Physics.OverlapBox(boxCenter, boxSize, boxRotation, layerMask);
            bool isOverlapping = overlaps.Length > 1;

            if (Input.GetMouseButtonDown(0) && !isOverlapping)
            {
                // ���� ������ ����� ������ ���� � ��� �����������, ��������� "��������" ������
                if (flyingBuilding.GetComponent<House>())
                {
                    if (flyingBuilding.GetComponent<House>().Fence)
                    {
                        flyingBuilding.GetComponent<House>().Fence.GetComponent<Fence>().changingShapeToRelief();
                    }
                }

                flyingBuilding.Deselect(); // ������� ��������� � "���������" ������
                flyingBuilding = null; // �������� "��������" ������
            }
        }
    }

    private void SelectBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���, ��������� �� ������� ������� ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Building>())
                {
                    // ���� ��� ����� � ������, �������� ��� ������
                    flyingBuilding = hit.collider.gameObject.GetComponent<Building>();
                    flyingBuilding.Select();

                    if (hit.collider.GetComponent<House>())
                    {
                        if (flyingBuilding.GetComponent<House>().Fence)
                        {
                            flyingBuilding.GetComponent<House>().Fence.GetComponent<Fence>().RestorationStandardForm();
                        }
                    }
                }
            }
        }
    }
    public void BuildFenceAroundBuildings()
    {
        foreach (Building building in buildings)
        {
            if (building.GetComponent<House>())
            {
                if (!building.GetComponent<House>().Fence)
                {
                    // ���� � ������ ��� ������, ������� ����� ������ ������
                    Fence fence = Instantiate(fencePrefab, building.transform).GetComponent<Fence>();
                    building.GetComponent<House>().Fence = fence;
                }
            }
        }
    }
}