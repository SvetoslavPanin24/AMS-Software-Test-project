using UnityEngine;

public class House : Building, IFencable
{
    //�����
    public Fence Fence { get; set; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
