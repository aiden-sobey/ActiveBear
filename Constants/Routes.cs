using System;
namespace ActiveBear.Constants
{
    public static class Routes
    {
        internal const string Version = "1.0.0";

        internal const string Login = "/User/Login";
        internal const string Logout = "/User/Logout";
        internal const string Register = "/User/Register";

        internal const string Home = "/Message/ViewAll";
        internal const string AuthUserToChannel = "/ChannelAuth/AuthUserToChannel";
        internal const string EngageChannel = "/Channel/Engage";
        internal const string CreateChannel = "/Channel/Create";
    }
}
