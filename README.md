# StealAllTheCats
A CRUD Rest API that communicates with Cats as a Service API.

This project was build using the following tools:
- .Net 8.0
- Visual Studio 2022
- Microsoft SQL Server Management Studio - 20.2.1
- Microsoft SQL Server 2019 LocalDB

## Install & Run
- clone the source code from <code>https://github.com/jimbek/StealAllTheCats.git</code>
- open the project through <b>Visual Studio</b>
- right-click on the project and select <b>Manage User Secrets</b>
- browse to path <b>%APPDATA%\Microsoft\UserSecrets\a16d7e49-e7e5-4ded-bab1-6d9bbfa7d003\secrets.json</b>
- paste <code>{ "ConnectionStrings": { "Db": "Your db connection string" }, "ApiKey": "Your API key" }</code> to the <code>secrets.json</code> file
- run the app using <b>IIS Express</b>
- the new database and the tables will be created automatically