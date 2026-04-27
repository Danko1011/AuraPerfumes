using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AuraPerfumes.Services
{
    public class CloudinaryImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string?> UploadImageFromUrlAsync(string imageUrl, string publicId)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return null;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageUrl),
                PublicId = publicId,
                Folder = "AuraPerfumes"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString();
        }

        public async Task<string?> UploadImageFileAsync(IFormFile file, string publicId)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Folder = "AuraPerfumes"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString();
        }
    }
}
