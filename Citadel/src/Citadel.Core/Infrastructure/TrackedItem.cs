namespace Citadel.Infrastructure
{
    public class TrackedItem<T>
    {
        public TrackedItem(T item, TrackedItemActivity activity)
        {
            Item = item;
            Activity = activity;
        }
        public T Item { get; }
        public TrackedItemActivity Activity { get; set; }
    }
}
