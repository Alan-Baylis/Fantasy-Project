//The ICancellable interface allows the event that implements it to be cancelled
public interface ICancelable {

    //The abstract method that all inheriting classes must have in order for the event to be cancellable
    bool isCanceled();

    //All cancellable events must have some sort of functionality to allow them to be cancellable
    void setCanceled(bool canceled);

}
