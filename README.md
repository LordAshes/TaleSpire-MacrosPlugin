# Macros Plugin (aka LASS with a LAMB)

This unofficial TaleSpire plugin allows users the Lord Ashes Scripting System (LASS) to create macros which
can be launched using a side out menu. Includes a Lord Ashes Macros Builder (LAMB) which will convert the
user macros scripts into the Macros plugin. The user can then access the macros using a slide out menu.
The slide out menu can be made content sensitive so its contents can change based on conditions provided
in the macros (such as selected mini, player, etc).

## Change Log

1.0.0: Initial release

## Requirements

You will need Visual Studio to create macros but Visual Studio is not needed to use the macros. 
Any edition of Visual Studio should work including the free Community edition. You will need the following
plugin to build and/or use the created macros:

Hollo's Radial UI plugin installed.

Lord Ashes' Stat Messaging plugin installed.

Lord Ashes' File Access plugin installed.

## Install

Use R2ModMan or similar installer to install this plugin. This will install a sample macro plugin but more
importantly it will install the Lord Ashes Macros Builder (LAMB) so that you can build your own Macros plugin.
Warning: Some manual configuration is required to use LAMB. See the first time use section below. 

## First Time Use

Before you can use LAMB there is a one time manual configuration needed. Once you configure it, there should
not be any need to configure it again unless you update the plugin.

LAMB can be found in a sub-folder of the Macros Plugin folder.

1. Locate the MacrosBuilder.config file and edit it.
2. Replace the first line with the drive, path and file name of Visual Studio's MSBuilder file.
   (The default entry is for a standard install of Visual Studio 2019 Community edition)
3. Replace the second line with the location of the Macros Plugin folder
4. Ensure that there are no blank lines before or between the two entries. They must be the 1st and 2nd lines.
5. In the MacrosPlugin sub-folder, locate the MacrosPlugin.csproj file and edit it.
6. Correct the full path of all references in this file so that they point to the correct location for the
   respective assembly. The Microsoft assembly references can be left as is (relative path) but all the
   TaleSpire references and the custom plugin references need to be updated to locations on the user's device. 

## Creating Macros

Each macro script is a seperate file placed in the LAMB macros folder. The file is a text file with the extension
of "macroScript". The contents of the file has two labels and two section. All four of these must be present in
order for the macro to work:

### Macro Label

Each macro requires a line starting with "Macro:" followed by the name of the macro. The name is not actually used
in the code itself (but is still required). It is used as a comment header in the source code to make editing errors
more easier. For example:

Macro: Attack 1

### Button Label

Each macro requires a line starting with "Button:" followed by the text that will appear on the macro button when
the macro button is visible.

### Check Section

Each macro requires a line that contains "Check:". No other text follows on the same line. This section header
indicates that the lines of code that follow are for the available check. The availability check defines the
conditions under which the button will be displayed. It expects the code to return either a true or false. If the
code returns true, the macro button will be displayed when the macro menu is pulled out. If the code returns false
the menu button will not be displayed when the macros menu is pulled out. The condition is continioually checked
while the menu is pulled out so that it can be used to make context sensitive macro options.

If you want a macro button to always be visible when the macro menu is pulled out, simply return true.

Warning: The check code needs to be a quick evaluation of conditions. Do not include any code that will block
         execution (such as prompts for user input) or operations which may take a long time to execute such
		 as disk access. Doing so can lead to memory or stack overflow errors.

### Execute Section   

Each macro requires a line that contains "Execute:". No other text follows on the same line. This section header
indicates that the lines of code that follow are for the execution script. This is the script that is executed
when the button is clicked.

### Scripting Langauge

The check and execute section use C# coding syntax. The scripts can make use of any core C# functionality such
as loops and conditional statements. In addition, the scripts have access to most of the core TaleSpire objects.
To make some of the TaleSpire operations easiers, the user has access to a bunch of Helper functions which are
simple wrappers around TaleSpire functionality and thus make calling the TaleSpire functions easier. The Helper
functions can be expanded to include additional Helper functions (see below).

LAMB comes with 3 sample macros to show the correct macro file syntax.

## Building The Macros Plugin

