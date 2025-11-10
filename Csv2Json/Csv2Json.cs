using System.Text;
using System.Text.Json;

public static class Csv2Json
{
    public static async Task<string> ToJsonAsync(Stream content, Options options)
    {
        using var reader = new StreamReader(content);
        var firstLine = await reader.ReadLineAsync();
        var fieldNames = CreateFieldNames(firstLine, options);
        var result = new StringBuilder("[");

        while (!reader.EndOfStream)
        {
            var lineValues = firstLine?.Split(options.SplitChar) ??
                             (await reader.ReadLineAsync()).Split(options.SplitChar);
            result.AddJsonEntry(lineValues, fieldNames, options);
            if (!reader.EndOfStream) result.Append(",");
            firstLine = null;
        }

        result.Append("]");
        return result.ToString();
    }

    public static string ToJson(string content, Options options)
    {
        var result = new StringBuilder("[");
        var lines = content.Split(Environment.NewLine);
        var fieldNames = CreateFieldNames(lines[0], options);

        for (var x = options.ContainsHeader ? 1 : 0; x < lines.Length; x++)
        {
            result.AddJsonEntry(lines[x].Split(options.SplitChar), fieldNames, options);
            if (x < lines.Length - 1) result.Append(",");
        }

        result.Append("]");
        return result.ToString();
    }

    /// <summary>
    ///     This utility method accepts a raw JSON-formatted string and
    ///     returns it in a more readable, "prettified" format with indentation.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static string PrettyPrintJson(string json)
    {
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(jsonElement, options);
    }

    private static string[] CreateFieldNames(string firstLine, Options options)
    {
        if (options.ContainsHeader && options.UseAvailableHeader)
            return firstLine.Split(options.SplitChar).Select(n => n.Replace(" ", string.Empty)).ToArray();
        return firstLine.Split(options.SplitChar).Select((n, idx) => options.FieldPrefixIfNoHeader + idx).ToArray();
    }

    private static void AddJsonEntry(this StringBuilder result, string[] lineValues, string[] headers, Options options)
    {
        result.Append("{");
        for (var y = 0; y < lineValues.Length; y++)
        {
            result.Append("\"");
            result.Append(headers[y]);
            result.Append("\":\"");
            result.Append(lineValues[y].Trim());
            result.Append("\"");
            if (y < lineValues.Length - 1) result.Append(",");
        }

        result.Append("}");
    }

    public class Options
    {
        /// <summary>
        ///     Indication of the presence of a header line
        /// </summary>
        public bool ContainsHeader = true;

        /// <summary>
        ///     String value to be used as a field name, when there is no header line available or the header line should not be
        ///     used
        ///     Default value = "item"
        ///     Will result in a json like { "item0": "", "item1": "", "item2": "" }
        /// </summary>
        public string FieldPrefixIfNoHeader = "item";

        /// <summary>
        ///     The separation char to use to split the columns in the CSV file/content
        /// </summary>
        public char SplitChar = ',';

        /// <summary>
        ///     Should the column names be based upon an available header line
        /// </summary>
        public bool UseAvailableHeader = true;
    }
}