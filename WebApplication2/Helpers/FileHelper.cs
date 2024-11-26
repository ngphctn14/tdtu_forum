using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Helpers
{
    public static class FileHelper
    {
        private static readonly string UploadsBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
        
        public static void InitializeDirectoryStructure()
        {
            // Create base directories if they don't exist
            Directory.CreateDirectory(Path.Combine(UploadsBasePath, "Users"));
            Directory.CreateDirectory(Path.Combine(UploadsBasePath, "Posts"));
        }
        
        public static string CreatePostDirectory(int postId)
        {
            string postPath = Path.Combine(UploadsBasePath, "Posts", postId.ToString());
            Directory.CreateDirectory(postPath);
            return postPath;
        }

        public static string CreateUserDirectory(int userId)
        {
            string userPath = Path.Combine(UploadsBasePath, "Users", userId.ToString());
            Directory.CreateDirectory(userPath);
            return userPath;
        }
        
        public static List<string> SavePostFiles(int postId, IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || !files.Any()) return new List<string>();

            string postPath = CreatePostDirectory(postId);
            List<string> savedFiles = new List<string>();

            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string safeFileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_");
                        string extension = Path.GetExtension(file.FileName);
                        string uniqueFileName = $"{safeFileName}_{DateTime.Now.Ticks}{extension}";
                        
                        string filePath = Path.Combine(postPath, uniqueFileName);
                        
                        file.SaveAs(filePath);
                        savedFiles.Add(uniqueFileName);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error saving file: {ex.Message}");
                    }
                }
            }
            
            return savedFiles;
        }
        public static string SaveUserProfilePicture(int userId, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return null;
                
            string userPath = CreateUserDirectory(userId);
            
            try
            {
                string safeFileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_");
                string extension = Path.GetExtension(file.FileName);
                string uniqueFileName = $"{safeFileName}_{DateTime.Now.Ticks}{extension}";
                
                string filePath = Path.Combine(userPath, uniqueFileName);
                
                file.SaveAs(filePath);
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving profile picture: {ex.Message}");
                throw;
            }
        }
        
        private static bool IsValidImageFile(string extension)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension.ToLower());
        }
    }
}