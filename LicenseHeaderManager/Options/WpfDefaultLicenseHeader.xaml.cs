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
using System.Windows;
using System.Windows.Controls;

namespace LicenseHeaderManager.Options
{
  /// <summary>
  ///   Interaction logic for WpfDefaultLicenseHeader.xaml
  /// </summary>
  public partial class WpfDefaultLicenseHeader : UserControl
  {
    private readonly IDefaultLicenseHeaderPage _page;

    public WpfDefaultLicenseHeader (IDefaultLicenseHeaderPage page)
        : this()
    {
      _page = page;
      DataContext = _page;
    }

    public WpfDefaultLicenseHeader ()
    {
      InitializeComponent();
    }

    private void EditButton_Click (object sender, RoutedEventArgs e)
    {
      var dialog = new WpfEditDefaultLicenseHeaderDialog (_page);
      dialog.ShowDialog();
    }
  }
}