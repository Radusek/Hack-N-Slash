using UnityEngine;

public interface IRecyclable
{
    bool IsActive();
    void SetInitialValues(Vector3 spawnPosition, Vector3 areaPosition, float areaRadius);
    void StartDying();
    void EndDying();
}