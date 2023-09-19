using Entities.DTO.UserDto;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace Forum.Extensions
{
    public static class ForumAvatarHelper
    {
        public static string LoadAvatar(this ForumUserDto? forumUserDto, string webRootPath)
        {
            string imageName = forumUserDto.UserName.Trim() + "_" + forumUserDto.Id + ".jpg";
            string filePath = Path.Combine(webRootPath, "images", "avatars", imageName);

            if (!string.IsNullOrEmpty(forumUserDto.UserName))
            {
                if (File.Exists(filePath))
                {
                    filePath = "~/images/avatars/" + imageName;
                }
                else
                {
                    filePath = "~/images/avatars/EmptyAvatar.jpg";
                }
            }

            return filePath;
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}