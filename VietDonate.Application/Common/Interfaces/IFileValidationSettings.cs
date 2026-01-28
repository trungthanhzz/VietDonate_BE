namespace VietDonate.Application.Common.Interfaces
{
    public interface IFileValidationSettings
    {
        long MaxFileSizeBytes { get; }
        string[] AllowedFileTypes { get; }
    }
}
