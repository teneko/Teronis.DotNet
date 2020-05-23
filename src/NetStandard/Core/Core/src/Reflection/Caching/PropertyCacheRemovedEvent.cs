namespace Teronis.Reflection.Caching
{
    public delegate void PropertyCacheRemovedEvent<TProperty>(object sender, PropertyCacheRemovedEventArgs<TProperty> args);
}