Now that you have your macros, we need to build the corresponding Macros Plugin in order to be able to use the
macros in TaleSpire. This is what the Lord Ashes Macro Builder (LAMB) is for. Open up a Command Prompt and change
to the Macros Builder folder. Then simpley type:

BuildTaleSpireMacros

and press ENTER. If your macros are written correctly you will see the screen fill with text but near the end of
the text there should be a message that says:

```Build succeeded.
    0 Warning(s)
    0 Error(s)
 ```
	
If that is the case the MacrosPlugin is built, copied to your plugin directory, and read for use in TaleSpire.
If the results show some errors then you need to correct the macro code before the MacrosPlugin will be built.
If the results shows warning but not errors then the MacrosPlugin will be built but may not function as expected.

When errors occur the text before the summary may help determine what the problem is and you can go back to your
macros files to correct it. However, there is a second way that you can determine the error which can, in most
cases, be easier. See the next section.

### Fixing Errors

When LAMB's BuildTaleSpireMacros is used, it collect the macros content and inserts it into a plugin template.
This plugin template is a single file of C# code. When the build process has errors, you can open the generated
MacrosPlugin project in VisualStudio and get not only the location of errors when you try to build it but also
access to the intellisense to help write/fix your code.

Warning: The C# code file that is created is overwritten each time you use LAMB's BuildTaleSpireMacros. Thus any
         corrections you make in this file should be copied back to the macros, so that when BuildTaleSpireMacros
		 is used, it does not overwrite all your changes. Alternatively you can just build the MacroPlugin from
		 within VisualStudio but then your macro files will be out of date.
		 
## How It Works And Advanced Features

In a nut shell the user is building a single TaleSpire plugin which contains both the code for the macros sideout
menu and the the code for the macros. LAMB comes with a template file for a TaleSpire plugin which it uses to make
the actual plugin source code. The template file has a place holders into which LAMB inserts the user code which
results in a complete C# source code file. LAMB also comes with a VS project for compiling the plugin. When LAMB
creates the plugin complete C# source code, it overwrites the corresponding file in the VS project. Lastly LAMB
triggers the VS compiler to compile the results and copy the resulting MacrosPlugin file into the indicated folder
where TS can find it.

As mentioned above this means that the plugin source code can be found in MacrosPlugin/MacrosPlugin.cs. You will
find all of the macros merged into this file and seperated by comments indicating the macro name. While the plugin
can be built from within VisualStudio from this file, it is recommended that if you use this file to correct errors
within Visual Studio, you propagate the changes back to your macro files. This will ensure that the macro files are
up to date in case you use them in the future.

### Changing The Macro Menus Keyboard Shortcut

Most Lord Ashes plugin have a R2ModMan setting for changing the keyboard key that triggers them. Currently LAMB
does not but the key combination can be changed. Locate the maco_template.pcs file. The pcs stands for partial
c# script. This is the template into which LAMB inserts all the user's macros code. Be very careful when editing
this file because any errors will prevent LAMB from being able to compile your macros. On line 39 you will find
the check for toggling the macro menu. Change "KeyCode.Backslash" to any other VisualStudio valid KeyCode. You will
have to re-build your macros after this change but now the MacrosPlugin will use your specified keyboard shorcut.

### Adding/Modifying/Removing Helpers

The end of the maco_template.pcs file has a section of Helpers. These are ordinary C# methods which simplify various
common functionality. For example the SelectedAsset() helper takes the currently selected mini guid and obtains the
related asset since users will tend to create scripts which need to work with the asset or Creature. The methods
SelectedCreature() performs a similar function but return the Creature instead. This makes it easy for scripts
to user the asset or creature without having to write a lot of code. A Random() helper has also been provided for
easy access to a random number generator.

This section of the template can be modified to add, modify or remove Helper functions. Just make sure that you does
not introduce errors in the template since that will prevent any macros from being built. 

## Limitation

1. Currently macros can only be built ahead of time and not at runtime.
2. There is only one Macros Plugin so all desired macros need to be added. However, the checks for macros can be used
   to control when the macros are and are not available.
3. Macros themselves are not networked but the results of the macro script might be. For example, when a macro that
   changes HP is executed on a device, the macro is only executed on that one specific device but the act of changing
   HP is then networked by core TS to other users.
