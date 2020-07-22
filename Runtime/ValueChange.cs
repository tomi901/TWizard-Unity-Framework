


namespace TWizard.Framework
{
	public delegate void ValueChange<T>(T previous, T current);
	
    public interface IValueChangeArgs<T>
    {
        T Previous { get; }
        T Current { get; }
    }

    public struct ValueChangeArgs<T> : IValueChangeArgs<T>
    {
        public T Previous { get; }
        public T Current { get; }


        public ValueChangeArgs(T previous, T current)
        {
            Previous = previous;
            Current = current;
        }
    }
}
