namespace OganiMasterMVC.Areas.Admin.Data
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool IsAllowedSize(this IFormFile file, int mb)
        {
            return file.Length <= mb * 1024 * 1024;
        }
        public static async Task<string> GenerateFile(this IFormFile file, string path, string extension=null! )
        {
            if (string.IsNullOrEmpty(extension))
                extension = Path.GetExtension(file.FileName).ToLower();

            var fileName = $"product-{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName; 
        }

    }
}
