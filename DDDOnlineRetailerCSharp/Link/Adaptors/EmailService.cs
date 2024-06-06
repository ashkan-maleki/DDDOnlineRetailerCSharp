namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public interface IEmailService
{
    bool Sent(string email);
    void Send(string email, string message);
}

public class EmailService : IEmailService
{
    private Dictionary<string, string> _logs { get; } = new();

    public bool Sent(string email) => _logs.ContainsKey(email);

    public void Send(string email, string message)
    {
        _logs.Add(email, message);
        Console.WriteLine(message);
    }
}