namespace DDDOnlineRetailerCSharp.Link.Services;

public interface IEmailService
{
    void Send(string message);
}

public class EmailService : IEmailService
{
    public void Send(string message)
    {
        Console.WriteLine(message);
    }
}