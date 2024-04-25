using UnityEngine;

public class PartOfFence : MonoBehaviour
{
    public Terrain terrain; // �������, �� ������� ����� ������������� �����

    // ���������� ������
    private Mesh fenceMesh;
    private MeshCollider fenceCollider;
    private MeshFilter fenceMeshFilter;

    // ������� ������ �� ���������
    private Vector3[] defaultFenceVertices;

    private void Start()
    {
        InitializeComponents();
        AdjustMeshToTerrainHeight();
    }

    // ������������� �����������
    private void InitializeComponents()
    {
        terrain = Terrain.activeTerrain;
        fenceMeshFilter = GetComponent<MeshFilter>();
        fenceCollider = GetComponent<MeshCollider>();
        fenceMesh = fenceMeshFilter.mesh;
        defaultFenceVertices = fenceMesh.vertices;
    }

    // �������� ���� ������ ��� ������ ��������
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

    // �������������� ������ ������ �� �������� �� ���������
    public void SettingDefaultVertices()
    {
        Vector3[] vertices = fenceMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = defaultFenceVertices[i];
        }
        UpdateMesh(vertices);
    }

    // ���������� ���� ������
    private void UpdateMesh(Vector3[] vertices)
    {
        fenceMesh.vertices = vertices;
        fenceCollider.sharedMesh = fenceMesh;
        fenceMeshFilter.mesh = fenceMesh;
    }
}
