namespace EmployeeManagement.Helper
{
    public static class HashingHelper
    {
        public static string ComputeMd5Hash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);  // Use BitConverter.ToString(hashBytes).Replace("-", "").ToLower() for .NET Core 3.1 or below
            }
        }
    }

}
