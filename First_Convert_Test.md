## Error List on First run


Error	CS0029	Cannot implicitly convert type 'AppUIBasics.NavigationRootPage' to 'Microsoft.UI.Xaml.UIElement'	

Error	CS0030	Cannot convert type 'Microsoft.UI.Xaml.UIElement' to 'AppUIBasics.NavigationRootPage'	

Error	CS0104	'LaunchActivatedEventArgs' is an ambiguous reference between 'Windows.ApplicationModel.Activation.LaunchActivatedEventArgs' and 'Microsoft.UI.Xaml.LaunchActivatedEventArgs'	

Error	CS0115	'App.OnActivated(IActivatedEventArgs)': no suitable method found to override

Error	CS0263	Partial declarations of 'App' must not specify different base classes

Error	CS1061	'App' does not contain a definition for 'Suspending' and no accessible extension method 'Suspending' accepting a first argument of type 'App' could be found (are you missing a using directive or an assembly reference?)	

Error	CS8121	An expression of type 'UIElement' cannot be handled by a pattern of type 'NavigationRootPage'.

### After clean and Rebuild - Same

### After Close and Reopen

Errors CS0104, CS161

CS0104 - 
1. Microsoft.UI.Xaml.LaunchActivatedEventArgs

    new Error CS1503 - await EnsureWindow(args.UWPLaunchActivatedEventArgs)

2. targetPageArguments = ((Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)args).Arguments;

### Delete 2 Ink Pages that caused Errors

### Errors
1. CS0104 - remove System.Windows.Input; to solve ambiguity
2. Microsoft.UI Microsoft.UI.Composition changed (was not in )
3. scrollViewer = Helper.UIHelper.GetDescendantsOfType<ScrollViewer>(AssociatedObject).FirstOrDefault(); -> Common.UIHelper....

### VariedImageSizeLayout 
Removed?


 - ImageScrollBehavior... interface still expects Windows.UI.Xaml... should be microsoft, so it is failing...

 Uninstall Microsoft.Xaml.Behaviors.Uwp.Managed
 - Use Microsoft.Xaml.Behaviors.WinUI.Managed using Microsoft.Xaml.Interactivity; -Ibehavior
