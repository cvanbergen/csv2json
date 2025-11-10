# Csv2Json Library

The `Csv2Json` library provides methods to convert CSV (Comma-Separated Values) content into JSON format in C#. This utility class can handle various options and scenarios while parsing the input data. The following sections describe its usage, features, and code structure.

## Dependencies

To use this library, make sure you have installed or referenced the `System.Text` and `System.Text.Json` namespaces from .NET 5.0+ in your project.

## Options Class

The `Options` class is a custom class that allows users to specify their preferences for CSV-to-JSON conversion:

1. **ContainsHeader** (`bool`): Indicates whether the first line of the input content contains column headers (default value: true).
2. **UseAvailableHeader** (`bool`): Specifies if available header values should be used as field names in JSON when present and `ContainsHeader == true`. If this is set to false, it will use a predefined prefix instead. Default value is true.
3. **FieldPrefixIfNoHeader** (default: `"item"`): A string that acts as the default field name prefix if there's no header line available or headers are not used (`UseAvailableHeader == false`). The resulting JSON structure will have keys like `"item0"`, `"item1"`, etc.
4. **SplitChar** (default: `','`): The character to split CSV columns on while reading the input content. Default value is a comma.

## Methods

The library offers two main methods for converting CSV to JSON:

#### ToJsonAsync(Stream content, Options options)

This asynchronous method accepts a stream of the CSV data and an `Options` instance as parameters. It reads the input stream line by line using a `StreamReader`, splits it into columns based on the defined separator (`SplitChar`), creates JSON objects for each row while handling headers accordingly, then appends these entries to a `StringBuilder`. Finally, it returns the complete JSON array represented as a string.

#### ToJson(string content, Options options)

This method takes a CSV-formatted string and an `Options` instance. It splits this input by newline characters (`Environment.NewLine`), processes each line similar to `ToJsonAsync()`, but does not support asynchronous operation. The result is returned as a JSON array string.

#### PrettyPrintJson(string json)

This utility method accepts a raw JSON-formatted string and returns it in a more readable, "prettified" format with indentation. It deserializes the input using `JsonSerializer`, applies new options to enable indenting (`WriteIndented = true`), then serializes back into a formatted string. Note that this method does not rely on any custom class attributes or annotations within JSON data.

## Usage Example

Here's an example of how you can use the `Csv2Json` library:

```csharp
var csvData = "Name, Age\nJohn Doe, 30"; // Sample CSV input string (headers included)
var options = new Csv2Json.Options
{
    ContainsHeader = true,
    UseAvailableHeader = true,
};

string jsonOutput = Csv2Json.ToJson(csvData, options);
Console.WriteLine(jsonOutput);
```

This example would output the following JSON:

```json
[{"Name":"John Doe","Age":"30"}]
```

## Source Code Structure

The library is structured into an `Options` class and a static `Csv2Json` class, with all related methods defined within it. The `CreateFieldNames()`, `AddJsonEntry()` extension method for `StringBuilder`, and the utility function `PrettyPrintJson(string json)` are used to facilitate CSV-to-JSON conversion operations.

## Conclusion

The `Csv2Json` library provides a comprehensive, flexible solution for converting CSV data into JSON format in C# applications. With support for header handling and prettified output, this utility class can be easily integrated and customized to fit various use cases.