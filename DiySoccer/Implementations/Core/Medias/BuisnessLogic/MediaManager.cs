using System;
using System.Drawing;
using System.IO;
using System.Web;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Interfaces.Core.Services.Medias.BuisnessLogic;
using Interfaces.Core.Services.Medias.DataAccess;

namespace Implementations.Core.Medias.BuisnessLogic
{
    public class MediaManager : IMediaManager
    {
        private const string MediaPrefix = "Media\\";
        private readonly IMediaRepository _mediaRepository;

        public MediaManager(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public MediaIOViewModel GetCutImage(string mediaId, int? width, int? height)
        {
            var media = _mediaRepository.Get(mediaId);
            if (media == null || string.IsNullOrEmpty(media.RelativeUrl))
                return null;

            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + media.RelativeUrl;
            var extenstion = Path.GetExtension(path);
            var pathWithoutExtension = path.Replace(extenstion, "");

            var targetPath = string.Empty;
            if (width.HasValue && height.HasValue)
            {
                targetPath = pathWithoutExtension + "_" + width + "_" + height + extenstion;
                if (!File.Exists(targetPath))
                    CreateImage(path, targetPath, new Size(width.Value, height.Value), true);
            }
            else
            {
                targetPath = pathWithoutExtension;
                if (!File.Exists(targetPath))
                    CreateImage(path, targetPath, true);
            }

            if (!File.Exists(targetPath))
                return null;
            
            return new MediaIOViewModel
            {
                Stream = new FileStream(targetPath, FileMode.Open),
                ContentType = media.ContentType
            };
        }

        public MediaIOViewModel GetImage(string mediaId, int? width, int? height)
        {
            var media = _mediaRepository.Get(mediaId);
            if (media == null || string.IsNullOrEmpty(media.RelativeUrl))
                return null;

            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + media.RelativeUrl;
            var extenstion = Path.GetExtension(path);
            var pathWithoutExtension = path.Replace(extenstion, "");

            var targetPath = string.Empty;
            if (width.HasValue && height.HasValue)
            {
                targetPath = pathWithoutExtension + "_" + width + "_" + height + "_uncut" + extenstion;
                if (!File.Exists(targetPath))
                    CreateImage(path, targetPath, new Size(width.Value, height.Value), false);
            }
            else
            {
                targetPath = pathWithoutExtension + "_uncut";
                if (!File.Exists(targetPath))
                    CreateImage(path, targetPath, false);
            }

            if (!File.Exists(targetPath))
                return null;

            return new MediaIOViewModel
            {
                Stream = new FileStream(targetPath, FileMode.Open),
                ContentType = media.ContentType
            };
        }

        private void CreateImage(string sourcePath, string targetPath, bool cut)
        {
            using (var fileStream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] photoBytes = File.ReadAllBytes(sourcePath); // change imagePath with a valid image path
                int quality = 70;
                var format = new PngFormat(); // we gonna convert a jpeg image to a png one

                using (var inStream = new MemoryStream(photoBytes))
                {
                    using (var outStream = new MemoryStream())
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (var imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            // Do your magic here
                            if (cut)
                            {
                                imageFactory.Load(inStream)
                                    .RoundedCorners(new RoundedCornerLayer(190, true, true, true, true))
                                    .Format(format)
                                    .Quality(quality)
                                    .Save(outStream);
                            }
                            else
                            {
                                imageFactory.Load(inStream)
                                    .Format(format)
                                    .Quality(quality)
                                    .Save(outStream);
                            }

                            outStream.WriteTo(fileStream);
                        }
                    }
                }
            }
        }

        private void CreateImage(string sourcePath, string targetPath, Size size, bool cut)
        {
            if (!File.Exists(sourcePath))
                return;

            using (var fileStream = new FileStream(targetPath, FileMode.Create))
            {
                byte[] photoBytes = File.ReadAllBytes(sourcePath); // change imagePath with a valid image path
                int quality = 70;
                var format = new PngFormat(); // we gonna convert a jpeg image to a png one

                using (var inStream = new MemoryStream(photoBytes))
                {
                    using (var outStream = new MemoryStream())
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (var imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            // Do your magic here
                            if (cut)
                            {
                                imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Quality(quality)
                                    .Save(outStream);
                            }
                            else
                            {
                                imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Quality(quality)
                                    .Save(outStream);
                            }

                            outStream.WriteTo(fileStream);
                        }
                    }
                }
            }
        }

        public MediaViewModel Upload(HttpPostedFile file)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + MediaPrefix;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            var prefix = string.Empty;
            var count = 0;
            while (File.Exists(path + prefix + file.FileName))
            {
                count++;
                prefix = count + "_";
            }

            var filePath = path + prefix + file.FileName;
            try
            {
                file.SaveAs(filePath);
            }
            catch (Exception)
            {
                return null;
            }

            var relativePath = MediaPrefix + prefix + file.FileName;

            var entity = new MediaDb
            {
                Name = file.FileName,
                ContentType = file.ContentType,
                RelativeUrl = relativePath
            };
            var media = _mediaRepository.Add(entity);

            return new MediaViewModel
            {
                Id = media.EntityId,
                Url = relativePath
            };
        }
    }
}
