using FindPath;
using UnityEngine;

public abstract class FindPathTrigger
{
    // это первичные условия для поиска пути
    public abstract Tile.Surface CheckEvent();

    public abstract PathParams GetPathParams();
}
