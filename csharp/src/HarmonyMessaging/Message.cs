using MessagePack;

namespace Harmony {

/*
 * A general message wrapper for use with Harmony Services.
 *
 * Used internally to wrap any message type a user might
 * create with additional information and convert it to
 * MessagePack.
 *
 * Type (string):        Additional context for what type of message
 *                       is being sent across
 *                       (e.g. "Take Damage", "End Session", etc.).
 *
 * Receivers (string[]): An array of strings representing users to
 *                       send this message to.
 * 
 * Content (string):     A stringified version of the core content
 *                       of the message you want to send over websockets.
 *                       NOTE: When used with other default Harmony services,
 *                       this is expected to be stringified JSON. However,
 *                       none of the internals of this library specifically
 *                       require that to be true.
 */
[MessagePackObject]
public class Message {
    [Key(0)]
    public string Type { get; set; }

    [Key(1)]
    public string[] Receivers { get; set; }

    [Key(2)]
    public string Content { get; set; }

    public Message(string type, string[] receivers, string content) {
        this.Type = type;
        this.Receivers = receivers;
        this.Content = content;
    }
}

}
