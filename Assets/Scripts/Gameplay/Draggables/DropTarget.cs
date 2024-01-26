using UnityEngine;

public class DropTarget : MonoBehaviour
{
    private Draggable content;

    private void Update()
    {
        if (content) content.transform.position = transform.position;
    }

    public bool TrySetContent(Draggable content)
    {
        if (!content || this.content) return false;

        this.content = content;
        content.SetTarget(this);

        return true;
    }

    public void ClearContent() => content = null;
}