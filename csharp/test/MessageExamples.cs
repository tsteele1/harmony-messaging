namespace Harmony.Test;

public struct BasicMessage {
    public string testStr { get; set; }
    public int testInt { get; set; }
    public float testFloat { get; set; }
    public double testDouble { get; set; }
    public bool testBool { get; set; }

    public BasicMessage(string _str, int _int, float _float, double _double, bool _bool) {
        testStr = _str;
        testInt = _int;
        testFloat = _float;
        testBool = _bool;
        testDouble = _double;
    }
}

public struct ArrayMessage {
    public int[] testInts { get; set; }
    public float[] testFloats { get; set; }
    public double[] testDoubles { get; set; }
    public bool[] testBools { get; set; }

    public ArrayMessage(int[] ints, float[] floats, double[] doubles, bool[] bools) {
        testInts = ints;
        testFloats = floats;
        testDoubles = doubles;
        testBools = bools;
    }
}

public struct ListMessage {
    public List<string> testStrs { get; set; }
    public List<float> testFloats { get; set; }
    public List<double> testDoubles { get; set; }
    public List<bool> testBools { get; set; }

    public ListMessage(List<string> _strs, List<float> _floats, List<double> _doubles, List<bool> _bools) {
        testStrs = _strs;
        testFloats = _floats;
        testDoubles = _doubles;
        testBools = _bools;
    }
}

public struct ComplexMessage {
    public Dictionary<string, BasicMessage> testMap { get; set; }
    public HashSet<float> testSet { get; set; }

    public ComplexMessage(Dictionary<string, BasicMessage> testMap, HashSet<float> testSet) {
        this.testMap = testMap;
        this.testSet = testSet;
    }
}
