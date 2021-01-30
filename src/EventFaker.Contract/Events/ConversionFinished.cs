namespace Madailei.ProcessManagement.Client.Contracts.Events
{
    class ConversionFinished
    {
        public ConversionFinished(
            string location,
            string name,
            string videoEncoding,
            string audioEncoding)
        {
            Location = location;
            Name = name;
            VideoEncoding = videoEncoding;
            AudioEncoding = audioEncoding;
        }

        public string Location { get; set; }
        public string Name { get; set; }
        public string VideoEncoding { get; set; }
        public string AudioEncoding { get; set; }
    }
}