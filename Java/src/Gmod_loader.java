import java.io.File;
import java.io.IOException;
import java.util.Scanner;
import org.rauschig.jarchivelib.Archiver;
import org.rauschig.jarchivelib.ArchiverFactory;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author [OmG] Network
 */
public class Gmod_loader {

    /**
     * @param args the command line arguments
     * @throws java.io.IOException
     */
    public static void main(String[] args) throws IOException {
        
        Scanner s = new Scanner(System.in);
        
        System.out.println("Garrys Mod Texture Patcher");
        
        // Pfad eingabe
        
        System.out.print("Wurde Garrys Mod auf einer anderen Festplatte oder außerhalb des Standard Pfades installiert ? Y/N");
        String answer = s.next();
        String path = null;
        
       if ("y".equals(answer)){
            
            System.out.println("Bitte geben sie den Speicherpfad des Addon Ordners an");
            path = s.next();
            
        } else if ("n".equals(answer)){
            
            if (!"Windows ([0-7])".contains(System.getProperty("os.name"))){
                
                System.out.println("Mometan gib es keinen Standardpfad für " + System.getProperty("os.name")+" bitte gebe den Pfad manuell ein");
                System.exit(0);
       
            }

           /* if ("Linux".contains(System.getProperty("os.name"))){
                
                System.out.println("Mometan gib es keinen Standardpfad für " + System.getProperty("os.name")+" bitte gebe den Pfad manuell ein");
                System.exit(0);
       
            }*/
            path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\GarrysMod\\garrysmod\\addons";
            System.out.println("Der Speicherpfad von Garrys Mod wurde nicht verändert");
            System.out.println(path);
            
            
        }
        if (!"y".equals(answer) || "n".equals(answer)){
                System.exit(0);
    } 
       
        // Suche tmp Ordner
        
         File temp = File.createTempFile("temp-file-name", ".tmp");
         String absolutePath = temp.getAbsolutePath();
         String tempFilePath = absolutePath.substring(0,absolutePath.lastIndexOf(File.separator));
        
        
        // Starte Download
       
        downloader dl = new downloader();
        
        String fileURL = "http://fastdl.omg-network.de/gm/textures/textures.tar.gz";
        String saveDir = tempFilePath;
        
        System.out.println("Downloadvorbereitungen wurden abgeschlossen möchtest du starten ?");
        System.out.println("Beliebige eingabe um fortzufahren");
        s.next();
        
        try {
            downloader.downloadFile(fileURL, saveDir);
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        System.out.println("Der Download wurde abgeschlossen");
        
       // Extract Tar GZ
       
       System.out.println("Starte Entpack vorgang");
       
       File archive = new File(tempFilePath+"/textures.tar.gz");
       
       File destination = new File(path);

       Archiver archiver = ArchiverFactory.createArchiver("tar", "gz");
       archiver.extract(archive, destination);
       System.out.println("Entpacken abgeschlossen");
       System.out.println("Installation abgeschlossen");
       
       // Lösche Archiv
       archive.delete();
    }
    
}
