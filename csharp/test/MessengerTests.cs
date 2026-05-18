using Harmony;
using System.Text.Json;

namespace Harmony.Test;

public class MessengerTests
{
    // It doesn't matter what this is. It's just not serializable by our Messenger
    public struct ErrorStruct {
        public int x;
        public float y;
        public double z;
    }

    [Fact]
    public void TestMessageCreation() {
        string messageType = "Test Case";
        string[] receivers = ["Me", "Myself", "I", "Console"];
        Dictionary<object, object> messageData = new Dictionary<object, object> {
            {"Hello", "World"}, 
            {"Counter", 0},
            {"Why", "oh"}
        };

        Messenger messenger = new Messenger();

        Message message = messenger.CreateMessage(messageType, receivers, messageData);

        Assert.Equal(messageType, message.Type);
        Assert.Equal(receivers, message.Receivers);

        Dictionary<object, object> messageContent = (Dictionary<object, object>) message.Content;
        foreach (KeyValuePair<object, object> kvp in messageData) {
            Assert.Contains(kvp.Key, messageContent);
            Assert.Equal(kvp.Value, messageContent[kvp.Key]);
        }
    }

    [Fact]
    void TestSerializationSuccess() {
        Messenger messenger = new Messenger();

        // Notice how this is formatted identically to our ErrorStruct
        object validStruct = new { x = 0, y = 0.0f, z = 0.0 };

        try {
            byte[] message = messenger.CreateAndSerializeMessage("Test", ["Hello", "world"], validStruct);
        }
        catch(Exception) {
            Assert.Fail("You should be able to serialize generic objects");
        }
    }

    [Fact] 
    void TestSerializationCatch() {
        Messenger messenger = new Messenger();

        ErrorStruct errStruct = new ErrorStruct();

        try {
            byte[] message = messenger.CreateAndSerializeMessage("Test", ["Hello", "World"], errStruct);
            Assert.Fail("You should not be able to serialize this struct.");
        }
        catch(Exception) {
            Assert.True(true);
        }
    }

    [Fact]
    void TestEmptyDecodeCatch() {
        Messenger messenger = new Messenger();

        try {
            messenger.DecodeBinaryToMessage(new ReadOnlyMemory<byte>());
            Assert.Fail("Decoding an empty byte memory area should throw an exception.");
        }
        catch (InvalidOperationException) {
            Assert.True(true);
        }
    }

    [Fact]
    public void TestMockTransfer() {
        string messageType = "Test Case";
        string[] receivers = ["Me", "Myself", "I", "Console"];
        Dictionary<object, object> messageData = new Dictionary<object, object> {
            {"Hello", "World"}, 
            {"Counter", 0},
            {"Why", "oh"}
        };

        Messenger messenger = new Messenger();

        byte[] encodedMessage = messenger.CreateAndSerializeMessage(messageType, receivers, messageData);

        Message decodedMessage = messenger.DecodeBinaryToMessage(encodedMessage);

        Assert.Equal(decodedMessage.Type, messageType);
        Assert.Equal(decodedMessage.Receivers, receivers);

        Dictionary<object, object> messageContent = (Dictionary<object, object>) decodedMessage.Content;

        foreach (KeyValuePair<object, object> kvp in messageData) {
            Assert.Contains(kvp.Key, messageContent);
            Assert.Equal(kvp.Value, messageContent[kvp.Key]);
        }
    }


}
