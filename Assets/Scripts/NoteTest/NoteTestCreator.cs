using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTestCreator : MonoBehaviour
{
    public GameObject NotePrefab;

    public List<KeyCode> Keys;

    public List<Vector3> NotePositions;

    void Update()
    {
        for (int i = 0; i < Keys.Count; i++)
        {
            if (Input.GetKeyDown(Keys[i]))
            {
                Instantiate(NotePrefab, NotePositions[i], Quaternion.identity);
            }
        }
    }
}
