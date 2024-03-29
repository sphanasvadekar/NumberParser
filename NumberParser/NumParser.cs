
using System.Text.Json;
using System.Xml.Linq;

namespace NumberParser
{
    // Interface for file writers
    public interface IFileWriter
    {
        void WriteToFile(IEnumerable<int> num, string file);
    }

    // Text file writer
    public class TextFileWriter : IFileWriter
    {
        public void WriteToFile(IEnumerable<int> num, string file)
        {
            File.WriteAllLines(file, num.Select(num => num.ToString()));
        }
    }

    // XML file writer
    public class XmlFileWriter : IFileWriter
    {
        public void WriteToFile(IEnumerable<int> num, string file)
        {
            XElement root = new XElement("Numbers", num.Select(num => new XElement("Number", num)));
            root.Save(file);
        }
    }

    // JSON file writer
    public class JsonFileWriter : IFileWriter
    {
        public void WriteToFile(IEnumerable<int> num, string file)
        {
            string json = JsonSerializer.Serialize(num);
            File.WriteAllText(file, json);
        }
    }

    // Factory for creating file writers

    public static class FileWriterFactory
    {
        public static IFileWriter Create(string format)
        {
            
            if (format.Equals("TEXT", StringComparison.OrdinalIgnoreCase))
            {
                return new TextFileWriter();
            }
            else if (format.Equals("XML", StringComparison.OrdinalIgnoreCase))
            {
                return new XmlFileWriter();
            }
            else if (format.Equals("JSON", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonFileWriter();
            }
            else
            {
                throw new ArgumentException("Unsupported file format.");
            }
        }
    }

    class NumParser
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Use Case: NumberParser <numbers> <fileformat>");
                return;
            }

            string[] numbersStr = args[0].Split(',');
            List<int> numbers = numbersStr.Select(int.Parse).ToList(); // Convert string array to list of integers

            //  numbers sorting in descending 
            numbers = numbers.OrderByDescending(n => n).ToList();
            
            //Second argument as file format
            string format = args[1].ToUpper();

            // file writer based on format
            IFileWriter fileWriter = FileWriterFactory.Create(format);

            // Get numbers to file
            string fileName = $"SortedNumbers.{format.ToLower()}";
            fileWriter.WriteToFile(numbers, fileName);

            //file  created as per the input and will be saved in descending order
            Console.WriteLine($"Sorted numbers have been written to {fileName}.");
        }
    }
}

