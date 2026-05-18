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
 * Content (object):     A generic object that users must take care
 *                       to know the type of when they encode / decode
 *                       so that they can safely handle it and cast it
 *                       back to the relevant types when receiving.
 *                       NOTE: This does not allow for user defined
 *                       types. Stick to basic predefined data structures
 *                       or primitives when using this (ideally, your
 *                       most complicated data will essentially be JSON
 *                       anyways when sending data over).
 */
[MessagePackObject]
public class Message {
    [Key(0)]
    public string Type { get; set; } = null!;

    [Key(1)]
    public string[] Receivers { get; set; } = null!;

    [Key(2)]
    public object Content { get; set; } = null!;

    public Message() {}

    public Message(string type, string[] receivers, object content) {
        this.Type = type;
        this.Receivers = receivers;
        this.Content = content;
    }
}

}
