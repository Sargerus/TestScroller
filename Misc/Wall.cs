using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isTriggerCreateNewLine { get; set; }

    private void Start()
    {
        isTriggerCreateNewLine = true;
    }
}
