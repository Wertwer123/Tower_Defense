namespace Interfaces
{
    //Interface used to get notified about the change of a specified value
    public interface IObserver<in T>
    {
        /// <summary>
        /// newer remove yourself inside of this function this would create issues
        /// </summary>
        /// <param name="observedValue"></param>
        public void Notify(T observedValue);
    }
}