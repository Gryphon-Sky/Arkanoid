using UnityEngine;

public class EditorOnlyObject : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_EDITOR
        GameObject.Destroy(gameObject);
#endif
    }
}
