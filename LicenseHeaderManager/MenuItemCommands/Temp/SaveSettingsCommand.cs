﻿/* Copyright (c) rubicon IT GmbH
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Core.Options;
using LicenseHeaderManager.Options;
using LicenseHeaderManager.Utils;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Task = System.Threading.Tasks.Task;

namespace LicenseHeaderManager.MenuItemCommands.Temp
{
  /// <summary>
  ///   Command handler
  /// </summary>
  internal sealed class SaveSettingsCommand
  {
    /// <summary>
    ///   Command ID.
    /// </summary>
    public const int CommandId = 4146;

    /// <summary>
    ///   Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid ("1a75d6da-3b30-4ec9-81ae-72b8b7eba1a0");

    /// <summary>
    ///   Initializes a new instance of the <see cref="SaveSettingsCommand" /> class.
    ///   Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private SaveSettingsCommand (AsyncPackage package, OleMenuCommandService commandService)
    {
      this.ServiceProvider = (LicenseHeadersPackage) package ?? throw new ArgumentNullException (nameof(package));
      commandService = commandService ?? throw new ArgumentNullException (nameof(commandService));

      var menuCommandID = new CommandID (CommandSet, CommandId);
      var menuItem = new MenuCommand (Execute, menuCommandID);
      commandService.AddCommand (menuItem);
    }

    /// <summary>
    ///   Gets the instance of the command.
    /// </summary>
    public static SaveSettingsCommand Instance { get; private set; }

    /// <summary>
    ///   Gets the service provider from the owner package.
    /// </summary>
    private LicenseHeadersPackage ServiceProvider { get; }

    /// <summary>
    ///   Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync (AsyncPackage package)
    {
      // Switch to the main thread - the call to AddCommand in SaveSettingsCommand's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (package.DisposalToken);

      var commandService = await package.GetServiceAsync (typeof (IMenuCommandService)) as OleMenuCommandService;
      Instance = new SaveSettingsCommand (package, commandService);
    }

    /// <summary>
    ///   This function is the callback used to execute the command when the menu item is clicked.
    ///   See the constructor to see how the menu item is associated with this function using
    ///   OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute (object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      var dlg = new SaveFileDialog
                {
                    Title = "Select path to save config file to...",
                    Filter = "LHM Config Files (*.json)|*.json"
                };

      if (dlg.ShowDialog() != true)
        return;

      ExecuteInternalAsync (dlg.FileName, dlg.FileName + "_vs").FireAndForget();
    }

    private async Task ExecuteInternalAsync (string coreFilePath, string visualStudioFilePath)
    {
      OptionsFacade.CurrentOptions.LinkedCommands = new List<LinkedCommand> (ServiceProvider.OptionsPage.LinkedCommands);
      await OptionsFacade.SaveAsync (OptionsFacade.CurrentOptions, coreFilePath, visualStudioFilePath);
    }
  }
}