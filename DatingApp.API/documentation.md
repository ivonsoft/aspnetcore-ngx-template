stages:
0. mkdir DatingApp, then cd
- create model, controller, Data, based on that we can create db scheleton
1. dotnet ef migrations add <nazwa>initialCreate -> create migration folder and files named initialCreate
2. dotnet ef database update -> run script and create db
3. dotnet restore and run
4. dodwanie dodatkowego modulu: dotnet add package Microsoft.AspNetCore.NodeServices: https://dotnetthoughts.net/using-node-services-in-aspnet-core/
, Microsoft.EntityFrameworkCore.Sqlite
embedded c++ dll: https://blog.quickbird.uk/calling-c-from-net-core-759563bab75d,
https://medium.com/@xaviergeerinck/how-to-bind-c-code-with-dotnet-core-157a121c0aa6 (not tested mac)
5. stworzenie folderu dla JS scripts: NodeModule, w tym folderze instalujesz np qr-script, node_modules, potrzebne do uruchomienia skryptow js




