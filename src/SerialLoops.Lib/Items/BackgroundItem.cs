﻿using HaruhiChokuretsuLib.Archive;
using HaruhiChokuretsuLib.Archive.Data;
using HaruhiChokuretsuLib.Archive.Graphics;
using SkiaSharp;
using System.Linq;

namespace SerialLoops.Lib.Items
{
    public class BackgroundItem : Item
    {
        public GraphicsFile Graphic1 { get; set; }
        public GraphicsFile Graphic2 { get; set; }
        public BgType BackgroundType { get; set; }

        public BackgroundItem(string name) : base(name, ItemType.Background)
        {
        }
        public BackgroundItem(string name, BgTableEntry entry, ArchiveFile<GraphicsFile> grp) : base(name, ItemType.Background)
        {
            BackgroundType = entry.Type;
            Graphic1 = grp.Files.First(g => g.Index == entry.BgIndex1);
            Graphic2 = grp.Files.FirstOrDefault(g => g.Index == entry.BgIndex2); // can be null if type is SINGLE_TEX
        }

        public SKBitmap GetBackground()
        {
            if (BackgroundType == BgType.SINGLE_TEX)
            {
                return Graphic1.GetImage();
            }
            else if (BackgroundType == BgType.TEX_BOTTOM_TILE_TOP)
            {
                SKBitmap bitmap = new(Graphic1.Width, Graphic1.Height + Graphic2.Height);
                SKCanvas canvas = new(bitmap);

                SKBitmap tileBitmap = new(Graphic2.Width, Graphic2.Height);
                SKBitmap tiles = Graphic2.GetImage(width: 64);
                SKCanvas tileCanvas = new(tileBitmap);
                int currentTile = 0;
                for (int y = 0; y < tileBitmap.Height; y += 64)
                {
                    for (int x = 0; x < tileBitmap.Width; x += 64)
                    {
                        SKRect crop = new(0, currentTile * 64, 64, (currentTile + 1) * 64);
                        SKRect dest = new(x, y, x + 64, y + 64);
                        tileCanvas.DrawBitmap(tiles, crop, dest);
                        currentTile++;
                    }
                }
                tileCanvas.Flush();

                canvas.DrawBitmap(tileBitmap, new SKPoint(0, 0));
                canvas.DrawBitmap(Graphic1.GetImage(), new SKPoint(0, Graphic2.Height));
                canvas.Flush();

                return bitmap;
            }
            else if (BackgroundType == BgType.KINETIC_SCREEN)
            {
                return Graphic2.GetScreenImage(Graphic1);
            }
            else
            {
                SKBitmap bitmap = new(Graphic1.Width, Graphic1.Height + Graphic2.Height);
                SKCanvas canvas = new(bitmap);

                canvas.DrawBitmap(Graphic1.GetImage(), new SKPoint(0, 0));
                canvas.DrawBitmap(Graphic2.GetImage(), new SKPoint(0, Graphic1.Height));
                canvas.Flush();

                return bitmap;
            }
        }
    }
}