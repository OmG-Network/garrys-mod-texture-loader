using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;


namespace Gmod_Texture_Loader
{
    class Program
    {
        public void ExtractTGZ(String gzArchiveName, String destFolder)
        {

            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

        static void Main(string[] args)
        {

          String tmp = Path.Combine(Path.GetTempPath(), "texture.tar.gz");
            using (var client = new WebClient())
            {
                Console.WriteLine("Download wird ausgeführt...");
                client.DownloadFile("http://fastdl.omg-network.de/gm/textures/textures.tar.gz", tmp);
                
            }
            Console.WriteLine("Download abgeschlossen");
            Console.WriteLine("Archiv wird extrahiert");
            Program extract = new Program();
            extract.ExtractTGZ(tmp, "C:\\Program Files (x86)\\Steam\\steamapps\\common\\GarrysMod\\garrysmod\\addons");
            Console.WriteLine("Texturen wurden installiert");

            if (File.Exists(tmp))
            {
                File.Delete(tmp);
            }

        }
    }
}
