
rem skeleton project
md UqsWeather
cd UqsWeather
dotnet new sln
dotnet new webapi -o Uqs.Weather -f net6.0
dotnet sln add Uqs.Weather
rem dotnet add package AdamTibi.OpenWeather
dotnet add package OpenWeather.Net --version 1.0.0
rem  dotnet add package OpenWeather --version 2.0.0  otra version 

rem Tests projects
rem dotnet new console -o Uqs.Weather.TestRunner
rem dotnet sln add Uqs.Weather.TestRunner
rem dotnet add Uqs.Weather.TestRunner reference Uqs.Weather

rem adding Weather
dotnet new xunit -o Uqs.Weather.Tests.Unit -f net6.0
dotnet sln add .\Uqs.Weather.Tests.Unit\
dotnet add Uqs.Weather.Tests.Unit reference Uqs.Weather


rem dotnet new xunit -o Uqs.Weather.Tests.Integration   -f net6.0
rem dotnet sln add    .\Uqs.Weather.Tests.Integration\ 
rem dotnet     add      Uqs.Weather.Tests.integration  reference  Uqs.Weather
rem dotnet new xunit -o Uqs.Weather.Tests.Sintegration  -f net6.0
rem dotnet sln add    .\Uqs.Weather.Tests.Sintegration\
rem dotnet     add      Uqs.Weather.Tests.Sintegration reference  Uqs.Weather


