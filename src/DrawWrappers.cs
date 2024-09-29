﻿﻿using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace MMXOnline;

public class DrawableWrapper {
	public List<ShaderWrapper>? shaders;
	public Drawable drawable;
	public Color color;
	public int[]? size;

	public DrawableWrapper(
		List<ShaderWrapper> shaders, Drawable drawable,
		Color? color = null, int[]? size = null
	) {
		shaders?.RemoveAll(s => s == null);
		this.shaders = shaders;
		this.drawable = drawable;
		this.color = color ?? Color.White;
		this.size = size;
	}
}

public class DrawLayer : Transformable, Drawable {
	public List<DrawableWrapper> oneOffs = new List<DrawableWrapper>();

	public DrawLayer() {

	}

	public void Draw(RenderTarget target, RenderStates states) {
		for (int i = 0; i < oneOffs.Count; i++) {
			var oneOff = oneOffs[i];
			Global.window.SetView(Global.view);
			// No shaders.
			if (oneOff.shaders == null || oneOff.shaders.Count == 0) {
				target.Draw(oneOff.drawable);
			}
			// One shader.
			else if (oneOff.shaders.Count == 1 && oneOff.color == Color.White) {
				RenderStates renderStates = new RenderStates(states);
				renderStates.Shader = oneOff.shaders[0].getShader();
				target.Draw(oneOff.drawable, renderStates);
			}
			// Multi-shader.
			else {
				var sprite = oneOff.drawable as SFML.Graphics.Sprite;
				if (sprite == null) {
					continue;
				}
				RenderStates renderStates = new RenderStates(states);
				Vector2f originalPosition = sprite.Position;
				Vector2f originalOrigin = sprite.Origin;
				float originalRotation = sprite.Rotation;
				Vector2f originalScale = sprite.Scale;
				Color originalColor = sprite.Color;
				sprite.Position = new Vector2f(0, 0);
				sprite.Origin = new Vector2f(0, 0);
				sprite.Scale = new Vector2f(1, 1);
				sprite.Rotation = 0;
				sprite.Color = Color.White;
				// Get textures.
				int encodeKey = (oneOff.size[0] * 397) ^ oneOff.size[1];
				// If something goes off.
				if (!Global.renderTextures.ContainsKey(encodeKey)) {
					Global.renderTextures[encodeKey] = (
						new RenderTexture((uint)oneOff.size[0], (uint)oneOff.size[1]),
						new RenderTexture((uint)oneOff.size[0], (uint)oneOff.size[1])
					);
				}
				RenderTexture front;
				RenderTexture back;
				(front, back) = Global.renderTextures[encodeKey];
				// Create a clear texture first.
				back.Clear(new Color(0, 0, 0, 0));
				back.Display();
				renderStates.Shader = oneOff.shaders[0].getShader();
				back.Draw(sprite, renderStates);
				// Iterate shaders.
				for (int num = 1; num < oneOff.shaders.Count; num++) {
					// Clear image.
					renderStates = new RenderStates(states);
					front.Clear(new Color(0, 0, 0, 0));
					front.Display();
					// Apply shader and draw.
					renderStates.Shader = oneOff.shaders[num].getShader();
					front.Draw(
						new SFML.Graphics.Sprite(back.Texture), renderStates
					);
					// Swap.
					(
						front,
						back
					) = (
						back,
						front
					);
				}
				// Final result.
				var finalSprite = new SFML.Graphics.Sprite(back.Texture);
				finalSprite.Position = originalPosition;
				finalSprite.Origin = originalOrigin;
				finalSprite.Scale = originalScale;
				finalSprite.Rotation = originalRotation;
				finalSprite.Color = originalColor;

				renderStates = new RenderStates(states);
				target.Draw(finalSprite, renderStates);
			}
		}
	}
}

public partial class DrawWrappers {
	public static Dictionary<long, DrawLayer> walDrawObjects = new Dictionary<long, DrawLayer>();

	private static DrawLayer getDrawLayer(long depth) {
		DrawLayer drawLayer;
		if (!walDrawObjects.ContainsKey(depth)) {
			walDrawObjects[depth] = new DrawLayer();
		}
		drawLayer = walDrawObjects[depth];
		return drawLayer;
	}

