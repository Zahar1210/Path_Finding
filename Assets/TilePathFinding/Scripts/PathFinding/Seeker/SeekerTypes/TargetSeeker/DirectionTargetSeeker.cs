using System.Collections.Generic;

namespace FindPath
{
    public abstract class DirectionTargetSeeker
    {
        public abstract Tile.Surface GetSurface(Seeker seeker, List<Tile.Surface> surfaces);
    }
}
