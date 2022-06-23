namespace Moe.La.Core.Integration
{
    public interface IEmailNotification
    {
        bool Send(string to, string subject, string message);

        string GetTemplate(string message);
    }
}