	public static void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness, long depth, bool isWorldPos = true) {
		if (isWorldPos && Options.main.enablePostProcessing) {
			x1 -= Global.level.camX;
			x2 -= Global.level.camX;
			y1 -= Global.level.camY;
			y2 -= Global.level.camY;
		}

		LineShape line = new LineShape(new Vector2f(x1, y1), new Vector2f(x2, y2), color, thickness);

		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(null, line));
		} else {
			drawToHUD(line);
		}
	}

	public static void DrawDebugLine(float x1, float y1, float x2, float y2, Color? color = null, float thickness = 2) {
		DrawLine(x1, y1, x2, y2, color ?? Color.Red, thickness, ZIndex.HUD, true);
	}

	public static void DrawDebugDot(float x, float y, Color? color = null, float radius = 2) {
		DrawCircle(x, y, radius, true, color ?? Color.Red, 1, ZIndex.HUD, true);
	}

	public static void DrawCircle(float x, float y, float radius, bool filled, Color color, float thickness, long depth, bool isWorldPos = true, Color? outlineColor = null, uint? pointCount = null) {
		if (isWorldPos && Options.main.enablePostProcessing) {
			x -= Global.level.camX;
			y -= Global.level.camY;
		}

		CircleShape circle = new CircleShape(radius);
		circle.Position = new Vector2f(x, y);
		circle.Origin = new Vector2f(radius, radius);
		if (filled) {
			circle.FillColor = color;
			if (outlineColor != null) {
				circle.OutlineColor = (Color)outlineColor;
				circle.OutlineThickness = thickness;
			}
		} else {
			circle.FillColor = Color.Transparent;
			circle.OutlineColor = color;
			circle.OutlineThickness = thickness;
		}
		if (pointCount != null) {
			circle.SetPointCount(pointCount.Value);
		}

		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(null, circle));
		} else {
			drawToHUD(circle);
		}
	}

	public static RenderTexture pixel = new RenderTexture(1, 1);
	public static void DrawPixel(float x, float y, Color color, long depth, bool isWorldPos = true) {
		if (isWorldPos && Options.main.enablePostProcessing) {
			x -= Global.level.camX;
			y -= Global.level.camY;
		}

		pixel.Clear(color);
		pixel.Display();
		var pixelSprite = new SFML.Graphics.Sprite(pixel.Texture);
		pixelSprite.Position = new Vector2f(x, y);

		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(null, pixelSprite));
		} else {
			drawToHUD(pixelSprite);
		}
	}

	public static void DrawRect(float x1, float y1, float x2, float y2, bool filled, Color color, float thickness, long depth, bool isWorldPos = true, Color? outlineColor = null) {
		if (isWorldPos && Options.main.enablePostProcessing) {
			x1 -= Global.level.camX;
			x2 -= Global.level.camX;
			y1 -= Global.level.camY;
			y2 -= Global.level.camY;
		}

		RectangleShape rect = new RectangleShape(new Vector2f(x2 - x1, y2 - y1));
		rect.Position = new Vector2f(x1, y1);
		if (filled) {
			rect.FillColor = color;
			if (outlineColor != null) {
				rect.OutlineColor = (Color)outlineColor;
				rect.OutlineThickness = thickness;
			}
		} else {
			rect.FillColor = Color.Transparent;
			rect.OutlineColor = color;
			rect.OutlineThickness = thickness;
		}

		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(null, rect));
		} else {
			drawToHUD(rect);
		}
	}

	public static void DrawRectWH(float x1, float y1, float w, float h, bool filled, Color color, int thickness, long depth, bool isWorldPos = true, Color? outlineColor = null) {
		DrawRect(x1, y1, x1 + w, y1 + h, filled, color, thickness, depth, isWorldPos, outlineColor);
	}

	public static void DrawPolygon(
		List<Point> points, Color color, bool fill, long depth,
		bool isWorldPos = true, Color? outlineColor = null, int thickness = 1
	) {
		if (isWorldPos && Options.main.enablePostProcessing) {
			for (int i = 0; i < points.Count; i++) {
				points[i] = new Point(points[i].x - Global.level.camX, points[i].y - Global.level.camY);
			}
		}
		ConvexShape shape = new ConvexShape((uint)points.Count);
		for (int i = 0; i < points.Count; i++) {
			shape.SetPoint((uint)i, new Vector2f(points[i].x, points[i].y));
		}
		if (fill || outlineColor != null) {
			shape.FillColor = color;
		} else {
			shape.OutlineColor = color;
			shape.OutlineThickness = thickness;
			shape.FillColor = Color.Transparent;
		}
		if (outlineColor != null) {
			shape.OutlineColor = outlineColor.Value;
			shape.OutlineThickness = thickness;
		}
		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(null, shape));
		} else {
			drawToHUD(shape);
		}
	}

	public static void DrawTexture(
		Texture texture, float sx, float sy, float sw, float sh,
		float dx, float dy, long depth,
		float cx = 0, float cy = 0,
		float xScale = 1, float yScale = 1,
		float angle = 0, float alpha = 1,
		List<ShaderWrapper> shaders = null, bool isWorldPos = true
	) {
		if (texture == null) return;

		if (isWorldPos && Options.main.enablePostProcessing) {
			dx -= Global.level.camX;
			dy -= Global.level.camY;
		}
		dx = MathF.Round(dx);
		dy = MathF.Round(dy);
		cx = MathF.Floor(cx);
		cy = MathF.Floor(cy);

		var sprite = new SFML.Graphics.Sprite(texture, new IntRect((int)sx, (int)sy, (int)sw, (int)sh));
		sprite.Position = new Vector2f(dx, dy);
		sprite.Origin = new Vector2f(cx, cy);
		sprite.Scale = new Vector2f(xScale, yScale);
		sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, (byte)(int)(alpha * 255));
		sprite.Rotation = angle;

		if (isWorldPos) {
			DrawLayer drawLayer = getDrawLayer(depth);
			drawLayer.oneOffs.Add(new DrawableWrapper(shaders, sprite, sprite.Color, [(int)sw, (int)sh]));
		} else {
			drawToHUD(sprite);
		}
	}

	public static void DrawMapTiles(Texture[,] textureMDA, float x, float y, RenderTexture screenRenderTexture = null, ShaderWrapper shaderWrapper = null) {
		float origX = x;
		float origY = y;
		if (textureMDA == null) return;
		if (screenRenderTexture == null) {
			Global.window.SetView(Global.view);
		} else {
			x = x - Global.level.camX;
			y = y - Global.level.camY;

			// High speed scrolling may look better with this code. Might cause issues elsewhere so limiting to race
			if (Global.level?.gameMode is Race) {
				x = MathInt.Round(x);
				y = MathInt.Round(y);
			}
		}

		for (int i = 0; i < textureMDA.GetLength(0); i++) {
			for (int j = 0; j < textureMDA.GetLength(1); j++) {
				Texture texture = textureMDA[i, j];
				if (texture != null) {
					var sprite = new SFML.Graphics.Sprite(texture);
					int xOffset = 0;
					int yOffset = 0;
					int tileW = (int)texture.Size.X;
					int tileH = (int)texture.Size.Y;
					if (j > 0) xOffset = j * (int)textureMDA[i, j - 1].Size.X;
					if (i > 0) yOffset = i * (int)textureMDA[i - 1, j].Size.Y;

					// Don't draw tiles out of the screen for optimization
					var rect = Rect.createFromWH(origX + xOffset, origY + yOffset, tileW, tileH);
					var camRect = new Rect(Global.level.camX, Global.level.camY, Global.level.camX + Global.viewScreenW, Global.level.camY + Global.viewScreenH);
					if (!rect.overlaps(camRect)) {
						continue;
					}

					RenderStates? renderStates = null;
					Shader shader = shaderWrapper?.getShader();
					if (shader != null) {
						renderStates = new RenderStates(shader);
					}

					sprite.Position = new Vector2f(x + xOffset, y + yOffset);
					if (screenRenderTexture == null) {
						if (renderStates == null) Global.window.Draw(sprite);
						else Global.window.Draw(sprite, renderStates.Value);
					} else {
						if (renderStates == null) screenRenderTexture.Draw(sprite);
						else screenRenderTexture.Draw(sprite, renderStates.Value);
					}
				}
			}
		}
	}
}
