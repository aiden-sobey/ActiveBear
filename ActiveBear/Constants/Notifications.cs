namespace ActiveBear.Constants
{
    public static class NotificationTypes
    {
        internal const string Error = "Error";
        internal const string Info = "Info";
    }

    public static class ErrorMessages
    {
        internal const string MinPassword = "Password must be at least 8 characters.";
        internal const string BlockedChannelDelete = "You can't delete channels you didn't create!";
        internal const string Unknown = "An unknown error occured performing this action.";
    }

    public static class InfoMessages
    {
        internal const string ChannelDeleted = "Channel succesfully deleted.";
    }
}
