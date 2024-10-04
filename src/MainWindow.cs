﻿using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MMXOnline;

public partial class Global {
	public static RenderWindow window = null!;
	public static bool fullscreen;

	public static Dictionary<int, (RenderTexture, RenderTexture)> renderTextures = new();
	public static RenderTexture screenRenderTexture = null!;
	public static RenderTexture srtBuffer1 = null!;
	public static RenderTexture srtBuffer2 = null!;
	public static RenderTexture radarRenderTexture = null!;

	// Normal (small) camera
	public static RenderTexture screenRenderTextureS = null!;
	public static RenderTexture srtBuffer1S = null!;
	public static RenderTexture srtBuffer2S = null!;

	// Large camera
	public static RenderTexture screenRenderTextureL = null!;
	public static RenderTexture srtBuffer1L = null!;
	public static RenderTexture srtBuffer2L = null!;

	public static View view = null!;
	public static View backgroundView = null!;

    public static uint screenW = 420; //428
    public static uint screenH = 240;

    // OG size
	//public static uint screenW = 384;
	//public static uint screenH = 216;

	public static uint viewScreenW { get { return screenW * (uint)viewSize; } }
	public static uint viewScreenH { get { return screenH * (uint)viewSize; } }

	public static uint halfViewScreenW { get { return viewScreenW / 2; } }
	public static uint halfViewScreenH { get { return viewScreenH / 2; } }

	public static uint halfScreenW = screenW / 2;
	public static uint halfScreenH = screenH / 2;

	public static uint windowW;
	public static uint windowH;

	public static int viewSize = 1;

	public static void changeWindowSize(uint windowScale) {
		windowW = screenW * windowScale;
		windowH = screenH * windowScale;
		if (window != null) {
			window.Size = new Vector2u(windowW, windowH);
		}
	}

	public static void initMainWindow(Options options) {
		fullscreen = options.fullScreen;

		changeWindowSize(options.windowScale);

		screenRenderTextureS = new RenderTexture(screenW, screenH);
		srtBuffer1S = new RenderTexture(screenW, screenH);
		srtBuffer2S = new RenderTexture(screenW, screenH);

		screenRenderTextureL = new RenderTexture(screenW * 2, screenH * 2);
		srtBuffer1L = new RenderTexture(screenW * 2, screenH * 2);
		srtBuffer2L = new RenderTexture(screenW * 2, screenH * 2);

		var viewPort = new FloatRect(0, 0, 1, 1);

		if (!fullscreen) {
			window = new RenderWindow(new VideoMode(windowW, windowH), "MMX Online Deathmatch");
			window.SetVerticalSyncEnabled(options.vsync);
			if (Global.hideMouse) window.SetMouseCursorVisible(false);
		} else {
			var desktopWidth = VideoMode.DesktopMode.Width;
			var desktopHeight = VideoMode.DesktopMode.Height;
			window = new RenderWindow(new VideoMode(desktopWidth, desktopHeight), "MMX Online Deathmatch", Styles.Fullscreen);
			window.SetMouseCursorVisible(false);
			viewPort = getFullScreenViewPort();
		}

		if (!File.Exists(Global.assetPath + "assets/menu/icon.png")) {
			throw new Exception("Error loading icon asset file, posible missing assets.");
		}

		var image = new Image(Global.assetPath + "assets/menu/icon.png");
		window.SetIcon(image.Size.X, image.Size.Y, image.Pixels);

		view = new View(new Vector2f(0, 0), new Vector2f(screenW, screenH));
		view.Viewport = viewPort;

		DrawWrappers.initHUD();
		DrawWrappers.hudView.Viewport = viewPort;

		window.SetView(view);
		if (Global.overrideFPS != null) {
			window.SetFramerateLimit((uint)Global.overrideFPS);
		} else {
			window.SetFramerateLimit((uint)options.maxFPS);
		}

		window.SetActive();
	}

	public static FloatRect getFullScreenViewPort() {
		float desktopWidth = VideoMode.DesktopMode.Width;
		float desktopHeight = VideoMode.DesktopMode.Height;
		float heightMultiple = VideoMode.DesktopMode.Height / (float)screenH;

		if (Options.main.integerFullscreen) {
			heightMultiple = MathF.Floor(VideoMode.DesktopMode.Height / (float)screenH);
		}
		float extraWidthPercent = (desktopWidth - screenW * heightMultiple) / desktopWidth;
		float extraHeightPercent = (desktopHeight - screenH * heightMultiple) / desktopHeight;

		return new FloatRect(extraWidthPercent / 2f, extraHeightPercent / 2f, 1f - extraWidthPercent, 1f - extraHeightPercent);
	}
}
