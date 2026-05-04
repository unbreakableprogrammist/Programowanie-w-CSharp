namespace ShopEvents;

public class StockChangedEventArgs : EventArgs
{
    public int OldQuantity { get; }
    public int NewQuantity { get; }

    public StockChangedEventArgs(int oldQuantity, int newQuantity)
    {
        OldQuantity = oldQuantity;
        NewQuantity = newQuantity;
    }
}
