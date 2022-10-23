namespace ARTBEE_API.Helper
{
    public class FileHead
    {
        public static byte[] ReadFileHead(IFormFile file)
        {
            if (file is null)
            {
                return null;
            }
            using var fs = new BinaryReader(file.OpenReadStream());
            var bytes = new byte[20];
            fs.Read(bytes, 0, 20);
            return bytes;
        }
    }
}
