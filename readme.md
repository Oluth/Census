# Important Notes

* This project is deployed on *.NET Framework 3.5*. You can download the respective executable [here from Microsoft](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks). Note that build attempts with newer frameworks than 4.6.1 have proven to fail in Cities: Skylines *(Ver. 1.14.0-f4, January 2022)*.
* In order to enhance your compilation workflow, it is recommended to add the following batch script as your post-build instruction:  
     *        mkdir "%LOCALAPPDATA%\Colossal Order\Cities_Skylines\Addons\Mods\$(SolutionName)"
            del /Q "%LOCALAPPDATA%\Colossal Order\Cities_Skylines\Addons\Mods\$(SolutionName)\*"
            xcopy /y "$(TargetPath)" "%LOCALAPPDATA%\Colossal Order\Cities_Skylines\Addons\Mods\$(SolutionName)" 
* *If you do not use the original Census.csproj:* Keep in mind that you need to manually change a line in your *XXX.csproj* file: *<Deterministic>false</Deterministic>* -> *<Deterministic>true</Deterministic>*. This is in order to avoid conflict of your local Visual Studio Code project with the *Census/Properties/AssemblyInfo.cs* that includes a non-deterministic versioning in order to support [DLL hot-swapping in Cities: Skylines](https://skylines.paradoxwikis.com/Advanced_Mod_Setup#Automatic_reloading).
