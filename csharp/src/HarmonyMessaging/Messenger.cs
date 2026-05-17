using MessagePack;

namespace Harmony {

/*
 * A Messenger for creating and decoding Message Objects.
 *
 * Designed to simplify the conversion from data to MessagePack
 * without the user needing to deal with more extensive semantics.
 */
public class Messenger {
    MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard;

    /*
     * Safely create and return a Message Object.
     *
     * See Message.cs for argument details.
     *
     * Throws an ArgumentException if message is null or empty.
     */
    public Message CreateMessage(string messageType, string[] receivers, string message) {
        if (string.IsNullOrEmpty(message)) {
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
        if (string.IsNullOrEmpty(message.Content)) {
            throw new ArgumentException("Expected message.Content With Content.");
        }

        try {
            return MessagePackSerializer.Serialize<Message>(message, options);
        }
        catch (MessagePackSerializationException mpse) {
            throw new Exception("Error in SerializeMessage:", mpse);
        }
    }

    /*
     * A combination of CreateMessage and SerializeMessage.
     */
    public byte[] CreateAndSerializeMessage(string messageType, string[] receivers, string messageData) {
        try {
            return SerializeMessage(new Message(messageType, receivers, messageData));
        }
        catch (ArgumentException) {
            throw;
        }
        catch (Exception) {
            throw;
        }
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
