/*Copyright (c) 2012 Cesar Ramirez
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
 * documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Reflection;
using Img = System.Drawing.Imaging;
using TexLib;

using MechTactics.GameElements;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using MechTactics.Interfaces;
using MechTactics.Abstracts;

namespace MechTactics
{
        public class Display
        {

            public int workerId;
            public Bitmap worker;            
            public int infantryId;
            public Bitmap infantry;
            public int heavyId;
            public Bitmap heavy;
            public int sniperId;
            public Bitmap sniper;
            public int tankId;
            public Bitmap tank;
            public int HtankId;
            public Bitmap Htank;
            public int artilleryId;
            public Bitmap artillery;
            public int heroId;
            public Bitmap hero;
            public int central_baseId;
            public Bitmap central_base;
            public int barrackId;
            public Bitmap barrack;
            public int factoryId;
            public Bitmap factory;
            public int mineId;
            public Bitmap mine;
            public int portalId;
            public Bitmap portal;
            
            private bool loaded = false;
            private bool ready = false;

            private GLControl glControl;
            private ISimulator sim;

            private double maxHeight;
            private double mapSize;
            private double tileWidth;
            private double tileHeight;
            private List<BaseGameObject> objects;
            private int[] action;

            private double rationW;
            private double rationH;

            public Display(GLControl glControl)
            {
                this.glControl = glControl;
                string dir = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "GameElements", "res");
                worker = new Bitmap(System.IO.Path.Combine(dir, "worker.png"));
                infantry = new Bitmap(System.IO.Path.Combine(dir, "infantry.png"));
                heavy = new Bitmap(System.IO.Path.Combine(dir, "heavy.png"));
                sniper = new Bitmap(System.IO.Path.Combine(dir, "sniper.png"));
                tank = new Bitmap(System.IO.Path.Combine(dir, "Ltank.png"));
                Htank = new Bitmap(System.IO.Path.Combine(dir, "Htank.png"));
                artillery = new Bitmap(System.IO.Path.Combine(dir, "artillery.png"));
                hero = new Bitmap(System.IO.Path.Combine(dir, "hero.png"));
                central_base = new Bitmap(System.IO.Path.Combine(dir, "base.png"));
                barrack = new Bitmap(System.IO.Path.Combine(dir, "barrack.png"));
                mine = new Bitmap(System.IO.Path.Combine(dir, "mine.png"));
                factory = new Bitmap(System.IO.Path.Combine(dir, "factory.png"));
                portal = new Bitmap(System.IO.Path.Combine(dir, "portal.png"));
            }
            
            public void SetupViewport()
            {
                int w = glControl.Width;
                int h = glControl.Height;
                if (ready)
                {
                    tileHeight = h / mapSize;
                    tileWidth = w / mapSize;
                }
                rationW = (tileWidth / 20);
                rationH = (tileHeight / 20);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
                GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            }

            public void setSimulator(ISimulator sim)
            {
                this.sim = sim;
                /*this.maxHeight = sim.maxHeight;
                this.mapSize = sim.mapSize;
                this.objects = sim.map.elements;
                this.action = sim.action;*/
                ready = true;
                SetupViewport();
            }

            public void load(object sender, EventArgs e)
            {
                loaded = true;
                TexUtil.InitTexturing();

                //Worker
                GL.GenTextures(1, out workerId);
                GL.BindTexture(TextureTarget.Texture2D, workerId);

                BitmapData data = worker.LockBits(new System.Drawing.Rectangle(0, 0, worker.Width, worker.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                worker.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Infanrty
                GL.GenTextures(1, out infantryId);
                GL.BindTexture(TextureTarget.Texture2D, infantryId);

                data = infantry.LockBits(new System.Drawing.Rectangle(0, 0, infantry.Width, infantry.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                infantry.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Heavy
                GL.GenTextures(1, out heavyId);
                GL.BindTexture(TextureTarget.Texture2D, heavyId);

                data = heavy.LockBits(new System.Drawing.Rectangle(0, 0, heavy.Width, heavy.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                heavy.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Sniper
                GL.GenTextures(1, out sniperId);
                GL.BindTexture(TextureTarget.Texture2D, sniperId);

                data = sniper.LockBits(new System.Drawing.Rectangle(0, 0, sniper.Width, sniper.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                sniper.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Tank
                GL.GenTextures(1, out tankId);
                GL.BindTexture(TextureTarget.Texture2D, tankId);

                data = tank.LockBits(new System.Drawing.Rectangle(0, 0, tank.Width, tank.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                tank.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //HTank
                GL.GenTextures(1, out HtankId);
                GL.BindTexture(TextureTarget.Texture2D, HtankId);

                data = Htank.LockBits(new System.Drawing.Rectangle(0, 0, Htank.Width, Htank.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                Htank.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Artillery
                GL.GenTextures(1, out artilleryId);
                GL.BindTexture(TextureTarget.Texture2D, artilleryId);

                data = artillery.LockBits(new System.Drawing.Rectangle(0, 0, artillery.Width, artillery.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                artillery.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Hero
                GL.GenTextures(1, out heroId);
                GL.BindTexture(TextureTarget.Texture2D, heroId);

                data = hero.LockBits(new System.Drawing.Rectangle(0, 0, hero.Width, hero.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                hero.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                //Base
                /*GL.GenTextures(1, out central_baseId);
                GL.BindTexture(TextureTarget.Texture2D, central_baseId);

                data = central_base.LockBits(new System.Drawing.Rectangle(0, 0, central_base.Width, central_base.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                central_base.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                */
                central_baseId = TexUtil.CreateTextureFromBitmap(central_base);
                
                //Barrack
                GL.GenTextures(1, out barrackId);
                GL.BindTexture(TextureTarget.Texture2D, barrackId);

                data = barrack.LockBits(new System.Drawing.Rectangle(0, 0, barrack.Width, barrack.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                barrack.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                //Factory
                GL.GenTextures(1, out factoryId);
                GL.BindTexture(TextureTarget.Texture2D, factoryId);

                data = factory.LockBits(new System.Drawing.Rectangle(0, 0, factory.Width, factory.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                factory.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                //Mine
                GL.GenTextures(1, out mineId);
                GL.BindTexture(TextureTarget.Texture2D, mineId);

                data = mine.LockBits(new System.Drawing.Rectangle(0, 0, mine.Width, mine.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                mine.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                //Portal
                GL.GenTextures(1, out portalId);
                GL.BindTexture(TextureTarget.Texture2D, portalId);

                data = portal.LockBits(new System.Drawing.Rectangle(0, 0, portal.Width, portal.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                portal.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //End of texture mapping

                SetupViewport();
            }

            public void unload(EventArgs e)
            {
                GL.DeleteTextures(1, ref workerId);
                GL.DeleteTextures(1, ref infantryId);
                GL.DeleteTextures(1, ref heavyId);
                GL.DeleteTextures(1, ref sniperId);
                GL.DeleteTextures(1, ref tankId);
                GL.DeleteTextures(1, ref HtankId);
                GL.DeleteTextures(1, ref artilleryId);
                GL.DeleteTextures(1, ref heroId);
            }

            public void Application_Idle(object sender, EventArgs e)
            {
                glControl.Invalidate();
            }


            public void paint(object sender, PaintEventArgs e)
            {
                if (!loaded) // Play nice
                    return;
                draw();
            }

            public void resize(object sender, EventArgs e)
            {
                SetupViewport();
                glControl.Invalidate();
            }

            public void draw()
            {
                if (ready)
                {
                    if (false)
                    //if (!sim.running)
                    {
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();

                        for (int i = 0; i < mapSize; i++)
                        {
                            for (int j = 0; j < mapSize; j++)
                            {
                                /*if (sim.map.terrain[i, j].z < 1 * maxHeight / 10){
                                    GL.Color3(Color.DarkBlue);
                                }else if (sim.map.terrain[i, j].z < 2 * maxHeight / 10){
                                    GL.Color3(Color.LightBlue);
                                }else if (sim.map.terrain[i, j].z < 3 * maxHeight / 10){
                                    GL.Color3(Color.SandyBrown);
                                }else if (sim.map.terrain[i, j].z < 4 * maxHeight / 10){
                                    GL.Color3(Color.LightGreen);
                                }else if (sim.map.terrain[i, j].z < 5 * maxHeight / 10){
                                    GL.Color3(Color.Green);
                                }else if (sim.map.terrain[i, j].z < 6 * maxHeight / 10){
                                    GL.Color3(Color.DarkGreen);
                                }else if (sim.map.terrain[i, j].z < 7 * maxHeight / 10){
                                    GL.Color3(Color.Brown);
                                }else if (sim.map.terrain[i, j].z < 8 * maxHeight / 10){
                                    GL.Color3(Color.GhostWhite);
                                }else if (sim.map.terrain[i, j].z < 9 * maxHeight / 10){
                                    GL.Color3(Color.Gray);
                                }else{
                                    GL.Color3(Color.White);
                                }*/

                               /* int color = (int)(((sim.map.terrain[i, j].z) / maxHeight) * 255.0);
                                GL.Color3(Color.FromArgb(color, color, color));

                                GL.Begin(BeginMode.Polygon);
                                GL.Vertex2(i * tileWidth, j * tileHeight);
                                GL.Vertex2(i * tileWidth + tileWidth, j * tileHeight);
                                GL.Vertex2(i * tileWidth + tileWidth, j * tileHeight + tileHeight);
                                GL.Vertex2(i * tileWidth, j * tileHeight + tileHeight);
                                GL.End();*/


                            }
                        }

                        glControl.SwapBuffers();
                    }
                    //else if (sim.readable)
                    else if (false)
                    {
                        
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();

                        for (int i = 0; i < mapSize; i++)
                        {
                            for (int j = 0; j < mapSize; j++)
                            {
                                /*if (sim.map.terrain[i, j].z < 1 * maxHeight / 10){
                                    GL.Color3(Color.DarkBlue);
                                }else if (sim.map.terrain[i, j].z < 2 * maxHeight / 10){
                                    GL.Color3(Color.LightBlue);
                                }else if (sim.map.terrain[i, j].z < 3 * maxHeight / 10){
                                    GL.Color3(Color.SandyBrown);
                                }else if (sim.map.terrain[i, j].z < 4 * maxHeight / 10){
                                    GL.Color3(Color.LightGreen);
                                }else if (sim.map.terrain[i, j].z < 5 * maxHeight / 10){
                                    GL.Color3(Color.Green);
                                }else if (sim.map.terrain[i, j].z < 6 * maxHeight / 10){
                                    GL.Color3(Color.DarkGreen);
                                }else if (sim.map.terrain[i, j].z < 7 * maxHeight / 10){
                                    GL.Color3(Color.Brown);
                                }else if (sim.map.terrain[i, j].z < 8 * maxHeight / 10){
                                    GL.Color3(Color.GhostWhite);
                                }else if (sim.map.terrain[i, j].z < 9 * maxHeight / 10){
                                    GL.Color3(Color.Gray);
                                }else{
                                    GL.Color3(Color.White);
                                }*/

                               /* int color = (int)(((sim.map.terrain[i, j].z) / maxHeight) * 255.0);
                                GL.Color3(Color.FromArgb(color, color, color));*/

                                GL.Begin(BeginMode.Polygon);
                                GL.Vertex2(i * tileWidth, j * tileHeight);
                                GL.Vertex2(i * tileWidth + tileWidth, j * tileHeight);
                                GL.Vertex2(i * tileWidth + tileWidth, j * tileHeight + tileHeight);
                                GL.Vertex2(i * tileWidth, j * tileHeight + tileHeight);
                                GL.End();

                            }
                        }

                    
                        for(int i = 0; i < objects.Count; i++)
                        {
                            /*GameObject element = objects.ElementAt(i);
                            GL.Color3(Color.Black);
                            switch (element.getTeam())
                            {
                                case 0:
                                    GL.Color3(Color.Blue);
                                    break;
                                case 1:
                                    GL.Color3(Color.Green);
                                    break;
                            }
                            if(element.getWasAttacked())
                                GL.Color3(Color.Red);

                            GL.Begin(BeginMode.Polygon);
                            GL.Vertex2(element.getX() * tileWidth + rationW / 2, element.getY() * tileHeight + rationH / 2);
                            GL.Vertex2(element.getX() * tileWidth + tileWidth - rationW / 2, element.getY() * tileHeight + rationH / 2);
                            GL.Vertex2(element.getX() * tileWidth + tileWidth - rationW / 2, element.getY() * tileHeight + tileHeight - rationH / 2);
                            GL.Vertex2(element.getX() * tileWidth + rationW / 2, element.getY() * tileHeight + tileHeight - rationH / 2);
                            GL.End();

                            GL.Color3(Color.White);
                                                       
                            switch (element.getType())
                            {
                                case 'A':
                                    GL.BindTexture(TextureTarget.Texture2D, central_baseId);
                                    break;
                                case 'B':
                                    GL.BindTexture(TextureTarget.Texture2D, mineId);
                                    break;
                                case 'C':
                                    GL.BindTexture(TextureTarget.Texture2D, barrackId);
                                    break;
                                case 'D':
                                    GL.BindTexture(TextureTarget.Texture2D, factoryId);
                                    break;
                                case 'E':
                                    GL.BindTexture(TextureTarget.Texture2D, portalId);
                                    break;
                                case 'L':
                                    GL.BindTexture(TextureTarget.Texture2D, workerId);
                                    break;
                                case 'M':
                                    GL.BindTexture(TextureTarget.Texture2D, infantryId);
                                    break;
                                case 'N':
                                    GL.BindTexture(TextureTarget.Texture2D, heavyId);
                                    break;
                                case 'O':
                                    GL.BindTexture(TextureTarget.Texture2D, sniperId);
                                    break;
                                case 'P':
                                    GL.BindTexture(TextureTarget.Texture2D, tankId);
                                    break;
                                case 'Q':
                                    GL.BindTexture(TextureTarget.Texture2D, HtankId);
                                    break;
                                case 'R':
                                    GL.BindTexture(TextureTarget.Texture2D, artilleryId);
                                    break;
                                case 'S':
                                    GL.BindTexture(TextureTarget.Texture2D, heroId);
                                    break;
                            }

                            GL.Begin(BeginMode.Quads);

                            double spriteW = tileWidth - rationW;
                            double spriteH = tileHeight- rationH;

                            if (spriteW > (spriteH * 1.1f))
                            {
                                spriteW = spriteH;
                            }

                            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(element.getX() * tileWidth + rationW, element.getY() * tileHeight + rationH);
                            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(element.getX() * tileWidth + spriteW, element.getY() * tileHeight + rationH);
                            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(element.getX() * tileWidth + spriteW, element.getY() * tileHeight + spriteH);
                            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(element.getX() * tileWidth + rationW, element.getY() * tileHeight + spriteH);

                            GL.End();

                            if (!element.getWasAttacked())
                                GL.Color3(Color.Black);
                            GL.Begin(BeginMode.Polygon);
                            GL.Vertex2(element.getX() * tileWidth,                                                              element.getY() * tileHeight + (tileHeight * 0.7));
                            GL.Vertex2(element.getX() * tileWidth + tileWidth * (element.getHealth() / Constants.MAX_HEALTH),   element.getY() * tileHeight + (tileHeight * 0.7));
                            GL.Vertex2(element.getX() * tileWidth + tileWidth * (element.getHealth() / Constants.MAX_HEALTH),   element.getY() * tileHeight + tileHeight - (tileHeight / 5));
                            GL.Vertex2(element.getX() * tileWidth,                                                              element.getY() * tileHeight + tileHeight - (tileHeight / 5)); 
                            GL.End();*/
                        }

                        if (action[0] != -1)
                        {
                            GL.Color3(Color.Yellow);
                            GL.Begin(BeginMode.Lines);
                            GL.Vertex2(action[0] * tileWidth + tileWidth / 2, action[1] * tileHeight + tileHeight / 2);
                            GL.Vertex2(action[2] * tileWidth + tileWidth / 2, action[3] * tileHeight + tileHeight / 2);
                            GL.End();
                            GL.Color3(Color.Yellow);
                            GL.Begin(BeginMode.Triangles);
                            GL.Vertex2(action[2] * tileWidth + tileWidth / 2 - 2, action[3] * tileHeight + tileHeight / 2 - 2);
                            GL.Vertex2(action[2] * tileWidth + tileWidth / 2 - 2, action[3] * tileHeight + tileHeight / 2 + 2);
                            GL.Vertex2(action[2] * tileWidth + tileWidth / 2 + 2, action[3] * tileHeight + tileHeight / 2);
                            GL.End();
                        }
                        glControl.SwapBuffers();
                    }

                }
            }

            public static int CreateTextureFromBitmap(Bitmap bitmap)
            {
                Img.BitmapData data = bitmap.LockBits(
                  new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                  Img.ImageLockMode.ReadOnly,
                  Img.PixelFormat.Format32bppArgb);
                var tex = GiveMeATexture();
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.TexImage2D(
                  TextureTarget.Texture2D,
                  0,
                  PixelInternalFormat.Rgba,
                  data.Width, data.Height,
                  0,
                  OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                  PixelType.UnsignedByte,
                  data.Scan0);
                bitmap.UnlockBits(data);
                SetParameters();
                return tex;
            }

            public static int CreateTextureFromFile(string path)
            {
                return CreateTextureFromBitmap(new Bitmap(Bitmap.FromFile(path)));
            }

            private static int GiveMeATexture()
            {
                int tex = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, tex);
                return tex;
            }

            private static void SetParameters()
            {
                GL.TexParameter(
                  TextureTarget.Texture2D,
                  TextureParameterName.TextureMinFilter,
                  (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D,
                  TextureParameterName.TextureMagFilter,
                  (int)TextureMagFilter.Linear);
            }
        }
}
