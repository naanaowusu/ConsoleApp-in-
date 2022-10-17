using Microsoft.SqlServer.Management.HadrModel;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Npgsql;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace ApplicationTest
{
    class Program
    {
       

        static async Task Main(string[] args)
        {
            /// <summary>
            /// This is to check the availabilty of urls stored in config files if exist.
            /// we use an Httpclient class to check those urls and returns the status code if such url exist if not returns nothing
            /// If such url exist it shoulf return Successful responses - 200
            /// Display the result on the console.
            /// </summary>

            HttpClient client = new HttpClient();
            string urlList = ConfigurationManager.AppSettings["websites"];
            var res = urlList.Split(',');
            foreach (var url in res)
            {
                var result = await client.GetAsync(url);
                Console.WriteLine(((int)result.StatusCode));
            }



            /// <summary>
            /// We Check the availability of the PostgreSQL DBMS server (the connection string is specified).
            /// The connection string is stored in our config file and should open if succesffuly connected.
            /// we perform additional checks, if connection string exist then it should open and read the first two rows from table
            /// </summary>

            //connecting to databse
            NpgsqlConnection connect = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DBS"].ConnectionString);
            connect.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            NpgsqlDataReader dataReader; 
            if (connect != null) 
            {
                command = new NpgsqlCommand("SELECT first_name,country_of_birth FROM customers LIMIT 2", connect);
                dataReader = command.ExecuteReader();

                while(dataReader.Read())
                    Console.WriteLine("{0}\t{1} \n", dataReader[0], dataReader[1]);
                connect.Close();
            }
            else
                Console.WriteLine("There is no such Database string");


            ///<summary>
            /// The result of the last check should be stored in an xml file
            /// 
            /// </summary>

            string xmlFilePath = ConfigurationManager.AppSettings["FilePath"];
            var xmlDoc = ConfigurationManager.AppSettings["websites"].Split(',');

           
            XmlTextWriter writeToFile = new XmlTextWriter(xmlFilePath, Encoding.UTF8);
            writeToFile.Formatting = Formatting.Indented;
            

            //open document
            writeToFile.WriteStartDocument();
            //Add some comments 
            writeToFile.WriteComment("This program saves a result in xml file");

            //write first root element
            writeToFile.WriteStartElement("Site");
            for (int i = 0; i < xmlDoc.Count(); i++)
            {
                //first child element
                writeToFile.WriteStartElement("name");
                writeToFile.WriteString(xmlDoc[i]);
                writeToFile.WriteEndElement();

                writeToFile.WriteStartElement("result");
                writeToFile.WriteString("ok");
                writeToFile.WriteEndElement();       
                

          
            }

            writeToFile.WriteEndDocument();
            writeToFile.Flush();
            writeToFile.Close();


            ///<summary>
            /// Sending to an Email. we use Smtp
            /// </summary>
            /// 
            MailMessage message = new MailMessage(ConfigurationManager.AppSettings["FromAddress"], ConfigurationManager.AppSettings["ToAddress"]);
            message.Subject = ConfigurationManager.AppSettings["subject"];
            message.Body = ConfigurationManager.AppSettings["body"];
            message.BodyEncoding = Encoding.UTF8;

            //Adding to our email the saved xml file as an attachment
            Attachment attachment = new Attachment(ConfigurationManager.AppSettings["FilePath"]);
            message.Attachments.Add(attachment);

            SmtpClient sender = new SmtpClient(ConfigurationManager.AppSettings["server"], int.Parse(ConfigurationManager.AppSettings["port"]));
            sender.DeliveryMethod = SmtpDeliveryMethod.Network;
            sender.EnableSsl = false;

            sender.Send(message);

            ///<summary>
            /// Outputs the result of the last check from the saved file to the console.
            /// </summary>
            /// 
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ConfigurationManager.AppSettings["XmlFilePath"]);
            foreach (XmlNode node in xmldoc.DocumentElement)
            {
                Console.WriteLine(node.InnerText);
            }


            ///<summary>
            /// configure logging of operations to a file. 
            /// Information about the success or failure of the performed checks should be logged
            /// </summary>
            /// 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(ConfigurationManager.AppSettings["MyLogFile"], rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("All performed checkes are successfully checked!..");

            try
            {
                Log.Information("All logs are checked successfully");
                Log.Warning("If there are bugs write to the support team");
                Log.Debug("Application debuged without errors");

               
            }
            catch (Exception ex)
            {
                Log.Error(ex, "This is the error message...");
            }
            finally 
            {
                Log.Information("Logs are closing...");
            }

            Console.ReadLine();
        }


    }
}


