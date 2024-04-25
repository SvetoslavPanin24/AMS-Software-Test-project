using UnityEngine;

public class Fence : MonoBehaviour
{
    //элементы забора
    [SerializeField] private PartOfFence[] partsOfFence;

    private void Start()
    {
        changingShapeToRelief();
    }

    public void changingShapeToRelief()
    {
        foreach (PartOfFence partOfFence in partsOfFence)
        {
            partOfFence.AdjustMeshToTerrainHeight();
        }
    }
    public void RestorationStandardForm()
    {
        foreach(PartOfFence partOfFence in partsOfFence)
        {
            partOfFence.SettingDefaultVertices();
        }
    }
}






