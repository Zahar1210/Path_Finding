namespace FindPath
{
    public abstract class DynamicPath 
    {
        // это условаия при котоорый путь будет переделан (динамичность)
        public abstract bool VerificationPath(Seeker seeker);
    }
}
