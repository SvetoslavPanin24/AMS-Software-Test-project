using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public List<Building> buildings { private set; get; } // Список всех зданий
    private Building flyingBuilding; // Здание, которое в данный момент перемещается
    private Camera mainCamera; // Главная камера сцены
    private int layerMask; // Маска слоя для определения пересечений

    [SerializeField] private GameObject fencePrefab; // Префаб забора

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
        string terrainLayerName = "Terrain"; // Имя слоя Terrain
        int terrainLayer = LayerMask.NameToLayer(terrainLayerName);
        layerMask = ~(1 << terrainLayer); // Исключаем слой Terrain из маски слоя
        buildings = new List<Building>(2);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            PlaceBuilding(); // Если есть "летающее" здание, пытаемся его разместить
        }
        else
        {
            SelectBuilding(); // Если нет "летающего" здания, пытаемся выбрать здание
        }

    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            buildings.Remove(flyingBuilding); // Удаляем текущее "летающее" здание из списка зданий
            Destroy(flyingBuilding.gameObject); // Уничтожаем текущее "летающее" здание
        }

        flyingBuilding = Instantiate(buildingPrefab); // Создаем новое "летающее" здание
        buildings.Add(flyingBuilding); // Добавляем новое "летающее" здание в список зданий
        flyingBuilding.Select(); // Выбираем новое "летающее" здание
    }


    private void PlaceBuilding()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero); // Плоскость земли
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Луч, исходящий из позиции курсора мыши

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position); // Позиция в мире, куда указывает курсор мыши
            flyingBuilding.transform.position = worldPosition; // Перемещаем "летающее" здание на позицию курсора мыши

            // Определяем область вокруг здания
            Vector3 boxCenter = flyingBuilding.GetComponent<BoxCollider>().bounds.center;
            Vector3 boxSize = flyingBuilding.GetComponent<BoxCollider>().bounds.extents;
            Quaternion boxRotation = flyingBuilding.transform.rotation;

            // Проверяем, есть ли пересечения с другими объектами
            Collider[] overlaps = Physics.OverlapBox(boxCenter, boxSize, boxRotation, layerMask);
            bool isOverlapping = overlaps.Length > 1;

            if (Input.GetMouseButtonDown(0) && !isOverlapping)
            {
                // Если нажата левая кнопка мыши и нет пересечений, размещаем "летающее" здание
                if (flyingBuilding.GetComponent<House>())
                {
                    if (flyingBuilding.GetComponent<House>().Fence)
                    {
                        flyingBuilding.GetComponent<House>().Fence.GetComponent<Fence>().changingShapeToRelief();
                    }
                }

                flyingBuilding.Deselect(); // Снимаем выделение с "летающего" здания
                flyingBuilding = null; // Обнуляем "летающее" здание
            }
        }
    }

    private void SelectBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Луч, исходящий из позиции курсора мыши
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Building>())
                {
                    // Если луч попал в здание, выбираем это здание
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
                    // Если у здания нет забора, создаем забор вокруг здания
                    Fence fence = Instantiate(fencePrefab, building.transform).GetComponent<Fence>();
                    building.GetComponent<House>().Fence = fence;
                }
            }
        }
    }
}