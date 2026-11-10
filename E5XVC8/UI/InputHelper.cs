namespace E5XVC8.UI;

public static class InputHelper
{
    public static string ReadString(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input)) return input.Trim();

            Console.WriteLine("A mező nem lehet üres.");
        }
    }

    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value)) return value;

            Console.WriteLine("Érvénytelen szám.");
        }
    }

    public static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value)) return value;

            Console.WriteLine("Érvénytelen összeg.");
        }
    }
}