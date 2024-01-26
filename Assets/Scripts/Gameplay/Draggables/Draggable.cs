using UnityEngine;

public class Draggable : MonoBehaviour
{
    private DropTarget target;

    public void SetTarget(DropTarget target) => this.target = target;

    public void OnDropped()
    {
        if (!target) return;
        target.ClearContent();
    }
}