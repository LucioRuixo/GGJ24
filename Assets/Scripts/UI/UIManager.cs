using UnityEngine;

public class UIManager : PersistentMBSingleton<UIManager>
{
    [SerializeField] private Canvas canvas;
    public Canvas Canvas => canvas;
}