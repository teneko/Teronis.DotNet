using System.Threading.Tasks;

namespace Teronis
{
    public delegate Task EventHandlerAsync<in TSender, in TArgs>(TSender sender, TArgs eventArgs);
}
