using UnityEngine;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private Renderer webcamRenderer;

    private void Start()
    {

        webcamRenderer = GetComponent<Renderer>();

        webcamTexture = new WebCamTexture();

        webcamRenderer.material.mainTexture = webcamTexture;

    
        webcamTexture.Play();
    }

    public void StopCamera()
    {
        webcamTexture.Stop();
    }
}
