using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace session3Mvc.Helpers
{
    public static class DoucmentSettings
    {
        public static string UploadFile(IFormFile file , string folderName) 
        { 
            //1. GetHashCode located folder Path
            string folderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName);

            //2. get file name and make it uniqe 
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3. get file path
            string filePath=Path.Combine(folderPath,fileName);

            //4. save file as streams:[data ber time]
            using var fs=new FileStream(filePath,FileMode.Create);
            file.CopyTo(fs);
            return fileName ;
        }

        public static void  DeleteFile (string fileName , string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory (), "wwwroot\\files", folderName , fileName);
            if (File.Exists(filePath) )
            {
                File.Delete(filePath);
            }
        }
    }
}
