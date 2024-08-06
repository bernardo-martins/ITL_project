cd vue-project
cmd /C sudo npm install
cmd /C npm run build
cd ..\Anikatze.Webapi
dotnet restore --no-cache
:start
dotnet watch run
goto start


cache deaktivieren