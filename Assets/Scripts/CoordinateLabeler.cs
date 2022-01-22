using TMPro;
using UnityEngine;

public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro label;
    private Vector2Int coordinates;

    void Awake()
    {
        label = GetComponent<TextMeshPro>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
    }

    private void DisplayCoordinates()
    {
        var position = transform.parent.position;
        coordinates.x = Mathf.RoundToInt(position.x / 10);
        coordinates.y = Mathf.RoundToInt(position.z / 10);
        label.text = coordinates.x + "," + coordinates.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}