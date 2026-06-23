using GymMangment.BLL.Comman;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GymMangment.BLL.Services.AttachmentServices
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly long _maxFile = 5 * 1024 * 1024;
        private readonly string[] _allowedExtenstions = { ".png", ".jpeg", ".jpg" };

        public AttachmentService(ILogger<AttachmentService> logger,IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public bool Delete(string fileName, string folderName)
        {
            var fullPath = Path.Combine(_env.ContentRootPath,folderName, fileName);
            try
            {
                if (!File.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,$"Failed To Delete Attachment {fileName}");
                return false;
            }
            
        }

        public (Stream stream, string ContentType)? GetStream(string fileName, string folderName)
        {
            if(string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(folderName)) return null;
            var fullPath = Path.Combine(_env.ContentRootPath,folderName, fileName);
            if(!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(fullPath).ToLower();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream stream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (stream == null) return null;
            if (stream.Length == 0) return null;
            if(stream.Length > _maxFile)
            {
                _logger.LogError($"File Rejected : File Too Large {stream.Length} bytes");
                return null;
            }
            var extenstion = Path.GetExtension(fileName);
            if(string.IsNullOrWhiteSpace(extenstion) || !_allowedExtenstions.Contains(extenstion))
            {
                _logger.LogError($"File Rejected : Extention {extenstion} not allowed");
                return null;
            }
            var uploadsFolder = Path.Combine(_env.ContentRootPath,folderName);
            Directory.CreateDirectory(uploadsFolder);
            var storedFile = $"{Guid.NewGuid()}{fileName}";
            var filePath = Path.Combine(uploadsFolder, storedFile);
            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fs, ct);
                return storedFile;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,$"Faild to upload {fileName}");
                return null;
            }
        }
    }
}
