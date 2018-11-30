using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;
using System.Xml.Linq;
using System.Xml;

class DownloadFile
{

    static void Main(string[] args)
    {
        Console.BufferHeight = Int16.MaxValue - 1;
        Console.WindowHeight = Console.LargestWindowHeight;
        string folder = @"C:\\mobbyDick\\MobbyDick.txt";// url den aldığım veriyi yazdığım text dosyam (the folder that i got the text from url)
        string line = null;
        Console.WriteLine("Welcome This program download a file from web client");
        Console.WriteLine("Then counts how often each word occured.\nFinally save the result to an xml file");
        getContent(folder, line);//get all content from the webclient (webclient kullanarak url den isteğim içeriği alıyorum.) 
        getXml(folder, line);  //Verileri istediğim şekilde düzenleyip xml düzeyine getirip dosyasına kaydediyorum.
                                 // (modify textfile as an xml and save to xml file)


    }
     public static void getContent(string folder, string line)
     {
         int bufferSize = 256 * 1024 * 1024;
         FileStream streamfile = new FileStream(folder, FileMode.OpenOrCreate, FileAccess.Write);// filestream processors
             // i usu this for open folder and write text        
         WebClient client = new WebClient(); //webclient or we can use http client, i have choosen web client
         Stream stream = client.OpenRead("https://www.gutenberg.org/files/2701/2701-0.txt");//get content from the url with open and read method 
         BufferedStream bufferedStream = new BufferedStream(stream, bufferSize);
         StreamReader reader = new StreamReader(bufferedStream);// read content from stream
         StreamWriter streamwrite = new StreamWriter(streamfile, System.Text.Encoding.UTF8, 256*1024*1024);        
         try { 
         while ((line = reader.ReadLine()) != null)//while there is a line, write content to line array(string) 
         {
             streamwrite.WriteLine(line);
           

         }
            Console.WriteLine("************************");
            Console.WriteLine("text file downloaded.");

        }
        catch (IOException excepetion)
         {
             Console.WriteLine(excepetion);
         }


         streamwrite.Close();  
         File.AppendAllText(folder, line);//write all large text to folder
         

     }
    

     

    public static void getXml(string folder, string line)
    {
        

        StreamReader reader2 = new StreamReader(folder, System.Text.Encoding.UTF8, true, 256 * 1024 * 1024);
        string s = "";
        Array Result;
        List<string> list = new List<string>();
        while ((s = reader2.ReadLine()) != null)//while readline 
        {
            string[] words = reader2.ReadToEnd().Split();// split the words(kelimeleri böl)
            string[] lowercase = words.Select(s1 => s1.ToLowerInvariant()).ToArray();//all words to lowercase(küçük harflere çevir.)

            string excpList = "$*!./,;:?-[]'_\"|%&+^1234567890“—";

            Result = lowercase.Select(x => x.Except(excpList.ToArray()))// cut tthe specil char. (özel karakterleri kes)
                                     .Select(c => new string(c.ToArray()))
                                     .ToArray();


            foreach (string s3 in Result)
            {
                //Console.WriteLine(s3);   //test
                list.Add(s3.ToString());//içeriği listeye ekle add result to the list
              
            }
            Console.WriteLine("************************");
            Console.WriteLine("list created");


        }

        var result = list.GroupBy(r => r)
               .Select(grp => new
               {
                   Word = grp.Key,
                   Count = grp.Count()
               }).OrderByDescending(r=>r.Count);//listenin içeriğini sırala.short and count all words and save count

        

        XElement identity = new XElement("words");        //xml olustur. creat an xml element
        

        foreach (var item in result)
        {
            //Console.WriteLine(item);  //test
            XElement elm = new XElement("word",
              new XAttribute("Text", item.Word), // convert list to xml 
              new XAttribute("Count", item.Count));

            identity.Add(elm);

        }
        Console.WriteLine("************************");
        Console.WriteLine("list converted result to Xml Element.");

        XElement xml = new XElement("xml", identity);

        Console.WriteLine("************************");
        Console.WriteLine("xml has been created"); //test

       //Console.WriteLine("Press any key to exit.");
        //Console.ReadKey();

        XmlDocument xdoc = new XmlDocument();

        xdoc.LoadXml(xml.ToString());
        string folderxml = @"C:\\mobbyDick\\MobbyDick.xml";
        xdoc.Save(folderxml);
        Console.WriteLine("************************");
        Console.WriteLine("Xml File has saved to Folder");

        Console.WriteLine("************************");
        Console.WriteLine("All process Completed Pres a key To Quit console");
        Console.ReadKey();
        







    }

}
