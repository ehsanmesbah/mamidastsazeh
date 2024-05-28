using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Abstractions
{
    public enum SaveResult
    {
        Success = 1,
        Fail = 2,
        UnknownFormat = 3,
        ErrorConversion = 4,
        BigFile=5
    };
    public interface IMamiFileManager
    {
        public SaveResult SaveImage(IFormFile file,string path);
        public SaveResult GenerateThumbnailFromImage(string sourceFile, string thumbnailPath);
        public Task<SaveResult> SaveVideo(IFormFile file, string path);
        public Task<SaveResult> SavePdf(IFormFile file, string path);
        public Task<SaveResult> GenerateThumbnailFromVideo(string sourceFile, string thumbphotopath);
        public Task<string> ConvertVideo(string filePath, string destinationPath,bool full);
        public Task<string> ConvertAudio(string filePath, string destinationPath);


    }
}
