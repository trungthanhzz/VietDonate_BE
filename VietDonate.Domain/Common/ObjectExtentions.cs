namespace VietDonate.Domain.Common
{
    public static class ObjectExtentions
    {
        public static string GetKeyBlackListRedis(string jti) => GlobalVarians.BlackListFormat + jti;
    }
}
