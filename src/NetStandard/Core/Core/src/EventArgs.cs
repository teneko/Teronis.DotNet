namespace Teronis
{
    public class EventArgs<DataType>
    {
        public DataType Data { get; }

        public EventArgs(DataType data) =>
            Data = data;
    }
}
