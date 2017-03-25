//The ICancellable interface allows the event that implements it to be cancelled
public interface ICancellable {

    //The abstract method that all inheriting classes must have in order for the event to be cancellable
    bool isCancelled();

}
