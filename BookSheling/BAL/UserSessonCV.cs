namespace BookSheling.BAL
{
    public static class UserSessonCV
    {
        private static IHttpContextAccessor _contextAccessor;
        static UserSessonCV() => _contextAccessor = new HttpContextAccessor();
        public static string? Username() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetString("UserName") : null;
        public static string? UserPassword() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetString("Password") : null;
        public static int? UserId() => (_contextAccessor != null && _contextAccessor.HttpContext != null) ? _contextAccessor.HttpContext.Session.GetInt32("UserId") : null;
    }
}
