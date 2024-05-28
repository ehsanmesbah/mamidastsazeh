using mamidastsazeh.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Security.Policy;
using RestSharp;
using Newtonsoft.Json;
using mamidastsazeh.ViewModels;
using mamidastsazeh.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System.IO;
using Xabe.FFmpeg;

namespace mamidastsazeh.Services
{
    public class MamiFileManager: IMamiFileManager
    {
        IConfiguration _configuration;
        public MamiFileManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SaveResult SaveImage(IFormFile file, string path)
        {
            try
            {
                var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                double wToH = (double)image.Width / (double)image.Height;
                int newW = System.Convert.ToInt32(_configuration["ImageOption:ImageWidth"]);
                int newH = (int)(newW / (double)wToH);

                bool compand = true;
                ResizeMode mode = ResizeMode.Stretch;
                var resizeOptions = new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(newW, newH),
                    Sampler = KnownResamplers.Lanczos2,
                    //Sampler = KnownResamplers.Bicubic,
                    Compand = compand,
                    Mode = mode
                };


                image.Mutate(x => x
                     .Resize(resizeOptions)
                     );
                var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                {
                    Quality = 50 //Use variable to set between 5-30 based on your requirements
                };
                image.Save(path, encoder);

                return SaveResult.Success;
            }
            catch(Exception e)
            {
                return SaveResult.Fail;
            }
        }
        public SaveResult GenerateThumbnailFromImage(string sourceFile, string thumbnailPath)
        {
            
            var image = SixLabors.ImageSharp.Image.Load(sourceFile);
            if (image.Height >= image.Width)
            {
                double wToH = (double)image.Width / (double)image.Height;
                int newW = 400;
                int newH = (int)(newW / (double)wToH);

                image.Mutate(x => x.Resize(newW, newH));
                image.Mutate(x => x.Crop(newW, newW));
            }
            else
            {
                double htoW = (double)image.Height / (double)image.Width;
                int newH = 400;
                int newW = (int)(newH / (double)htoW);

                image.Mutate(x => x.Resize(newW, newH));
                image.Mutate(x => x.Crop(newH, newH));
            }
            image.Save(thumbnailPath);
            return SaveResult.Success;
        }
        public async Task<SaveResult> SaveVideo(IFormFile file, string path)
        {
            if (file.Length >System.Convert.ToInt32( _configuration["ImageOption:MaxFileSize"]))
            {
                return SaveResult.BigFile;
            }
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return SaveResult.Success;
            }
            catch(Exception e)
            {
                return SaveResult.Fail;
            }
        }
        public async Task<SaveResult> SavePdf(IFormFile file, string path)
        {
            if (file.Length > System.Convert.ToInt32(_configuration["ImageOption:MaxFileSize"]))
            {
                return SaveResult.BigFile;
            }
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return SaveResult.Success;
            }
            catch (Exception e)
            {
                return SaveResult.Fail;
            }
        }
        public async Task<SaveResult> GenerateThumbnailFromVideo(string sourceFile, string thumbphotopath)
        {
            try
            {
                thumbphotopath = thumbphotopath.Replace(".jpg", ".png");
                IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(sourceFile, thumbphotopath
                              , ((await FFmpeg.GetMediaInfo(sourceFile)).VideoStreams.First().Duration).Add(new TimeSpan(0, 0, -2)));

                IConversionResult result = await conversion.Start();


                var image = SixLabors.ImageSharp.Image.Load(thumbphotopath);
                if (image.Height >= image.Width)
                {
                    double wToH = (double)image.Width / (double)image.Height;
                    int newW = 400;
                    int newH = (int)(newW / (double)wToH);

                    image.Mutate(x => x.Resize(newW, newH));
                    image.Mutate(x => x.Crop(newW, newW));
                }
                else
                {
                    double htoW = (double)image.Height / (double)image.Width;
                    int newH = 400;
                    int newW = (int)(newH / (double)htoW);

                    image.Mutate(x => x.Resize(newW, newH));
                    image.Mutate(x => x.Crop(newH, newH));
                }

                //image.Mutate(x => x.Resize(newW, newH));
                var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                {
                    Quality = 50 //Use variable to set between 5-30 based on your requirements
                };
                image.Save(thumbphotopath.Replace("png", "jpg"), encoder);
                System.IO.File.Delete(thumbphotopath);
                return SaveResult.Success;
            }
            catch(Exception e)
            {
                return SaveResult.Fail;
            }
        }
        public async Task<string> ConvertVideo(string filePath,string destinationPath, bool full)
        {
            IConversionResult convertresult;
            string result = "";
           
            FFmpeg.SetExecutablesPath(_configuration["VideoTools:FFMpeg"], "ffmpeg", "ffprobe");

            try
            {
                var path = FFmpeg.ExecutablesPath;
                var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                var videoStream = mediaInfo.VideoStreams.First();
                var audioStream = mediaInfo.AudioStreams.First();


                //Change some parameters of video stream
                int width = videoStream.Width, height = videoStream.Height;
                long bitrate = videoStream.Bitrate;
                if (bitrate > 900000)
                {
                    bitrate = 900000;
                }
                if (videoStream.Width > 480)
                {
                    width = 480;
                    height = (int)((double)height / (double)((double)videoStream.Width / (double)width));
                }
                if (videoStream.Rotation != null && (videoStream.Rotation == 90 || videoStream.Rotation == -90 ||
                    videoStream.Rotation == 270 || videoStream.Rotation == -270))
                {
                    int test = width;
                    width = height;
                    height = test;
                }
                if (width % 2 == 1)
                {
                    width++;
                }
                if (height % 2 == 1) height++;
                videoStream
                    .SetSize(width, height)
                    .SetBitrate(bitrate)
                    .SetCodec(VideoCodec.h264)
                    ;
                audioStream.SetCodec(AudioCodec.aac);
                TimeSpan interval=new TimeSpan();
                if (full)
                {
                    interval = videoStream.Duration;
                }
                else
                {
                    interval = (videoStream.Duration.TotalMinutes >
                        System.Convert.ToDouble(_configuration["VideoTools:MaxVideoDuration"])
                        ?
                        TimeSpan.FromMinutes(System.Convert.ToInt32(_configuration["VideoTools:MaxVideoDuration"]))
                        : videoStream.Duration);
                }
                var conversion = FFmpeg.Conversions.New()
                   .AddStream(videoStream)
                   .AddStream(audioStream)
                   .SetOutput(destinationPath)
                   .SetOverwriteOutput(true)
                   .UseMultiThread(false)
                   .SetPreset(ConversionPreset.Fast)
                   .SetOutputTime(interval);
                convertresult = await conversion.Start();
                result = result = convertresult.ToString();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            
            System.IO.File.Delete(filePath);
            

            return result;
        }
        public async Task<string> ConvertAudio(string filePath, string destinationPath)
        {
            IConversionResult convertresult;
            string result = "";

            FFmpeg.SetExecutablesPath(_configuration["VideoTools:FFMpeg"], "ffmpeg", "ffprobe");

            try
            {
                var path = FFmpeg.ExecutablesPath;
                var mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(mediaInfo.Path, destinationPath);


                convertresult=await conversion.SetAudioBitrate(64000).SetOutputFormat(Format.mp3).Start();
                System.IO.File.Delete(filePath);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}