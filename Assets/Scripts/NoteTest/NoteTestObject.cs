using UnityEngine;

public class NoteTestObject : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }
}
