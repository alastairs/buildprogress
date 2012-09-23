# VS 2012 Taskbar Build Progress Add-In

## 1. Change Log

### 1.1

Adds support for Visual Studio 2012.

### v1.0

First major release.  Introduces support for Visual Studio 2008, and merges the 
Windows API Code Pack binaries into the source tree for the add-in.

### v0.3

Bug fix release.

Bug fix: #1: README inaccurate? 
Fixed by bdukes@130bb8ae88f190c85c12cc58411b993d2119ed8b

Bug fix: #2: unable to see progress indicator. 
Fixed by Kha@097d15127dfdafd5a2a0dae5e1fdcafbd57d9a59

### v0.21

Bug fix: Failed build icon overlay is not replaced with the successful build 
icon overlay when a successful build follows a failed build.

### v0.2

Overlay icons added.  A tick is displayed if the build was successful, a cross
if there was a failure.

### v0.1

Initial version.  Build progress is displayed in the Windows 7 task bar icon.  
If one or more of the projects contains an error, the progress bar will turn 
red.


## 2. Installation Instructions

Download the latest version of the build progress zip file from 
http://www.github.com/alastairs/buildprogress/downloads.  Inside the 
BuildProgress folder is a set of folders for VS2008, VS2010 and VS2012.  Choose 
the folder appropriate to your installation of Visual studio and copy the 
contents of that folder to:

  * My Documents\Visual Studio 2012\AddIns      or
  * My Documents\Visual Studio 2010\AddIns      or
  * My Documents\Visual Studio 2008\AddIns
  
depending on your installed version of Visual Studio.  You may need to create 
the AddIns directory if it does not already exist at the specified location.  

In many cases, you will need to "unblock" the binary files on Windows Vista and 
Windows 7 by right-clicking the file and choosing Properties, then clicking the 
Unblock button at the bottom of the General tab.  Screenshots and an explanation
of why this is required are available from 
http://www.hanselman.com/blog/FavIconsInternetZonesAndProjectsFromATrustworthySource.aspx


### 2.1 Installation to a UNC Share

If your My Documents folder is mapped to a network share (e.g., in some 
corporate environments), you will need to complete a one-time extra step to 
relax the security restrictions on the AddIns directory.  Run the Visual Studio
2010 Command Prompt as an Administrator and enter the following command, 
substituting servername, sharename and username for appropriate values for your
environment.  Note that username may need to be a folder hierarchy fragment, and
not just a username.

    > caspol -m -ag LocalIntranet_Zone -url \
      "\\servername\sharename\username\Visual Studio 2010\Addins\*" FullTrust -n  \
      "Visual Studio 2010 Add-ins" -d "Visual Studio 2010 Add-ins"



## 3. Build Instructions

If you would prefer to build the add-in yourself, you'll need to follow these
steps.

  1. Check out the source from GitHub, or use the download link on the 
     repository page.
  2. Open the solution file appropriate to your installation of Visual Studio:
       * BuildProgress.sln for Visual Studio 2012 or Visual Studio 2010
       * BuildProgress2008.sln for Visual Studio 2008
  3. Build the solution.  Drop the following files into
     My Documents\Visual Studio [Version]\AddIns, where [Version] should be 
	 replaced with your Visual Studio version (2008, 2010, 2012):
	   * BuildProgress.AddIn
	   * BuildProgress.dll
	   * Microsoft.WindowsAPICodePack.dll
	   * Microsoft.WindowsAPICodePack.Shell.dll     
  4. If you want to debug the Add-In, you will need to complete the following
     steps.
       a. In "BuildProgress - For Testing.AddIn", update the Extensibility/
          Addin/Assembly element to point to built binary in your build progress
          add-in development environment
       b. Place the "BuildProgress - For Testing.AddIn" file in 
	      My Documents\Visual Studio [Version]\AddIns, where [Version] should be
		  replaced with your Visual Studio version (2008, 2010, 2012).
	   c. Restart Visual Studio.
     

## 4. Credits

The overlay icons are taken from the free Silk icon set from [famfamfam.com](http://www.famfamfam.com/lab/icons/silk/).

The taskbar integration is provided by the [Windows API Codepack](http://code.msdn.microsoft.com/WindowsAPICodePack).

