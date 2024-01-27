using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private DropTarget assignedTarget;

    private DropTarget target;

    public bool Grabbed { get; private set; } = false;
    public bool Locked { get; private set; } = false;

    public event Action OnLockedEvent;

    public void SetTarget(DropTarget target)
    {
        if (Locked) return;

        this.target = target;
        if (target == assignedTarget)
        {
            Locked = true;
            OnLockedEvent?.Invoke();
        }
    }

    public void OnGrabbed() => Grabbed = true;

    public void OnDropped()
    {
        if (Locked) return;

        Grabbed = false;
        target?.ClearContent();
    }
}