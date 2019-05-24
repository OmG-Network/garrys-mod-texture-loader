using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Threading;

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

        
        class DownloadWithProgess
        {
            private volatile bool _completed;

            public void DownloadFile(string address, string location)
            {
                WebClient client = new WebClient();
                Uri Uri = new Uri(address);
                _completed = false;

                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                client.DownloadFileAsync(Uri, location);

            }

            public bool DownloadCompleted { get { return _completed; } }

            private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
            {
               
                Console.WriteLine("{0}    downloaded {1} von {2} bytes. {3} % fertig...",
                    (string)e.UserState,
                    e.BytesReceived,
                    e.TotalBytesToReceive,
                    e.ProgressPercentage);
            }

            private void Completed(object sender, AsyncCompletedEventArgs e)
            {
                if (e.Cancelled == true)
                {
                    Console.WriteLine("Der Download wurde abgebrochen.");
                   
                }
                else
                {
                    Console.WriteLine("Download komplett!");
                    //Entpacken hier 
                    Console.WriteLine("Archiv wird extrahiert");
                    Program extract = new Program();
                    string tmp = Path.Combine(Path.GetTempPath(), "texture.tar.gz");
                    extract.ExtractTGZ(tmp, "C:\\Program Files (x86)\\Steam\\steamapps\\common\\GarrysMod\\garrysmod\\addons");
                    Console.WriteLine("Texturen wurden installiert");
                }

                _completed = true;
            }
        }
        static void Main(string[] args)
        {


            //Einfache ASync Download anweisung, die TMP datei befindet sich im Exe Verzeichnis!
            DownloadWithProgess DGF = new DownloadWithProgess();

            string myUri = "http://fastdl.omg-network.de/gm/textures/textures.tar.gz";
            string tmp = Path.Combine(Path.GetTempPath(), "texture.tar.gz");
            DGF.DownloadFile(myUri, tmp);

            while (!DGF.DownloadCompleted)
                Thread.Sleep(1000);

        }


    }
}
