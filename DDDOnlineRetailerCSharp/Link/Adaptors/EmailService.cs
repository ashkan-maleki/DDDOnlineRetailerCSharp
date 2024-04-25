namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public interface IEmailService
{
    string Send(string message);
}

public class EmailService : IEmailService
{
    public string Send(string message)
    {
        Console.WriteLine(message);
        return message;
    }
}