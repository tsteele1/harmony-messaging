using MessagePack;

namespace Harmony {

/*
 * A Messenger for creating and decoding Message Objects.
 *
 * Designed to simplify the conversion from data to MessagePack
 * without the user needing to deal with more extensive semantics.
 */
public class Messenger {
    MessagePackSerializerOptions options;

    public Messenger() {
        options = MessagePackSerializerOptions.Standard;
    }

    public Messenger(MessagePackSerializerOptions options) {
        this.options = options;
    }

    /*
     * Safely create and return a Message Object.
     *
     * See Message.cs for argument details.
     *
     * Throws an ArgumentException if message is null or empty.
     */
    public Message CreateMessage(string messageType, string[] receivers, object message) {
        if (message == null) {
            throw new ArgumentException("Expected message With Content.");
        }

        return new Message(messageType, receivers, message);
    }

    /*
     * Serialize a Message Object to a MessagePack byte array.
     *
     * Throws an ArgumentException if message.Content is null or empty.
     * Throws an Exception if serialization fails.
     */
    public byte[] SerializeMessage(Message message) {
        if (message.Content == null) {
            throw new ArgumentException("Expected message.Content With Content.");
        }

        try {
            return MessagePackSerializer.Serialize<Message>(message, options);
        }
        catch (MessagePackSerializationException mpse) {
            throw new Exception($"Error in SerializeMessage: {mpse}", mpse);
        }
    }

    /*
     * A combination of CreateMessage and SerializeMessage.
     */
    public byte[] CreateAndSerializeMessage(string messageType, string[] receivers, object messageData) {
        return SerializeMessage(CreateMessage(messageType, receivers, messageData));
    }

    /*
     * Deserialize bytes into a Message Object.
     *
     * Throws InvalidOperationException if messageBinary is of Length 0.
     * Throws Exception if unable to deserialize.
     */
    public Message DecodeBinaryToMessage(ReadOnlyMemory<byte> messageBinary) {
        if (messageBinary.Length == 0) {
            throw new InvalidOperationException("Attempt to Decode Sequence Length 0.");
        }

        try {
            return MessagePackSerializer.Deserialize<Message>(messageBinary, options);
        }
        catch (MessagePackSerializationException mpse) {
            throw new Exception($"Error in DecodeBinaryToMessage", mpse);
        }
    }
}

}
