Task("Build")
    .Does(() =>
{
    var settings = new DotNetBuildSettings
    {
        Configuration = "Release",
        MSBuildSettings = new DotNetMSBuildSettings()
    };

    if (tag != null) 
    {
        settings.MSBuildSettings.Version = tag;
    }
    else if (buildId != 0)
    {
        settings.MSBuildSettings.VersionSuffix = "ci." + buildId;
    }

    DotNetBuild("./LevelImposter.csproj", settings);
});
