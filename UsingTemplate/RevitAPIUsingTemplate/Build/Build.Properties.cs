partial class Build
{
    readonly string[] Projects =
    {
        "MainApp"
    };

    public const string InstallerProject = "Installer";

    public const string BuildConfiguration = "Release";
    public const string InstallerConfiguration = "Installer";
    public const string BundleConfiguration = "";

    const string AddInBinPrefix = "AddIn";
    const string ArtifactsFolder = "output";

    //Specify the path to the MSBuild.exe file here if you are not using VisualStudio
    const string CustomMsBuildPath = @"C:\Program Files\JetBrains\JetBrains Rider\tools\MSBuild\Current\Bin\MSBuild.exe";
}