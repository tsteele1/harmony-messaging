namespace Harmony {

/*
 * A generic Message Handling Interface intended for usage with clients.
 *
 * Injected into other Harmony Services so a user can define custom Message
 * handling code alongisde Harmony's pre-configured services.
 */
public interface IClientMessageHandler {
    public Task HandleMessage(Message message, IClientMessaging messaging);
}

/*
 * Same as IMessageHandler, except intended for usage with servers.
*/
public interface IServerMessageHandler {
    public Task HandleMessage(Message message, IServerMessaging messaging);
}

}
