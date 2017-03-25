//An event is a data type that can store multiple bits of information that is then passed to any event handling methods
//In order for an event to be cancellable, it must implement ICancellable

public class BlockEvent : Event {

    private Block block;

    //The event can take any paramters you want in as arguments
    public BlockEvent(Block block) : base() {
        this.block = block;
    }

    public virtual Block getBlock() {
        return this.block;
    }

}
