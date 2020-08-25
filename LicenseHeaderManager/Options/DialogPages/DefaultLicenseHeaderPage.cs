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
using System.Reflection;
using LicenseHeaderManager.Options.DialogPageControls;
using System.Windows.Forms;
using Core.Options;
using LicenseHeaderManager.Options.Model;
using log4net;

namespace LicenseHeaderManager.Options.DialogPages
{
  public class DefaultLicenseHeaderPage : BaseOptionPage<DefaultLicenseHeaderPageModel>
  {
    private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public DefaultLicenseHeaderPage()
    {
    }

    public override void ResetSettings()
    {
      ((IDefaultLicenseHeaderPageModel)Model).Reset();
    }

    protected override IWin32Window Window => new WpfHost(new WpfDefaultLicenseHeader((IDefaultLicenseHeaderPageModel)Model));

    protected override IEnumerable<UpdateStep> GetVersionUpdateSteps()
    {
      //yield return new UpdateStep (new Version (3, 0, 1), MigrateStorageLocation_3_0_1);
      yield return new UpdateStep(new Version(3, 1, 0), MigrateStorageLocation_3_1_0);
    }

    private void MigrateStorageLocation_3_0_1()
    {
      s_log.Info("Start migration to License Header Manager Version 3.0.1");
      s_log.Info($"Current version: {Version}");

      if (!System.Version.TryParse(Version, out var version) || version < new Version(3, 0, 0))
      {
        LoadRegistryValuesBefore_3_0_0();
      }
      else
      {
        s_log.Info("Migration to 3.0.1 with existing default license header text page");
        var migratedDefaultLicenseHeaderTextPage = new DefaultLicenseHeaderPageModel();
        LoadRegistryValuesBefore_3_0_0(migratedDefaultLicenseHeaderTextPage);

        /*OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText = ThreeWaySelectionForMigration(
            OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText,
            migratedDefaultLicenseHeaderTextPage.DefaultLicenseHeaderFileText,
            CoreOptions._defaultDefaultLicenseHeaderFileText);*/

        OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText = migratedDefaultLicenseHeaderTextPage.DefaultLicenseHeaderFileText;
      }
    }

    private void MigrateStorageLocation_3_1_0()
    {
      s_log.Info("Start migration to License Header Manager Version 3.1.0");
      if (!System.Version.TryParse(Version, out var version) || version < new Version(3, 1, 0))
      {
        var logVersion = Version;
        if (Version == null)
        {
          logVersion = "null";
        }
        s_log.Info($"Current version: {logVersion}");
        LoadCurrentRegistryValues_3_0_3();
      }
      else
      {
        s_log.Info("Migration to 3.0.1 with existing default license header text page");
        var migratedDefaultLicenseHeaderTextPage = new DefaultLicenseHeaderPageModel();
        LoadCurrentRegistryValues_3_0_3(migratedDefaultLicenseHeaderTextPage);

        /*OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText = ThreeWaySelectionForMigration(
            OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText,
            migratedDefaultLicenseHeaderTextPage.DefaultLicenseHeaderFileText,
            CoreOptions._defaultDefaultLicenseHeaderFileText);*/

        OptionsFacade.CurrentOptions.DefaultLicenseHeaderFileText = migratedDefaultLicenseHeaderTextPage.DefaultLicenseHeaderFileText;
      }
    }
  }
}
