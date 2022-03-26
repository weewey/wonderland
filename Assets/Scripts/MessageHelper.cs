public class MessageHelper
{
    public static bool IsPrivateMessage(string message)
    {
        return message.Contains("@");
    }
}