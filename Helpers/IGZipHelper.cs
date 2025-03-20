using System.IO.Compression;
using System.Text;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers
{
    public interface IGZipHelper
    {

        public string CompressData(string CompressData);
        public string CompressDataUrl(string CompressData);
        public string DecompressData(string DecompressData);
        public string DecompressDataUrl(string DecompressData);

    }

    public class GZipHelper : IGZipHelper
    {
        public string CompressData(string CompressData)
        {
            try
            {
                string outputStr = "";
                byte[] inputBytes = Encoding.UTF8.GetBytes(CompressData);

                using (var outputStream = new MemoryStream())
                {
                    using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                        gZipStream.Write(inputBytes, 0, inputBytes.Length);

                    var outputBytes = outputStream.ToArray();

                    outputStr = Convert.ToBase64String(outputBytes);
                }

                return outputStr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DecompressData(string DecompressData)
        {
            var decompress = "";
            try
            {

                byte[] inputBytes = Convert.FromBase64String(DecompressData);

                using (var inputStream = new MemoryStream(inputBytes))
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var streamReader = new StreamReader(gZipStream))
                {
                    decompress = streamReader.ReadToEnd();


                }
                return decompress;
            }
            catch (Exception ex)
            {
                return decompress;
            }

        }

        public string CompressDataUrl(string CompressData)
        {
            try
            {
                string outputStr = "";
                byte[] inputBytes = Encoding.UTF8.GetBytes(CompressData);

                using (var outputStream = new MemoryStream())
                {
                    using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                        gZipStream.Write(inputBytes, 0, inputBytes.Length);

                    var outputBytes = outputStream.ToArray();

                    // Convertir a Base64 URL Safe
                    outputStr = Convert.ToBase64String(outputBytes)
                                        .Replace('+', '-')
                                        .Replace('/', '_')
                                        .TrimEnd('='); // Opcional: eliminar los caracteres de relleno '=' al final
                }

                return outputStr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string DecompressDataUrl(string DecompressData)
        {
            var decompress = "";
            try
            {

                // Revertir el Base64 URL Safe
                string base64 = DecompressData
                                .Replace('-', '+')
                                .Replace('_', '/');

                // Restaurar los caracteres de relleno '=' si es necesario
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }

                byte[] compressedBytes = Convert.FromBase64String(base64);

                using (var inputStream = new MemoryStream(compressedBytes))
                {
                    using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            gZipStream.CopyTo(outputStream);
                            byte[] outputBytes = outputStream.ToArray();
                            decompress = Encoding.UTF8.GetString(outputBytes);
                        }
                    }
                }

                return decompress;
            }
            catch (Exception ex)
            {

                return decompress;
            }

        }

    }
}
