namespace DocumentManagement.Rest.Api
{
    public class AppSettings
    {
        public string UploadFolderPath { get; set; }    
        public string[] AllowedExtensions { get; set; }
        public short? ResolutionDateNumOfDays { get; set; }  
    }
}
