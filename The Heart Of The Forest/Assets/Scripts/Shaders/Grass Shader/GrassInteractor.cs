using UnityEngine;

public class GrassInteractor : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", transform.position);
    }
}