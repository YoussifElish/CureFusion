namespace CureFusion.Domain.Abstactions.Consts
{
    public static class Permissions
    {
        public static string Type { get; } = "permissions";

        public const string GetAppoitments = "Appoitments:read";
        public const string AddAppoitmentss = "Appoitments:add";
        public const string UpdateAppoitments = "Appoitments:update";
        public const string DelteAppoitments = "Appoitments:delete";




        public static IList<string?> GetAllPermission() =>
            typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
    }
}
