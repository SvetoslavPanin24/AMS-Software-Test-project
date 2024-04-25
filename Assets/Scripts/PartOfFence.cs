using UnityEngine;

public class PartOfFence : MonoBehaviour
{
    public Terrain terrain; // Террейн, на котором будет располагаться забор

    // Компоненты забора
    private Mesh fenceMesh;
    private MeshCollider fenceCollider;
    private MeshFilter fenceMeshFilter;

    // Вершины забора по умолчанию
    private Vector3[] defaultFenceVertices;

    private void Start()
    {
        InitializeComponents();
        AdjustMeshToTerrainHeight();
    }

    // Инициализация компонентов
    private void InitializeComponents()
    {
        terrain = Terrain.activeTerrain;
        fenceMeshFilter = GetComponent<MeshFilter>();
        fenceCollider = GetComponent<MeshCollider>();
        fenceMesh = fenceMeshFilter.mesh;
        defaultFenceVertices = fenceMesh.vertices;
    }

    // Подгонка меша забора под высоту террейна
    public void AdjustMeshToTerrainHeight()
    {
        if (fenceMesh)
        {
            Vector3[] vertices = fenceMesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldVertex = transform.TransformPoint(vertices[i]);
                float terrainHeight = terrain.SampleHeight(worldVertex);
                vertices[i].y += terrainHeight;
            }
            UpdateMesh(vertices);
        }
    }

    // Восстановление вершин забора до значений по умолчанию
    public void SettingDefaultVertices()
    {
        Vector3[] vertices = fenceMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = defaultFenceVertices[i];
        }
        UpdateMesh(vertices);
    }

    // Обновление меша забора
    private void UpdateMesh(Vector3[] vertices)
    {
        fenceMesh.vertices = vertices;
        fenceCollider.sharedMesh = fenceMesh;
        fenceMeshFilter.mesh = fenceMesh;
    }
}
