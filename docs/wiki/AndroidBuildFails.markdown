Building on Android fails due to a java/google spat:
https://forum.unity.com/threads/android-java-lang-nosuchmethoderror-com-android-prefs-androidlocation-getavdfolder.505537/#post-3296018

Java 9 SDK: Fails at Detecting Current SDK Platform Version (?) because of avdmanager list target -c 

I get the same problem running avdmanager on the command line

1.8 SDK: advmanager is ok, it Detects Current SDK Platform Version, but fails at Detect SDK Build Tools Version with 
Win32Exception: ApplicationName='/Library/Java/JavaVirtualMachines/jdk-9.0.1.jdk/Contents/Home/bin/java', CommandLine='-Xmx2048M -Dcom.android.sdkmanager.toolsdir="/Users/rafael/Library/Android/sdk/tools" -Dfile.encoding=UTF8 -jar "/Applications/Unity/PlaybackEngines/AndroidPlayer/Tools/sdktools.jar" -', CurrentDirectory='/Users/rafael/Documents/dev/repos/einmaleins'
System.Diagnostics.Process.Start_noshell (System.Diagnostics.ProcessStartInfo startInfo, System.Diagnostics.Process process)
System.Diagnostics.Process.Start_common (System.Diagnostics.ProcessStartInfo startInfo, System.Diagnostics.Process process)
System.Diagnostics.Process.Start ()
(wrapper remoting-invoke-with-check) System.Diagnostics.Process:Start ()
UnityEditor.Utils.Program.Start (System.EventHandler exitCallback) (at /Users/builduser/buildslave/unity/build/Editor/Mono/Utils/Program.cs:44)
UnityEditor.Utils.Program.Start () (at /Users/builduser/buildslave/unity/build/Editor/Mono/Utils/Program.cs:28)
UnityEditor.Android.Command.Run (System.Diagnostics.ProcessStartInfo psi, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.RunCommandInternal (System.String javaExe, System.String sdkToolsDir, System.String[] sdkToolCommand, Int32 memoryMB, System.String workingdir, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.RunCommandSafe (System.String javaExe, System.String sdkToolsDir, System.String[] sdkToolCommand, Int32 memoryMB, System.String workingdir, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.RunCommand (System.String[] sdkToolCommand, Int32 memoryMB, System.String workingdir, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.RunCommand (System.String[] sdkToolCommand, System.String workingdir, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.RunCommand (System.String[] sdkToolCommand, UnityEditor.Android.WaitingForProcessToExit waitingForProcessToExit, System.String errorMsg)
UnityEditor.Android.AndroidSDKTools.GetSDKBuildToolsDir ()
UnityEditor.Android.AndroidSDKTools.UpdateToolsDirectories ()
UnityEditor.Android.PostProcessor.Tasks.CheckAndroidSdk.Execute (UnityEditor.Android.PostProcessor.PostProcessorContext context)
UnityEditor.Android.PostProcessor.PostProcessRunner.RunAllTasks (UnityEditor.Android.PostProcessor.PostProcessorContext context)
UnityEditor.Android.PostProcessAndroidPlayer.PrepareForBuild (BuildOptions options, BuildTarget target)
UnityEditor.Android.AndroidBuildPostprocessor.PrepareForBuild (BuildOptions options, BuildTarget target)
UnityEditor.PostprocessBuildPlayer.PrepareForBuild (BuildOptions options, BuildTargetGroup targetGroup, BuildTarget target) (at /Users/builduser/buildslave/unity/build/Editor/Mono/BuildPipeline/PostprocessBuildPlayer.cs:87)
UnityEditor.BuildPlayerWindow:BuildPlayerAndRun()

Note that it seems to be looking for the v9 SDK!


If I have both installed, the 9 is always used, leading to the first error (even if the Java path in unity is set to the other version)

jenv doesn’t change the fact that when I run java —version, it says v9, and this also doesn’t stop unity from using the other version. However, if I start a shell after setting up jenv, I *can* run avdmanager from the command line

What seems to have worked is to set JAVA_HOME with export JAVA_HOME=pathtojavasdk1.8 , then uninstalling java9. :-O
