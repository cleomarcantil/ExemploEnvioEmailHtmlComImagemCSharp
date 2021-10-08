using System.Security;

static class InputHelper
{
    public static string? GetValue(string? message = null)
    {
        if (message is not null)
            Console.Write($"{message} ");

        return Console.ReadLine();
    }

    public static SecureString GetPassword(string? message = null)
    {
        if (message is not null)
            Console.Write($"{message} ");

        var password = new SecureString();

        while (true)
        {
            var k = Console.ReadKey(true);

            if (k.Key == ConsoleKey.Enter)
                break;

            if ((k.Key == ConsoleKey.Backspace) && (password.Length > 0))
            {
                password.RemoveAt(password.Length - 1);
                Console.Write("\b \b");
                continue;
            }

            if (k.KeyChar == '\u0000')
                continue;

            password.AppendChar(k.KeyChar);
            Console.Write("*");
        }

        Console.WriteLine();

        return password;
    }
}