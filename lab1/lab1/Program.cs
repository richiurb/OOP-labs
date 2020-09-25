using System;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static void Run()
        {
            Console.Write("Enter file name: ");
            var filepath = Console.ReadLine();
            var ini = new IniFile(filepath);

            Console.Write("Enter section name: ");
            var sectionName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(sectionName))
            {
                throw new Exception("Section name is invalid");
            }

            Console.Write("Enter parameter name: ");
            var parameterName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new Exception("Parameter name is invalid");
            }

            Console.Write("Enter parameter type: ");
            var parameterType = Console.ReadLine();

            var parameterValue = ini.GetParameter(sectionName, parameterName);

            switch (parameterType)
            {
                case "int":
                    if (!int.TryParse(parameterValue, out var intValue))
                    {
                        ThrowCastException(sectionName, parameterName, parameterType, parameterValue);
                    }

                    Console.WriteLine(intValue);
                    break;

                case "double":
                    if (!double.TryParse(parameterValue, out var doubleValue))
                    {
                        ThrowCastException(sectionName, parameterName, parameterType, parameterValue);
                    }

                    Console.WriteLine(doubleValue);
                    break;

                case "string":
                    Console.WriteLine(parameterValue);
                    break;

                default:
                    throw new Exception($"Type \"{parameterType}\" is not supported");
            }
        }

        static void ThrowCastException(string sectionName, string parameterName, string parameterType, string parameterValue)
        {
            throw new InvalidCastException(
                $"Could not cast parameter \"{parameterName}\" with value \"{parameterValue}\" in section \"{sectionName}\" to type \"{parameterType}\"");
        }
    }
}
