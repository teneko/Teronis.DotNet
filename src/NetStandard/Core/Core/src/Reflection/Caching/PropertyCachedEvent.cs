namespace Teronis.Reflection.Caching
{
    public delegate void PropertyCachedEvent<TProperty>(object sender, PropertyCachedEventArgs<TProperty> args);
}
