namespace Teronis.Mvc.JsonProblemDetails.Mappers
{
    public interface IMapperContext<out MappableObjectType> : IMapperContext
    {
        MappableObjectType MappableObject { get; }
    }
}
