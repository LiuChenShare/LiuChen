namespace Chenyuan.Caching.Defaults
{
    public enum CacheItemRemovedReason
    {
        Removed = 1,
        Expired,
        Underused,
        DependencyChanged
    }
}
