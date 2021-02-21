# Command Console
Unity Package: A debug command console that can be used at runtime and easily extendable.

## How to install
This package can be installed through the Unity `Package Manager`

Open up the package manager `Window/Package Manager` and click on `Add package from git URL...`.

![unity_package_manager_git_drop_down](Documentation~/images/unity_package_manager_git_drop_down.png)

Paste in this repository's url.

`https://github.com/Jason-Skillman/Command-Console.git`

![unity_package_manager_git_with_url](Documentation~/images/unity_package_manager_git_with_url.png)

Click `Add` and the package will be installed in your project.

---
**NOTE:** For Unity version 2019.2 or lower

If you are using Unity 2019.2 or lower than you will not be able to install the package with the above method. Here are a few other ways to install the package.
1. You can clone this git repository into your project's `Packages` folder.
1. Another alternative would be to download this package from GitHub as a zip file. Unzip and in the `Package Manager` click on `Add package from disk...` and select the package's root folder.

---

## How to setup
You can create a new console and add it to your scene by right clicking in the hierarchy `"Console/Command Console"`.
Only one command console should exist within any given scene. 
The `EventSystem` must also be present in your scene.

To open the console at runtime use the tilde key `~`. This can be disabled in the inspector for custom input remaping.

## API
Custom commands can be written for the command console.

A small list of commands have already been written as examples. Some command examples include print, load scene and unload scene. They can be found at `Runtime/Scripts/Commands` starting at the root of this package.

### ICommand
To create a custom command create a new script and extend the ICommand interface. The console manager will automaticly detect the script and add it to the command list at runtime.

|Property/Method|Description|
|---|---|
|`Label`| This is the main name/label of the command you are creating.|
|`SuggestedArgs`| This is an array of args to let the user know what kind of data to put. Ex. int or string.|
|`Action(args)`| This is the executed code when the command has been activated. Commands are activated by running them in the console. Args should match suggested args correctly.|
