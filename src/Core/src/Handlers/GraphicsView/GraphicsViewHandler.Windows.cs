﻿using Microsoft.Maui.Graphics.Win2D;

namespace Microsoft.Maui.Handlers
{
	public partial class GraphicsViewHandler : ViewHandler<IGraphicsView, W2DGraphicsView>
	{
		protected override W2DGraphicsView CreatePlatformView()
		{
			return new W2DGraphicsView();
		}

		public static void MapDrawable(IGraphicsViewHandler handler, IGraphicsView graphicsView)
		{
			handler.PlatformView?.UpdateDrawable(graphicsView);
		}

		public static void MapFlowDirection(IGraphicsViewHandler handler, IGraphicsView graphicsView)
		{
			handler.PlatformView?.UpdateFlowDirection(graphicsView);
			handler.PlatformView?.Invalidate();
		}

		public static void MapInvalidate(IGraphicsViewHandler handler, IGraphicsView graphicsView, object? arg)
		{
			handler.PlatformView?.Invalidate();
		}
	}
}