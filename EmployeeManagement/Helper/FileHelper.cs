namespace EmployeeManagement.Helper
{
    public class FileHelper
    {
        private static readonly string _folder = "wwwroot/images";
        private static readonly string _defaultAvatar = "no-images.png";
        public FileHelper()
        {

        }
        // Delete file
        public static void DeleteFile(string fileName)
        {
            var path = Path.Combine(_folder, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        // Save file
        public static string SaveFile(IFormFile? file)
        {

            if (file == null || file.Length == 0)
            {
                return _defaultAvatar;
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(_folder, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }
        // Update file
        public static string UpdateFile(IFormFile? file, string? currentFileName)
        {
            if (file == null || file.Length == 0)
            {
                return currentFileName;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentFileName) && File.Exists(Path.Combine(_folder, currentFileName)))
                {
                    DeleteFile(currentFileName);
                }
                return SaveFile(file);
            }
        }

        // Get file
        public static string GetFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return _defaultAvatar;
            }
            var path = _folder + "/" + fileName;
            if (File.Exists(path))
            {
                return fileName;
            }
            return _defaultAvatar;
        }
    }
}
