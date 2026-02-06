using Microsoft.Extensions.Options;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Configurations;

namespace VietDonate.Infrastructure.Common.Storage
{
    public class FileValidationSettings : IFileValidationSettings
    {
        private readonly S3Config _config;

        public FileValidationSettings(IOptions<S3Config> config)
        {
            _config = config.Value;
        }

        public long MaxFileSizeBytes => _config.MaxFileSizeBytes;
        public string[] AllowedFileTypes => _config.AllowedFileTypes;
    }
}
