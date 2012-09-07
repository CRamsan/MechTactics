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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MechTactics.GameElements;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Net;
using System.IO;
using MechTactics.Interfaces;

namespace MechTactics
{
    public partial class Launcher : Form
    {
        /*
         * The delegate will recieve mesages from the game server and display them in the console 
         */
        public delegate void recieveMessageCallback(int tag, String message);
        private recieveMessageCallback updateText;

        /*
         * Game server itself, Baseserver is an abstract object that can be initialized to a async or sync mode
         */
        private IServer server;

        /*
         * OpenGL thread, this thread will run the Display, which will read the state of the objects in the server and display them.
         */
        private Thread displayThread;
        private Display display;

        public Launcher()
        {
            //Initialize UI components
            InitializeComponent();
            //Define delegate and callback
            updateText = new recieveMessageCallback(this.logMessage);
            //Create a server based on the defined configs
            server = Loader.getServer();
            //Set the callback in the server so messages can be send back
            server.setRecieveMessageCallback(updateText);
            //Start the display
            display = new Display(glControl);
        }

        /*
         * This method is passed to the server so the server can send messages back
         */
        private void logMessage(int tag, string message)
        {
            try
            {
                //Remember Invoke is important becuase this method should only be called from withing the Game Thread and so
                //a new delegate has to be created to post the message in the UI thread.
                textBoxOutput.Invoke(new MethodInvoker(delegate { textBoxOutput.AppendText(message + "\n"); }));
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error with logMessage");
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
            }
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            //display.load(sender, e);
            //Application.Idle += display.Application_Idle;
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            //display.paint(sender, e);
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            //display.resize(sender, e);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            //Server stops listening for clients and starts game
            server.finishListening();
            server.startGame();
        }
        private void buttonListen_Click(object sender, EventArgs e)
        {
            //Make the server start listening for clients
            server.startListening();
            //Update some UI elements
            buttonListen.Enabled = false;
            buttonStop.Enabled = true;
            buttonPlay.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            //Stop the current activity
            if (server.isListening())
            {   
                //If the server is listening, stop that
                server.finishListening();
            }
            else if (server.isRunning())
            {
                //If the server is running a game, stop that too
                server.endGame();
            }
            //Update UI elements
            buttonStop.Enabled = false;
        }

        private void btnFileChooser_Click(object sender, EventArgs e)
        {
            //Load the file chooser with the specified parameters
            fileChooseDialog.Filter = "Game XML Files|*.msg|All Files (*.*)|*.*";
            fileChooseDialog.InitialDirectory = Application.ExecutablePath;
            if (fileChooseDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileChosen.Text = fileChooseDialog.FileName;
            }
            btnVisualize.Enabled = true;
        }
    }
}
