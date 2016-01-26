﻿#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Terminal.TerminalPublic
File: App.xaml.cs
Created: 2015, 11, 11, 3:22 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License

using Ecng.Configuration;
using System.Windows;
using System.Windows.Threading;
using StockSharp.Studio.Core.Commands;
using StockSharp.Studio.Core.Services;

namespace StockSharp.Terminal
{
	public partial class App
	{
		private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(MainWindow, e.Exception.ToString());
			e.Handled = true;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
            ConfigManager.RegisterService<IStudioCommandService>(new StudioCommandService());
            //ConfigManager.RegisterService<ISecurityProvider>(new FakeSecurityProvider());
            //ConfigManager.RegisterService<IMarketDataProvider>(new FakeMarketDataProvider());

            base.OnStartup(e);
		}
	}
}