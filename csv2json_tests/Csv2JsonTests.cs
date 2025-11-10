using System.Reflection;

namespace csv2json_tests;

public class Csv2JsonTests
{
    [Fact]
    public void TestEmptyResult()
    {
        var sample = string.Empty;
        var expectedResult = "[]";
        var actual = Csv2Json.ToJson(sample, new Csv2Json.Options());

        Assert.Equal(actual, expectedResult);
    }


    [Fact]
    public void TestWithHeaders()
    {
        var sample = "Name, account, email\r\nChris, 123, chris@none.ut\r\nAndrew, 5245, andrew@none.ut";
        var expectedResult =
            "[{\"Name\":\"Chris\",\"account\":\"123\",\"email\":\"chris@none.ut\"},{\"Name\":\"Andrew\",\"account\":\"5245\",\"email\":\"andrew@none.ut\"}]";
        var actual = Csv2Json.ToJson(sample, new Csv2Json.Options());

        Assert.Equal(actual, expectedResult);
    }

    [Fact]
    public void TestWithoutHeadersDefaultPrefix()
    {
        var sample = "Chris, 123, chris@none.ut\r\nAndrew, 5245, andrew@none.ut";
        var expectedResult =
            "[{\"item0\":\"Chris\",\"item1\":\"123\",\"item2\":\"chris@none.ut\"},{\"item0\":\"Andrew\",\"item1\":\"5245\",\"item2\":\"andrew@none.ut\"}]";
        var actual = Csv2Json.ToJson(sample, new Csv2Json.Options { ContainsHeader = false });

        Assert.Equal(actual, expectedResult);
    }

    [Fact]
    public void ReadFromStream()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("csv2json_tests.testfile.csv");
        var actual = Csv2Json.ToJsonAsync(stream, new Csv2Json.Options()).Result;

        Assert.True(actual?.StartsWith("[{"));
    }
}