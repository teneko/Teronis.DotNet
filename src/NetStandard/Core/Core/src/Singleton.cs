namespace Teronis
{
    public sealed class Singleton : ISingleton
    {
        public static readonly Singleton Default = new Singleton();

        private Singleton() { }
    }
}
