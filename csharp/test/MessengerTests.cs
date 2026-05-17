using Harmony;
using System.Text.Json;

namespace Harmony.Test;

public class MessengerTests
{
    [Theory]
    [InlineData("Test", new string[] {"John", "Jane", "Adam"}, "Hello World!", 0, 3.1415f, 2.187654321, false)]
    public void TestCreateBasicMessageSuccess(string messageType, string[] receivers, string s, int i, float f, double d, bool b)
    {
        BasicMessage basic = new BasicMessage(s, i, f, d, b);
        Messenger messenger = new Messenger();

        string basicStr = JsonSerializer.Serialize(basic);

        Message message = messenger.CreateMessage(messageType, receivers, basicStr);

        Assert.Equal(messageType, message.Type);
        Assert.Equal(receivers, message.Receivers);
        Assert.Equal(basicStr, message.Content);
    }

    [Theory]
    [InlineData("Test2", new string[] {"Alex", "Amy"}, new int[] {3, 2, 1, 0}, new float[] { 0.0f, 0.1f }, new double[] {0.12}, new bool[] {false, true, false, false, false})]
    public void TestCreateArrayMessageSuccess(string messageType, string[] receivers, int[] i, float[] f, double[] d, bool[] b) {
        ArrayMessage array = new ArrayMessage(i, f, d, b);
        Messenger messenger = new Messenger();

        string arrStr = JsonSerializer.Serialize(array);

        Message message = messenger.CreateMessage(messageType, receivers, arrStr);

        Assert.Equal(messageType, message.Type);
        Assert.Equal(receivers, message.Receivers);
        Assert.Equal(arrStr, message.Content);
    }

    [Fact]
    public void TestCreateListMessageSuccess() {
        string messageType = "Lists";
        string[] receivers = {"Me"};
        List<string> strings = ["What do I put here?", "No idea.", "t"];
        List<float> floats = [3.14f, 4f, 2f, 5f];
        List<double> doubles = [1.234, 3.456, 4.567, 5.678, 6.789, 7.890];
        List<bool> bools = [false, false, false, true, true, true, false];

        ListMessage lm = new ListMessage(strings, floats, doubles, bools);

        string listStr = JsonSerializer.Serialize(lm);

        Messenger messenger = new Messenger();
        Message message = messenger.CreateMessage(messageType, receivers, listStr);


        Assert.Equal(messageType, message.Type);
        Assert.Equal(receivers, message.Receivers);
        Assert.Equal(listStr, message.Content);
    }

    [Fact]
    public void TestCreateComplexMessage() {
        Dictionary<string, BasicMessage> testMap = new Dictionary<string, BasicMessage> {
            {"Hello_World", new BasicMessage("1", 1, 1.0f, 1.001, true)},
            {"Hello_World2", new BasicMessage("2", 2, 2.0f, 2.02, false)},
            {"Hello_World3", new BasicMessage("3", 1, 4.3f, 3.124, true)}
        };
        HashSet<float> testSet = new HashSet<float> { 1.0f, 2.0f, 4.3f };

        ComplexMessage complex = new ComplexMessage(testMap, testSet);

        string complexStr = JsonSerializer.Serialize(complex);
        string messageType = "Complex";
        string[] receivers = ["P1", "P2", "P3", "P4", "P5", "P6"];
        Messenger messenger = new Messenger();
        Message message = messenger.CreateMessage(messageType, receivers, complexStr);


        Assert.Equal(messageType, message.Type);
        Assert.Equal(receivers, message.Receivers);
        Assert.Equal(complexStr, message.Content);
    }

    [Fact]
    public void TestDataTransferComplex() {
        Dictionary<string, BasicMessage> testMap = new Dictionary<string, BasicMessage> {
            {"Hello_World", new BasicMessage("1", 1, 1.0f, 1.001, true)},
            {"Hello_World2", new BasicMessage("2", 2, 2.0f, 2.02, false)},
            {"Hello_World3", new BasicMessage("3", 1, 4.3f, 3.124, true)}
        };
        HashSet<float> testSet = new HashSet<float> { 1.0f, 2.0f, 4.3f };

        ComplexMessage complex = new ComplexMessage(testMap, testSet);

        string complexStr = JsonSerializer.Serialize(complex);
        string messageType = "Complex";
        string[] receivers = ["P1", "P2", "P3", "P4", "P5", "P6"];
        Messenger messenger = new Messenger();

        byte[] messageBytes = messenger.CreateAndSerializeMessage(messageType, receivers, complexStr);

        Message message = messenger.DecodeBinaryToMessage(new ReadOnlyMemory<byte>(messageBytes));

        Assert.Equal(message.Type, messageType);
        Assert.Equal(message.Receivers, receivers);

        ComplexMessage decoded = JsonSerializer.Deserialize<ComplexMessage>(message.Content);
        Dictionary<string, BasicMessage> decodedMap = decoded.testMap;
        HashSet<float> decodedSet = decoded.testSet;

        foreach (KeyValuePair<string, BasicMessage> kvp in decodedMap) {
            Assert.True(testMap.ContainsKey(kvp.Key));
            Assert.Equal(testMap[kvp.Key].testStr, kvp.Value.testStr);
            Assert.Equal(testMap[kvp.Key].testInt, kvp.Value.testInt);
            Assert.Equal(testMap[kvp.Key].testFloat, kvp.Value.testFloat);
            Assert.Equal(testMap[kvp.Key].testDouble, kvp.Value.testDouble);
            Assert.Equal(testMap[kvp.Key].testBool, kvp.Value.testBool);
        }

        foreach (float value in testSet) {
            Assert.Contains(value, decodedSet);
        }
    }

    [Fact]
    public void TestDataTransferList() {
        string messageType = "Lists";
        string[] receivers = {"Me"};
        List<string> strings = ["What do I put here?", "No idea.", "t"];
        List<float> floats = [3.14f, 4f, 2f, 5f];
        List<double> doubles = [1.234, 3.456, 4.567, 5.678, 6.789, 7.890];
        List<bool> bools = [false, false, false, true, true, true, false];

        ListMessage lm = new ListMessage(strings, floats, doubles, bools);

        string listStr = JsonSerializer.Serialize<ListMessage>(lm);

        Messenger messenger = new Messenger();
        byte[] messageBytes = messenger.CreateAndSerializeMessage(messageType, receivers, listStr);

        Message decodedMessage = messenger.DecodeBinaryToMessage(new ReadOnlyMemory<byte>(messageBytes));

        Assert.Equal(decodedMessage.Type, messageType);
        Assert.Equal(decodedMessage.Receivers, receivers);

        ListMessage decoded = JsonSerializer.Deserialize<ListMessage>(decodedMessage.Content);

        Assert.Equal(strings.Count, decoded.testStrs.Count);
        Assert.Equal(floats.Count, decoded.testFloats.Count);
        Assert.Equal(doubles.Count, decoded.testDoubles.Count);
        Assert.Equal(bools.Count, decoded.testBools.Count);

        for (int i = 0; i < strings.Count; ++i) {
            Assert.Equal(strings[i], decoded.testStrs[i]);
        }

        for (int i = 0; i < floats.Count; ++i) {
            Assert.Equal(floats[i], decoded.testFloats[i]);
        }

        for (int i = 0; i < doubles.Count; ++i) {
            Assert.Equal(doubles[i], decoded.testDoubles[i]);
        }

        for (int i = 0; i < bools.Count; ++i) {
            Assert.Equal(bools[i], decoded.testBools[i]);
        }
    }
}
