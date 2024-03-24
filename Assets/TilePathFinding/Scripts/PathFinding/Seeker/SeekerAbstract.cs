namespace FindPath
{
    public abstract class SeekerAbstract 
    {
        public abstract bool CheckEvent(Seeker seeker);
        public abstract PathParams GetPathParams(Seeker seeker);
    }
}