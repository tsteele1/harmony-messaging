namespace Harmony {

// A collection of Messaging interfaces used with MessageHandlers to
// allow the user to perform messaging tasks based on Messages received.

/*
 * A basic Messaging interface with minimal implementation.
 *
 * Designed to give you the essentials and ONLY the essentials
 * if you want to create your own interface.
*/
public interface IMessaging {
    public Task SendMessageAsync(Message message);
    public Messenger Messager { get; }
}

/*
 * Messaging for Harmony's built in Server.
 *
 * Includes accessors / control flow for the Server's
 * who can join the server to receive messages, as well
 * as an Id that clients can identify with and connect to.
*/
public interface IServerMessaging: IMessaging {
    public Task SendMessageToGameAsync(Message message);
    public Task LockReceivers();
    public Task UnlockReceivers();
    public string[] Receivers { get; }
    public string Id { get; }
}

/*
 * Messaging for Harmony's built in C# Client.
 * 
 * Includes assurances that the Client can get
 * Receivers from the server to send messages to,
 * and storage for the Id that IServerMessaging has.
*/
public interface IClientMessaging: IMessaging {
    public string[] Receivers { get; set; }
    public string Id { get; set; }
}

}
