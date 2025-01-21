using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MineField
{
    public partial class Form1 : Form
    {
        //Music/Sound
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        System.Media.SoundPlayer playerwin = new System.Media.SoundPlayer();
        System.Media.SoundPlayer playerlose = new System.Media.SoundPlayer();

        //Ticking for the timer2
        private int _ticks;

        //Elapsed Timer
        System.Timers.Timer elapsedTimer;

        //Minutes and seconds for the timer
        int m, s;


        public Form1()
        {
            InitializeComponent();

            //Calls the music/sound for the game
            player.SoundLocation = "backgroundmusic.wav";
            playerwin.SoundLocation = "winsound.wav";
            playerlose.SoundLocation = "losesound.wav";

            //Plays the music for the game at the start
            player.Play();
        }



        //The time duration of the overal game
        int duration = 60;

        //Set start locaton of sprite
        int atCol = 9;
        int atRow = 19;

        //Boolean array holds the bombs locations
        bool[,] bombs = new bool[20, 20];

        private void Form1_Load(object sender, EventArgs e)
        {
            label10.Image = Properties.Resources.key;
            showSpriteAt(atCol, atRow);     //Set sprite to start location
            //plantBombs(40);                 //Set up a bomb field

            //Timer for the elapsed time
            elapsedTimer = new System.Timers.Timer();
            elapsedTimer.Interval = 1000;
            elapsedTimer.Elapsed += OnTimeEvent;

            //Start the timer for the elapsed time as soon as the form loads for the game
            elapsedTimer.Start();

            //Timer for the countdown time
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Tick += new EventHandler(count_down);
            countdownTimer.Interval = 1000;
            //countdownTimer.Start();


            btn1.Enabled = false;       //Disable button 1
            btn2.Enabled = false;       //Disable button 2
            btn3.Enabled = false;       //Disable button 3
            btn4.Enabled = false;       //Disable button 4
            btnJumpOver.Enabled = false;       //Disable button to jump over
            btnShowBombs.Enabled = false;       //Disable button to show bombs
        }



        //Calls when the countdown ends
        private void count_down(object sender, EventArgs e)
        {

            if (duration == 0)              //Once the countdown timer for the game reaches 0
            {
                this.BackColor = Color.LightBlue;       //Background colour
                btn1.Enabled = false;       //Disable button 1
                btn2.Enabled = false;       //Disable button 2
                btn3.Enabled = false;       //Disable button 3
                btn4.Enabled = false;       //Disable button 4
                btnJumpOver.Enabled = false;       //Disable button to jump over
                btnShowBombs.Enabled = false;       //Disable button to show bombs
                showBombs();                //Show the bombs is called
                lblBombCount.Text = "Oh no!!! You have run out of time! Try again! :(";     //Display text once the countdown runs out
                elapsedTimer.Stop();        //Stop the elapsed timer
                countdownTimer.Stop();      //Stop the countdown timer
                player.Stop();              //Stop the music
            }
            else if (duration > 0)          //Otherwise if the countdown timer remains continue to tick down
            {
                duration--;                 //Keeps ticking down by 1 fro the countdown timer
                txtCountdownTimer.Text = duration.ToString();       //Display the time of the countdown timer
            }
        }



        //This is used for the elapsed timer for the game to display the seconds and the minutes
        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {     
                //Seconds are being setup for the elapsed timer
                s += 1;
                if (s == 60)
                {
                    s = 0;
                    m += 1;
                }
                //Minutes are being setup for the elapsed timer
                if (m == 60)
                {
                    m = 0;
                }
                //Displays the elapsed timer minutes and seconds within a textbox on the game
                txtElapsedTimer.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
            }));
        }



        //Plant the bombs on the field
        private void plantBombs(int target)
        {
            Random r = new Random();    //Create a random number generator

            //Set variables
            int tryCol, tryRow;
            int setSoFar = 0;

            //Clear all current bombs
            Array.Clear(bombs, 0, bombs.Length);

            //Loop to fill array with the described number of bombs
            do
            {
                tryCol = r.Next(0, 20);
                tryRow = r.Next(1, 19);     //No bombs on the top and bottom row



                //if (tryRow == tryCol -1 )



                if (!bombs[tryCol, tryRow])
                {
                    bombs[tryCol, tryRow] = true;
                    setSoFar++;
                }
            } while (setSoFar < target);
        }



        //Count and return number of surrounding bombs
        private int bombCount(int Col, int Row)
        {
            int sofar = 0;
            if (Col > 0)
            {
                if (bombs[Col - 1, Row])
                    sofar++;
            }

            if (Col < 18)
            {
                if (bombs[Col + 1, Row])
                    sofar++;
            }

            if (Row > 0)
            {
                if (bombs[Col, Row - 1])
                    sofar++;
            }

            if (Row < 18)
            {
                if (bombs[Col, Row + 1])
                    sofar++;
            }

            return sofar;
        }



        //Check if landed on a bomb
        private void didIDie(int Col, int Row)
        {
            if (bombs[Col, Row])
            {
                this.BackColor = Color.Orange;      //Background colour
                btn1.Enabled = false;       //Disable button 1
                btn2.Enabled = false;       //Disable button 2
                btn3.Enabled = false;       //Disable button 3
                btn4.Enabled = false;       //Disable button 4
                btnJumpOver.Enabled = false;       //Disable button to jump over
                btnShowBombs.Enabled = false;       //Disable button to show bombs
                showBombs();                //Show the bombs is called
                lblBombCount.Text = "Well!!! You died, you suck at this ;)";        //Display text if the player touches a bomb
                playerlose.Play();          //Sound for hitting a bomb
                elapsedTimer.Stop();        //Stop the elapsed timer
                countdownTimer.Stop();      //Stop the countdown timer
            }

            else if (atCol == 9 && atRow == 0)      //If the player reaches the key to win
            {
                this.BackColor = Color.Green;       //Background colour
                btn1.Enabled = false;       //Disable button 1
                btn2.Enabled = false;       //Disable button 2
                btn3.Enabled = false;       //Disable button 3
                btn4.Enabled = false;       //Disable button 4
                btnJumpOver.Enabled = false;       //Disable button to jump over
                btnShowBombs.Enabled = false;       //Disable button to show bombs
                showBombs();                //Show the bombs is called
                lblBombCount.Text = "Wow!!! You achieved the impossible." +
                                    "Can you do it again haha :)";      //Display text if the player reaches the key
                playerwin.Play();           //Sound for reaching the key
                elapsedTimer.Stop();        //Stop the elapsed timer
                countdownTimer.Stop();      //Stop the countdown timer
            }

            else
            {
                //Otherwise just display if the player is near to a potential bomb
                lblBombCount.Text = "Danger level: " + bombCount(atCol, atRow);
            }
        }



        //Display bomb site
        private void showBombs()
        {
            Label lbl;
            for (int Col = 0; Col < 20; Col++)
            {
                for (int Row = 0; Row < 20; Row++)
                {
                    lbl = getLabel(Col, Row);
                    if (bombs[Col, Row])
                    {
                        lbl.BackColor = Color.Red;      //Sets the background colour
                        lbl.Image = Properties.Resources.bomb;      //Sets the image
                    }
                    else
                    {
                        lbl.BackColor = Color.Purple;       //Sets the background colour
                    }
                }
            }
        }



        //Hide bomb site
        private void hideBombs()
        {
            //Disable the button on first use
            btnShowBombs.Enabled = false;
            Label lbl;
            for (int Col = 0; Col < 20; Col++)
            {
                for (int Row = 0; Row < 20; Row++)
                {
                    lbl = getLabel(Col, Row);
                    if (bombs[Col, Row])
                    {
                        lbl.BackColor = Color.Teal;        //Sets the background colour back to original
                        lbl.Image = null;       //Sets the image to nothing to go back to the original
                    }
                    else
                    {
                        lbl.BackColor = Color.Teal;     //Sets the background colour back to original
                    }
                }
            }
        }



        //Function to show sprite at any grid location
        private void showSpriteAt(int Col, int Row)
        {
            Label lbl = getLabel(Col, Row);     //Get label at (atCol,atRow)
            lbl.BackColor = Color.LightGreen;        //Set backcolour colour
            lbl.Image = Properties.Resources.standing_up_man;     //Set to show image of the man
        }



        //Function to hide sprite at any grid location
        private void hideSpriteAt(int Col, int Row)

        //This was lb1 before changed it to lbl
        {
            Label lbl = getLabel(Col, Row);     //Get label at (atCol,atRow)
            lbl.Image = null;       //Set to show nothing as the image removing the man to hide
        }



        //Function to return a label at a given grid location
        private Label getLabel(int Col, int Row)
        {
            //K will be the label number we are seeking
            int k = Row * 20 + Col + 1;
            string s = "label" + k.ToString();

            foreach (Control c in panel1.Controls)
            {
                if (c.Name == s)
                {
                    return (Label)c;
                }
            }
            return null;
        }



        //The button to move the player up
        private void btn1_Click(object sender, EventArgs e)
        {
            if (atRow > 0)
            {
                hideSpriteAt(atCol, atRow);     //Delete sprite at current location
                atRow--;                        //Move up a row
                showSpriteAt(atCol, atRow);     //Show sprite at current location
                didIDie(atCol, atRow);          //Check to see if dead if true then call it
            }
        }


        //The button to move the player right
        private void btn2_Click(object sender, EventArgs e)
        {
            if (atCol < 19)
            {
                hideSpriteAt(atCol, atRow);     //Delete sprite at current location
                atCol++;                        //Move up a row
                showSpriteAt(atCol, atRow);     //Show sprite at current location
                didIDie(atCol, atRow);          //Check to see if dead if true then call it
            }
        }


        //The button to move the player down
        private void btn3_Click(object sender, EventArgs e)
        {
            if (atRow < 19)
            {
                hideSpriteAt(atCol, atRow);     //Delete sprite at current location
                atRow++;                        //Move up a row
                showSpriteAt(atCol, atRow);     //Show sprite at current location
                didIDie(atCol, atRow);          //Check to see if dead if true then call it
            }
        }


        //The button to move the player left
        private void btn4_Click(object sender, EventArgs e)
        {
            if (atCol > 0)
            {
                hideSpriteAt(atCol, atRow);     //Delete sprite at current location
                atCol--;                        //Move up a row
                showSpriteAt(atCol, atRow);     //Show sprite at current location
                didIDie(atCol, atRow);          //Check to see if dead if true then call it
            }
        }


        //Counter for the amount of used for the jump over button
        private int clickCounterJumpOver = 0;
        private void btnJumpOver_Click(object sender, EventArgs e)
        {
            this.clickCounterJumpOver++; //Add to the counter if button was used
            if (this.clickCounterJumpOver < 3) //Amount of uses set to 2
            {
                hideSpriteAt(atCol, atRow);     //Delete sprite at current location
                atRow--;                        //Move up a row
                atRow--;                        //Move up a row
                showSpriteAt(atCol, atRow);     //Show sprite at current location
                didIDie(atCol, atRow);          //Check to see if dead if true then call it
            }

            // if (atRow > 0)
            //{
            //hideSpriteAt(atCol, atRow);     //Delete sprite at current location
            // atRow--;                        //Move up a row
            //atRow--;                        //Move up a row
            // showSpriteAt(atCol, atRow);     //Show sprite at current location
            //didIDie(atCol, atRow);
            // }
        }



        //Arrows control keyboard navigation
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Up arrow key
            if (keyData == Keys.Up)
            {
                btn1.PerformClick();
            }


            //Right arrow key
            if (keyData == Keys.Right)
            {
                btn2.PerformClick();
            }


            //Down arrow key
            if (keyData == Keys.Down)
            {
                btn3.PerformClick();
            }


            //Left arrow key
            if (keyData == Keys.Left)
            {
                btn4.PerformClick();
            }



            //Letters control keybaord navigation
            //W key to go up
            if (keyData == Keys.W)
            {
                btn1.PerformClick();
            }


            //D key to go right
            if (keyData == Keys.D)
            {
                btn2.PerformClick();
            }


            //S key to go down
            if (keyData == Keys.S)
            {
                btn3.PerformClick();
            }


            //A key to go left
            if (keyData == Keys.A)
            {
                btn4.PerformClick();
            }


            //R key to restart game
            if (keyData == Keys.R)
            {
                btnReset.PerformClick();
            }


            //J key to use jump over function
            if (keyData == Keys.J)
            {
                btnJumpOver.PerformClick();
            }


            //P key to peak at the bombs function
            if (keyData == Keys.P)
            {
                btnShowBombs.PerformClick();
            }


            //E key to start easy mode
            if (keyData == Keys.E)
            {
                btnEasyMode.PerformClick();
            }


            //M key to start medium mode
            if (keyData == Keys.M)
            {
                btnMediumMode.PerformClick();
            }


            //H key to start hard mode
            if (keyData == Keys.H)
            {
                btnHardMode.PerformClick();
            }


            //I key to start insane mode
            if (keyData == Keys.I)
            {
                btnInsaneMode.PerformClick();
            }


            return base.ProcessCmdKey(ref msg, keyData);

        }


        private void lblBombCount_Click(object sender, EventArgs e)
        {

        }



        //Runs when the button is registered
        private void btnShowBombs_Click(object sender, EventArgs e)
        {
            timer2.Start();     //Starts the timer for the peak
            showBombs();        //Shows the player the bombs
        }



        private int clickCounterMode = 0;
        private void btnEasyMode_Click(object sender, EventArgs e)
        {
            this.clickCounterMode++;
            if (this.clickCounterMode < 2)      //Amount of uses set to 1 this one or another mode
            {
                countdownTimer.Start();
                plantBombs(30);                 //Set up a bomb field
                btn1.Enabled = true;       //Enable button 1
                btn2.Enabled = true;       //Enable button 2
                btn3.Enabled = true;       //Enable button 3
                btn4.Enabled = true;       //Enable button 4
                btnJumpOver.Enabled = true;       //Enable button to jump over
                btnShowBombs.Enabled = true;       //Enable button to show bombs
            }

        }



        private void btnMediumMode_Click(object sender, EventArgs e)
        {
            this.clickCounterMode++;
            if (this.clickCounterMode < 2)      //Amount of uses set to 1 this one or another mode
            {
                countdownTimer.Start();
                plantBombs(50);                 //Set up a bomb field
                btn1.Enabled = true;       //Enable button 1
                btn2.Enabled = true;       //Enable button 2
                btn3.Enabled = true;       //Enable button 3
                btn4.Enabled = true;       //Enable button 4
                btnJumpOver.Enabled = true;       //Enable button to jump over
                btnShowBombs.Enabled = true;       //Enable button to show bombs
            }
        }


        private void btnHardMode_Click(object sender, EventArgs e)
        {
            this.clickCounterMode++;
            if (this.clickCounterMode < 2)      //Amount of uses set to 1 this one or another mode
            {
                countdownTimer.Start();
                plantBombs(65);                 //Set up a bomb field
                btn1.Enabled = true;       //Enable button 1
                btn2.Enabled = true;       //Enable button 2
                btn3.Enabled = true;       //Enable button 3
                btn4.Enabled = true;       //Enable button 4
                btnJumpOver.Enabled = true;       //Enable button to jump over
                btnShowBombs.Enabled = true;       //Enable button to show bombs
            }
        }


        private void btnInsaneMode_Click(object sender, EventArgs e)
        {
            this.clickCounterMode++;
            if (this.clickCounterMode < 2)      //Amount of uses set to 1 this one or another mode
            {
                countdownTimer.Start();
                plantBombs(100);                 //Set up a bomb field
                btn1.Enabled = true;       //Enable button 1
                btn2.Enabled = true;       //Enable button 2
                btn3.Enabled = true;       //Enable button 3
                btn4.Enabled = true;       //Enable button 4
                btnJumpOver.Enabled = true;       //Enable button to jump over
                btnShowBombs.Enabled = false;       //Disable button to show bombs
            }
        }


        private void txtElapsedTimer_TextChanged(object sender, EventArgs e)
        {

        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            _ticks++;
            //this.Text = _ticks.ToString();

            if (_ticks == 2)
            {
                hideBombs();
                timer2.Stop();
            }
        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
