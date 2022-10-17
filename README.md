# ConsoleApp-in-
## Desciption of the Project
This is a console Application in c# language. The logic that ths app implements are these:
1.When running without parameters:
Checks the availability of sites from the list (for example, ya.ru, but the list of sites can be changed).
Checks the availability of the PostgreSQL DBMS server (the connection string is specified).
The result of the last check is saved to a file (in JSON or XML format), and also sent by e-mail.
2.	When running with any parameter:
Outputs the result of the last check from the saved file to the console.

## Details
First, all data required for the application to run successfully are all stored in the app config file. There is a connection string that connects to our Postgress database,  stored in the connection string with a name DBS. Inside the appsettings, stored websites links that we used for checking it's availability. 
A file path to save our files. The saved files are xml files which contains the results of the checks performed on the availablitiy of our websites. Information about sending email is also stored in appsettings. This is, who we sending the email toAddress the person whom we send the email to, fromAdress is the person sending the email, then the subject and main body of the email.
Logging files with the key name, MyLogFile has a value path to save all log performed in a file. Serilog was used to perform our logs for the application, so nutGet packages were installed for this logging to be performed.
NutGet packages installed are, Serilog, Serlog.sinks.console for logging to console, serilog.sinks.file for logging to a file.
For emails, Smtp server was used. Papercut was also downloaded for sending the emails. 

## NOTE
To run the application on your local machine, please you will need to change the file path and log file path in app config file.
You can a folder, for example in documents (say App files) and copy the path of this folder and replace with the value for FilePath. 
NutGet Packages that needs to be installed Smtp server for sendig emails,Papercut, Npgsql for working with postgress database,serilog, serilog.sink.file, and serilog.console.

## How the code works
First, Http client class was used get the required url from appsettings. We check the availabilty of urls stored in config files if they exist.
If they do, then we should get the status code 200 for a successful request. So on console it displays the results of all websites stored in appconfig and return its status code.
Second, we checked the availabilty of a connection string to our postgress database. If there is a such a connecting string then the app connects to the database and additional checks were performed, it displays first two roles from customer table in our database.
Third, is we stored the checks performed in an xml file.Then, we used smtp server to send the results of the check and attached it to our email and send as an email. 
Lastly, we performed a serilog to console and file.

           
