namespace EShop.Domain.Notification;

public interface IEmailNotification
{
    Task SendAsync(IEmailMessage emailMessage, CancellationToken cancellationToken = default);
}

public interface IEmailMessage
{
    string From { get; set;}
    string To { get; set;}
    string Subject { get; set;}
    string Body { get; set;}
}
