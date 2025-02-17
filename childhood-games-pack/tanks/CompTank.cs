﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;


namespace childhood_games_pack.tanks {
    public partial class CompTank : Form {
        public DIRECTION direction = DIRECTION.D;

        private TANK_TYPE type;
        private SPEED_LEVEL speedLevel;
        private TanksGame game;

        private int step; // pxl
        private int stepTimer; // ms
        
        public CompTank(TANK_TYPE type, SPEED_LEVEL speedLevel, Point spot) {
            InitializeComponent();
            SetTopLevel(false);
            AutoSize = false;

            this.type = type;
            this.speedLevel = speedLevel;
            this.game = TanksGame.gameRef;

            speedInit();
            shape();

            Location = spot;
            Size = new Size(TanksGame.tankWidth, TanksGame.tankHeight);
            
            walkAndAttackWorker.RunWorkerAsync();
        }

        private void speedInit() {
            switch (speedLevel) {
                case SPEED_LEVEL.LOW:
                    step = 20;
                    stepTimer = 3000;
                    break;

                case SPEED_LEVEL.MEDIUM:
                    step = 20;
                    stepTimer = 2000;
                    break;

                case SPEED_LEVEL.HIGHT:
                    step = 20;
                    stepTimer = 1000;
                    break;

                case SPEED_LEVEL.NONE:
                default:
                    throw new Exception("Wrong speed level");
            }
        }

        //! Change shape of form depending on the tank-type.
        private void shape() {
            switch (type) {
                case TANK_TYPE.LIGHT:
                    BackgroundImage = Properties.Resources.light_ctank_u;
                    break;

                case TANK_TYPE.MEDIUM:
                    BackgroundImage = Properties.Resources.medium_tank;
                    break;

                case TANK_TYPE.HEAVY:
                    BackgroundImage = Properties.Resources.heavy_tank;
                    break;

                case TANK_TYPE.NONE:
                default:
                    throw new Exception("Wrong type of Tank");
            }
        }

        private void walkAndAttackWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            Random rnd = new Random();

            while (true) {
                direction = (DIRECTION)(rnd.Next() % 4);
                int isNeedShot = rnd.Next() % 3000;

                if (isNeedShot < 1000) {
                    TanksGame.ShootKeyDown(Keys.B);
                }

                switch (direction) {
                    case DIRECTION.U:
                        BackgroundImage = Properties.Resources.light_ctank_u;

                        Point newUpLoc = new Point(Location.X, Location.Y - step);
                        if (newUpLoc.Y <= 0) {
                            break;
                        }

                        Location = newUpLoc;
                        break;

                    case DIRECTION.D:
                        BackgroundImage = Properties.Resources.light_ctank_d;

                        Point newDownLoc = new Point(Location.X, Location.Y + step);
                        if (newDownLoc.Y >= 600) {
                            break;
                        }

                        Location = newDownLoc;
                        break;

                    case DIRECTION.L:
                        BackgroundImage = Properties.Resources.light_ctank_l;

                        Point newLeftLoc = new Point(Location.X - step, Location.Y);
                        if (newLeftLoc.X <= 0) {
                            break;
                        }

                        Location = newLeftLoc;
                        break;

                    case DIRECTION.R:
                        BackgroundImage = Properties.Resources.light_ctank_r;

                        Point newRightLoc = new Point(Location.X + step, Location.Y);
                        if (newRightLoc.X >= 1200) {
                            break;
                        }

                        Location = newRightLoc;
                        break;
                }

                Thread.Sleep(stepTimer);
            }
        }
    }
}
