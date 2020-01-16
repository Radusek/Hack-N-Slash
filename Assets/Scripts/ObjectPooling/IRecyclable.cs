using UnityEngine;

public interface IRecyclable
{
    bool IsActive();
    GameObject GetGameObject();
    void SetInitialEnemyValues(Vector3 spawnPosition, Vector3 areaPosition, float areaRadius);
    void SetInitialProjectileValues(Vector3 spawnPosition, Vector3 velocity, int dmg, LayerMask layers, GameObject caster);
    void StartDying();
    void EndDying();
}