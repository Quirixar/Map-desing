using UnityEngine;
using Unity.Cinemachine;

public class CameraZ : MonoBehaviour
{
    public CinemachineCamera Camera;



    public void RefreshConfiner(GameObject LevelInstance)
    {
        var confiner = Camera.GetComponent<CinemachineConfiner2D>();
        if (confiner == null) return;
        var cols = LevelInstance.GetComponentsInChildren<Collider2D>(true);
        foreach (var col in cols)
        {
            if (col.isTrigger && col.gameObject.name=="CameraArea")
            {
                confiner.BoundingShape2D = col;
                confiner.InvalidateBoundingShapeCache();
                return;
            }
        }
    }





}
