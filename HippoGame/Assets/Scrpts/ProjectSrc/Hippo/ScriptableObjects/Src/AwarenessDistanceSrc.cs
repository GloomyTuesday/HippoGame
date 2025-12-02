using UnityEngine;

[CreateAssetMenu(fileName = "AwarenessDistance", menuName = "Scriptable Obj/Project src/Awareness distance")]
public class AwarenessDistanceSrc : ScriptableObject
{
    public float AwarnessDistance = 0;

    private void OnValidate()
    {
        if (AwarnessDistance < 0)
            AwarnessDistance = 0; 
    }
}
