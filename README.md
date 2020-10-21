# SaongroupCRUDTest

<!-- TABLE OF CONTENTS -->
### Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Installation](#installation)
* [Usage](#usage)
* [Contact](#contact)


<!-- ABOUT THE PROJECT -->
### About The Project

![PreviewApp](https://snipboard.io/hoPXyq.jpg)

This is a basic CRUD Application that consists in two completly separated projects in the same solution. One of them is a Web-API wich contains 
all the logic business of the application, the second project is a MVC Web Application as a FrontEnd consuming that Web API to get the data and manipulate it
in order to satisfy all the functionality  from the Web API.

### Built With
* [.Net Core](https://dotnet.microsoft.com/download)
* [EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.0-rc.2.20475.6)
* [SQLite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.0-rc.2.20475.6)
* [Bootstrap](https://getbootstrap.com)
* [JQuery](https://jquery.com)
* [JQueryDataTable](https://datatables.net/)
* [ClosedXML](https://www.nuget.org/packages/ClosedXML/)
* [TempusDominus](https://tempusdominus.github.io/bootstrap-4/)
* [SweetAlert2](https://sweetalert2.github.io/)
* [MomentJS](https://momentjs.com/)

<!-- GETTING STARTED -->
### Getting Started

### Prerequisites

* .Net Core 3.1+
* .Net Standard 2.0+
* Visual Studio or Visual Studio Code to test the solution.

If you are using Visual Studio Code: type the next code at the root folder:
```
dotnet restore
```
### Installation

Once you have the solution downloaded you will just have to wait to the nutget package manager finish to download all the 
packages at the solution, this wouln't take too long and it's possibly you may not even realized when it's done.

##### Ports:  The projects use the ports "51155" and "51157" so if you are using this ports in another application, this may cause a problem launching at the solution. 
In order to solve this you will have to: 
* change both ports in the file "launchSettings.json" in both projects.
* change the port in the file "appsettings.json" in FrontEnd project using the new port you set in BackEnd project.

<!-- USAGE EXAMPLES -->
### Usage

To launch the solution you will have to enable to start multiple projects at the same time.

In order to achieve this:  
Go to Solution properties → Common Properties → Startup Project →  select Multiple startup projects → Select BackEnd at the top to start first and FrontEnd at the bottom

![Start Multiple projects](https://snipboard.io/5DHIiJ.jpg)

<!-- CONTACT -->
### Contact

Juan Carlos Erroa Molina - [@Linkedin](https://www.linkedin.com/in/juan-erroa-3432131b8/) - juanerroa@outlook.com
