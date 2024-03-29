using FindPath;
using UnityEngine;

public class TargetPositionTrigger : FindPathTrigger
{
    private readonly Transform _target;
    private Tile.Surface _currentSurfaceTarget;
    private Vector3Int _pastTargetPosition;
    
    public TargetPositionTrigger(Transform target)
    {
        _target = target;
    }
    
    public override Tile.Surface CheckEvent()
    {
        Vector3Int targetPos = Vector3Int.RoundToInt(_target.transform.position);
        
        if (VectorsAreDifferent(targetPos, _pastTargetPosition)) //если прошлая позиция не равна текущей 
        {
            _pastTargetPosition = targetPos;
            return targetPos;
        }

        return null;
    }
    
    private bool VectorsAreDifferent(Vector3Int currentPosition, Vector3Int pastPosition)
    {
        return currentPosition.x != pastPosition.x || currentPosition.y != pastPosition.y || currentPosition.z != pastPosition.z;
    }
    
    public override PathParams GetPathParams()
    {
        throw new System.NotImplementedException();
    }
}
