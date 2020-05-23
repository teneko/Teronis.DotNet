namespace Teronis.Reflection.Caching
{
    public delegate void PropertyCachingEvent<TProperty>(object sender, PropertyCachingEventArgs<TProperty> args);
}
