namespace Moe.La.Core.Integration
{
    public interface ISMSNotification
    {
        string GetBalance();
        string Send(string msg, string numbers);
    }
}