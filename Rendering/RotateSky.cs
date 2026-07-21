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

    // Update is called once per frame
    void Update()
    {
     var x= skybox.transform.rotation.x;
     skybox.material.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
