# ConsoleApp-in-
## Desciption of the Project
This is a console application in c# language. The logic that this app implements is these: 1.When running without parameters: Checks the availability of sites from the list (for example, ya.ru, but the list of sites can be changed). Checks the availability of the postgresql DBMS server (the connection string is specified). The result of the last check is saved to a file (in JSON or XML format), and also sent by e-mail. 2. When running with any parameter: Outputs the result of the last check from the saved file to the console.

## Details
First, all the data required for the application to run successfully is all stored in the app config file. There is a connection string that connects to our Postgress database, stored in the connection string with a name DBS. Inside the appsettings, stored websites links that we use for checking it's availability. A file path to save our files. 
The saved files are xml files which contain the results of the checks performed on the availability of our websites. Information about sending email is also stored in the app settings. This is, who we sending the email toAddress, the person whom we send the email to, fromAddress is the person sending the email, then the subject and main body of the email. Logging files with the key name, MyLogFile has a value path to save all log performed in a file.
Serilog was used to perform our logs for the application, so nutGet packages were installed for this logging to be performed. NutGet packages installed are, Serilog, Serlog.sinks.console for logging to console, serilog.sinks.file for logging to a file. For emails, Smtp server was used. Papercut was also downloaded for sending the emails.

## NOTE
To run the application on your local machine, you will need to change the file path and log file path in the app config file.
You can create a folder, for example in documents (say App files) and copy the path of this folder and replace it with the value for FilePath.
NutGet Packages that needs to be installed Smtp server for sending emails, Papercut, Npgsql for working with postgress database, serilog, serilog.sink and serilog.sink.console.

## How the code works
First, Http client class was used to get the required url from appsettings. We check the availability of urls stored in config files, if they exist.
If they do, then we should get the status code 200 for a successful request. So on console it displays the results of all websites stored in appconfig and returns its status code.
Second, we checked the availability of a connection string to our postgress database. If there is such a connecting string, then the app connects to the database and additional checks are performed. It displays the first two roles from a customer table in our database.
Third, we stored the checks performed in an xml file. Then, we used smtp server to send the results of the check and attached it to our email and sent as an email.
Lastly, we performed a serilog to console and file.
           
