
namespace SzlqTech.Common.Context
{
    public sealed class UserContext
    {
        private static IUserContext? _context;

        public static string? UserCode => _context?.UserCode;

        public static string? Username => _context?.Username;

        public static string? PersonName => _context?.PersonName;

        public static string? RoleCode => _context?.RoleCode;

        public static long? RoleId => _context?.RoleId;

        public static long? UserId => _context?.UserId;

        public static string? Session => _context?.Session;

        public static void SetContext(IUserContext context)
        {
            _context = context;
        }

        public static IUserContext? GetContext()
        {
            return _context;
        }
    }
}
