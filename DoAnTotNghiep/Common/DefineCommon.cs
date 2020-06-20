namespace DoAnTotNghiep
{
    public static class TypeUser
    {
        public static string User { get; set; } = "User";
        public static string Admin { get; set; } = "Admin";
        public static string Teacher { get; set; } = "Teacher";
    }
    public static class DefineCommon
    {      
        public static int ExpireCookie { get; set; } = 3600;
    }
}