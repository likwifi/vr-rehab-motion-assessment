using UnityEngine;

public class RotateSky : MonoBehaviour
{
    public float rotateSpeed = 1.2f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private Skybox skybox;
    private void Start()
    {
        skybox = GetComponent<Skybox>();
    }
}
