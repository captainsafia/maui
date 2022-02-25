using System;
using Microsoft.UI.Xaml.Controls;
using WThickness = Microsoft.UI.Xaml.Thickness;

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : ElementHandler<IWindow, UI.Xaml.Window>
	{
		RootPanel? _rootPanel = null;

		protected override void ConnectHandler(UI.Xaml.Window platformView)
		{
			base.ConnectHandler(platformView);

			if (_rootPanel == null)
			{
				// TODO WINUI should this be some other known constant or via some mechanism? Or done differently?
				MauiWinUIApplication.Current.Resources.TryGetValue("MauiRootContainerStyle", out object? style);

				_rootPanel = new RootPanel
				{
					Style = style as UI.Xaml.Style
				};
			}

			platformView.Content = _rootPanel;

			var WindowHandle = PlatformView.GetWindowHandle();

			// Retrieve current extended style
			var extended_style = PlatformMethods.GetWindowLongPtr(WindowHandle, PlatformMethods.WindowLongFlags.GWL_EXSTYLE);

			// https://docs.microsoft.com/en-us/windows/win32/directwrite/how-to-ensure-text-is-displayed-with-the-correct-reading-direction
			var layoutRTL = extended_style & (long)PlatformMethods.ExtendedWindowStyles.WS_EX_LAYOUTRTL;
			var readingRTL = extended_style & (long)PlatformMethods.ExtendedWindowStyles.WS_EX_RTLREADING;

			if (layoutRTL == (long)PlatformMethods.ExtendedWindowStyles.WS_EX_LAYOUTRTL ||
				readingRTL == (long)PlatformMethods.ExtendedWindowStyles.WS_EX_RTLREADING)
			{
				VirtualView.SetDeviceFlowDirection(FlowDirection.RightToLeft);
			}
			else
			{
				VirtualView.SetDeviceFlowDirection(FlowDirection.LeftToRight);
			}
		}

		protected override void DisconnectHandler(UI.Xaml.Window platformView)
		{
			MauiContext
				?.GetNavigationRootManager()
				?.Disconnect();

			_rootPanel?.Children?.Clear();
			platformView.Content = null;

			base.DisconnectHandler(platformView);
		}

		public static void MapTitle(IWindowHandler handler, IWindow window) =>
			handler.PlatformView?.UpdateTitle(window);

		public static void MapContent(IWindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			var windowManager = handler.MauiContext.GetNavigationRootManager();
			windowManager.Connect(handler.VirtualView.Content);
			var rootPanel = handler.PlatformView.Content as Panel;

			if (rootPanel == null)
				return;

			rootPanel.Children.Clear();
			rootPanel.Children.Add(windowManager.RootView);

			if (window.VisualDiagnosticsOverlay != null)
				window.VisualDiagnosticsOverlay.Initialize();
		}

		public static void MapToolbar(IWindowHandler handler, IWindow view)
		{
			if (view is IToolbarElement tb)
				ViewHandler.MapToolbar(handler, tb);
		}

		public static void MapMenuBar(IWindowHandler handler, IWindow view)
		{
			if (view is IMenuBarElement mb)
			{
				_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
				var windowManager = handler.MauiContext.GetNavigationRootManager();
				windowManager.SetMenuBar(mb.MenuBar);
			}
		}

		public static void MapFlowDirection(IWindowHandler handler, IWindow view)
		{
			if (view.FlowDirection == FlowDirection.MatchParent)
				return;

			var WindowHandle = handler.PlatformView.GetWindowHandle();

			// Retrieve current extended style
			var extended_style = PlatformMethods.GetWindowLongPtr(WindowHandle, PlatformMethods.WindowLongFlags.GWL_EXSTYLE);

			if (view.FlowDirection == FlowDirection.RightToLeft)
				extended_style = extended_style | (long)PlatformMethods.ExtendedWindowStyles.WS_EX_LAYOUTRTL;
			else
				extended_style = extended_style & ~((long)PlatformMethods.ExtendedWindowStyles.WS_EX_LAYOUTRTL);

			PlatformMethods.SetWindowLongPtr(WindowHandle, PlatformMethods.WindowLongFlags.GWL_EXSTYLE, extended_style);
		}
	}
}