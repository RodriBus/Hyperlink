using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
internal class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    private const string MainProject = "Hyperlink";

    [Solution] private readonly Solution Solution;
    [GitRepository] private readonly GitRepository GitRepository;
    [GitVersion] private readonly GitVersion GitVersion;

    private AbsolutePath SourceDirectory => RootDirectory / "src";
    private AbsolutePath TestsDirectory => RootDirectory / "tests";
    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    private Target Clean => _ => _
         .Before(Restore)
         .Executes(() =>
         {
             SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
             TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
             EnsureCleanDirectory(ArtifactsDirectory);
         });

    private Target Restore => _ => _
         .Executes(() =>
         {
             DotNetRestore(s => s
                 .SetProjectFile(Solution));
         });

    private Target Compile => _ => _
         .DependsOn(Restore)
         .Executes(() =>
         {
             DotNetBuild(s => s
                 .SetProjectFile(Solution)
                 .SetConfiguration(Configuration)
                 .SetAssemblyVersion(GitVersion.AssemblySemVer)
                 .SetFileVersion(GitVersion.AssemblySemFileVer)
                 .SetInformationalVersion(GitVersion.InformationalVersion)
                 .EnableNoRestore());
         });

    private Target Publish => _ => _
     .After(Clean)
     .Executes(() =>
     {
         EnsureCleanDirectory(ArtifactsDirectory);
         DotNetPublish(_ => _
             .SetAssemblyVersion(GitVersion.AssemblySemVer)
             .SetFileVersion(GitVersion.AssemblySemFileVer)
             .SetInformationalVersion(GitVersion.InformationalVersion)
             .SetOutput(ArtifactsDirectory)
             .SetProject(Solution.GetProject(MainProject))
             .SetConfiguration(Configuration)
             .EnableSelfContained()
             .SetArgumentConfigurator(a => a
                .Add("/p:PublishSingleFile=true")
                )
         );
         DeleteFile(ArtifactsDirectory / "appsettings.Development.json");
     });
}