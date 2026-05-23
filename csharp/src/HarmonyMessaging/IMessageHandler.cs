namespace Harmony {

/*
 * A generic Message Handling Interface.
 *
 * Injected into other Harmony Services so a user can define custom Message
 * handling code alongisde Harmony's pre-configured services.
 */
public interface IMessageHandler {
    /*
     * Indicators for how your HandleMessage definition handled the received Message.
     *
     * SUCCESS: Everything went perfectly.
     * FINISHED: A special code for the server to indicate we are done handling all Message Objects.
     * ERROR: Something went wrong when handling a Message, and Harmony should recognize that.
     */
    public enum Result {
        SUCCESS,
        FINISHED,
        ERROR
    }

    public Result HandleMessage(Message message);
}

}
