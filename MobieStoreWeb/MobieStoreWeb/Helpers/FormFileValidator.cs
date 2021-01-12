using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Helpers
{
    public static class FormFileValidator
    {
        public static bool IsValidFileSizeLimit(this IFormFile file, long fileSizeLimit) => file != null && file.Length > 0 && file.Length < fileSizeLimit;

        public static string GetFileExtension(string fileName) => Path.GetExtension(fileName).ToLowerInvariant();

        private static readonly string[] _imagePermittedExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };

        public static bool IsValidImageFileExtension(this IFormFile file, out string extension)
        {
            extension = GetFileExtension(file.FileName);
            return IsValidImageFileExtension(extension);
        }

        public static bool IsValidImageFileExtension(string extension) => !string.IsNullOrEmpty(extension) && _imagePermittedExtensions.Contains(extension);
    }
}
