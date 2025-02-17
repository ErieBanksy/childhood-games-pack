﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace childhood_games_pack.tennis {
    public partial class Ball : Form {
        private TennisGame tennisGame;
        public bool isStay = false;
        public bool isLastKickUser = true;
        public Ball(TennisGame tennisGame) {
            InitializeComponent();
            SetTopLevel(false);
            this.tennisGame = tennisGame;
            isStay = true;

            Size = new Size(tennisGame.userRacket.Size.Width / 3, tennisGame.userRacket.Size.Height);
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                if(tennisGame.gameStatus == GAME_STATUS.STOP) {
                    isStay = true;
                    return;
                }
                if (isStay) {
                    if (isLastKickUser) {
                        Location = new Point(tennisGame.userRacket.Left + tennisGame.userRacket.Size.Width / 2 - Size.Width / 2, tennisGame.userRacket.Top - tennisGame.userRacket.Size.Height);
                    }
                    else {
                        Location = new Point(tennisGame.compRacket.Left + tennisGame.compRacket.Size.Width / 2 - Size.Width / 2, tennisGame.compRacket.Top + tennisGame.compRacket.Size.Height);
                    }
                    continue;
                }

                if (isLastKickUser) {
                    if (Top > tennisGame.compRacket.Bottom) { // if the ball going from player to computer
                        if (Top - Size.Height < tennisGame.compRacket.Bottom &&
                            Right >= tennisGame.compRacket.Left &&
                            Left <= tennisGame.compRacket.Right) { // for not to cross computer racket
                            Location = new Point(Location.X, tennisGame.compRacket.Bottom);
                        }
                        else {
                            switch (tennisGame.userRacket.lastKick) {
                                case KICKS.LEFT_HAND: {
                                    Location = new Point(Location.X + 3, Location.Y - Size.Height);
                                    break;
                                }
                                case KICKS.DIRECT: {
                                    Location = new Point(Location.X, Location.Y - Size.Height);
                                    break;
                                }
                                case KICKS.RIGHT_HAND: {
                                    Location = new Point(Location.X - 3, Location.Y - Size.Height);
                                    break;
                                }
                                default:
                                    throw new Exception("Wrong shoot");
                            }
                            
                        }
                    }
                    else { // if the ball reached computer racket
                        if (Right < tennisGame.compRacket.Left  || 
                            Left > tennisGame.compRacket.Right) { // if computer racket can't kick the ball
                            Location = new Point(Location.X, 0);
                            //MessageBox.Show("You win!");
                            isStay = true;
                        }
                        else {
                            isLastKickUser = false;
                        }
                    }

                }
                else {
                    if (Bottom < tennisGame.userRacket.Top) { // if the ball going from computer to player 
                        if (Bottom + Size.Height > tennisGame.userRacket.Top &&
                            Right >= tennisGame.userRacket.Left &&
                            Left <= tennisGame.userRacket.Right) // for not to cross user racket
                        {
                            Location = new Point(Location.X, tennisGame.userRacket.Top - Size.Height);
                        }
                        else {
                            Location = new Point(Location.X, Location.Y + Size.Height);
                        }
                    }
                    else { // if the ball reached user racket
                        if (Left > tennisGame.userRacket.Right ||
                            Right < tennisGame.userRacket.Left) { // if user racket can't kick the ball
                            Location = new Point(Location.X, Location.Y + Size.Height);
                            //MessageBox.Show("You lose!");
                            isStay = true;
                        }
                        else {
                            isLastKickUser = true;
                            if(Left < tennisGame.userRacket.Left + Size.Width) {
                                tennisGame.userRacket.lastKick = KICKS.LEFT_HAND;
                            }
                            else if(Left < tennisGame.userRacket.Left + 2 * Size.Width) {
                                tennisGame.userRacket.lastKick = KICKS.DIRECT;
                            }
                            else {
                                tennisGame.userRacket.lastKick = KICKS.RIGHT_HAND;
                            }
                        }
                    }
                }
                Thread.Sleep(40);
            }
        }
    }
}
