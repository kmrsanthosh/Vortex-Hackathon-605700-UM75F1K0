using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeasurementController : MonoBehaviour
{
    public Text sizeText;

    private float objectWidth;
    private float objectHeight;
    private float objectSize;

    private void Start()
    {
        // Initialize the object size to zero
        objectWidth = 0;
        objectHeight = 0;
        objectSize = 0;
    }

    public void StartMeasurement()
    {
        // Start the measurement coroutine
        StartCoroutine(MeasurementCoroutine());
    }

    public void StopMeasurement()
    {
        // Stop the measurement coroutine
        StopAllCoroutines();
    }

    private IEnumerator MeasurementCoroutine()
    {
        // Continuously update the object size text
        while (true)
        {
            objectSize = objectWidth * objectHeight;
            sizeText.text = string.Format("Object Size: {0} square pixels", objectSize);
            yield return null;
        }
    }

    public void SetObjectSize(float width, float height)
    {
        // Set the object width and height
        objectWidth = width;
        objectHeight = height;
    }
}
