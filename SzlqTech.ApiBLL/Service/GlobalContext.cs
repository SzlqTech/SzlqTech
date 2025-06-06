using System.ComponentModel;

namespace SzlqTech.ApiBLL.Service
{
    public static class GlobalContext
    {
        public static string SaUserName { get; } = "sa";


        public static string SaPassword { get; } = "sieractech";


        public static string? Token { get; set; }

        public static string? Cookie { get; set; }

        public static IContainer Container { get; set; } = null;

    }
}
