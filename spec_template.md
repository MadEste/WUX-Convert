<!-- The purpose of this spec is to describe a new feature and
its APIs that make up a new feature in WinUI. -->

<!-- There are two audiences for the spec. The first are people
that want to evaluate and give feedback on the API, as part of
the submission process.  When it's complete
it will be incorporated into the public documentation at
docs.microsoft.com (http://docs.microsoft.com/uwp/toolkits/winui/).
Hopefully we'll be able to copy it mostly verbatim.
So the second audience is everyone that reads there to learn how
and why to use this API. -->


# Background
<!-- Use this section to provide background context for the new API(s) 
in this spec. -->
Xaml has many private dependencies and Microsoft is working to get to a purely public SDK. Xaml, Composition, and Input components of Windows are being lifted out of the OS and deployed in WinUI3. WinUI3 will include support for apps to run in the Desktop app model, not just the UWP app model.
WinUI3 has differences compared to WinUI2. The purpose of this project is to create a porting solution in the form of Code Analyzers and Fixes to allow allow Developers to convert files to the new WinUI3 wherever possible.

<!-- This section and the appendix are the only sections that likely
do not get copied to docs.microsoft.com; they're just an aid to reading this spec. -->

<!-- If you're modifying an existing API, included a link here to the
existing page(s) -->

<!-- For example, this section is a place to explain why you're adding this API rather than
modifying an existing API. -->

<!-- For example, this is a place to provide a brief explanation of some dependent
area, just explanation enough to understand this new API, rather than telling
the reader "go read 100 pages of background information posted at ...". -->


# Description
<!-- Use this section to provide a brief description of the feature.
For an example, see the introduction to the PasswordBox control 
(http://docs.microsoft.com/windows/uwp/design/controls-and-patterns/password-box). -->
This tool assists with the conversion process with custom Roslyn Analyzer and CodeFixs to convert existing UWP/WinUI2 C# projects to WinUI3.

In General, several types in the Windows.UI.Xaml namespace 
are now located in the Microsoft.UI.Xaml

Generate Diagnostics for old Windows Namespace Usings to the new Microsoft namespace


# Examples
<!-- Use this section to explain the features of the API, showing
example code with each description. The general format is: 
  feature explanation,
  example code
  feature explanation,
  example code
  etc.-->
  
<!-- Code samples should be in C# and/or C++/WinRT -->

Given the following code :
```
using System;
using Windows.UI;
using Windows.UI.Xaml;
namespace ExampleCode
{
    class Program
    {
        Windows.UI.Xaml.Controls.Frame rootFrame = Window.Current.Content as Microsoft.UI.Xaml.Controls.Frame;
    }
}
            
```

The CodeFix applied to the Solution:
```
using System;
using Windows.UI;
using Microsoft.UI.Xaml;
namespace ExampleCode
{
    class Program
    {
        Microsoft.UI.Xaml.Controls.Frame rootFrame = Window.Current.Content as Microsoft.UI.Xaml.Controls.Frame;
    }
}
            
```


# Conversion Process
<!-- Explanation and guidance on how to use the converter that doesn't fit into the Examples section. -->

1. Download and use WinUI packages in your app using the NuGet package manager: see the [Getting Started](https://docs.microsoft.com/uwp/toolkits/winui/getting-started) with the Windows UI Library page for more information.
2. Some conflicting Nuget packages must be uninstalled. (TODO: more detailed instructions)
3. Light Bulb suggestions should highlight issues that need to be updated for WinUI3 conversion. 
4. Click the down arrow by the lightbulb, Convert to WinUI3 and select Fix all occurences in project. 

![Visual Studio Lightbulb Suggestion](./assets/img1.png#thumb)

5. All namespace changes should be fixed in your project!

Note some WInUI2 resources are not compatible with WinUI3. These issues may be highlighted in code but cannot be fixed by the converter. 

# Appendix
<!-- Anything else that you want to write down for posterity, but 
that isn't necessary to understand the purpose and usage of the API.
For example, implementation details. -->
Testing process and design notes... To be added. 
<style>
img[src*="#thumb"] {
   margin-left:3%;
   width:50%;
   height:auto;
}
</style>