﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace childhood_games_pack.tennis {
    public enum GAME_STATUS { IN_GAME, STOP };
    public partial class TennisGame : Form {
        private MainMenuForm mainMenu;
        private Graphics table { get; }
        public UserRacket userRacket { get; }
        public CompRacket compRacket { get; }
        public Ball ball { get; }
        public GAME_STATUS gameStatus { get; set; }

        public TennisGame(MainMenuForm mainMenu) {
            InitializeComponent();
            this.mainMenu = mainMenu;
            gameStatus = GAME_STATUS.IN_GAME;

            table = TablePanel.CreateGraphics();

            userRacket = new UserRacket(this);
            compRacket = new CompRacket(this);
            ball = new Ball(this);

            TablePanel.Controls.Add(userRacket);
            TablePanel.Controls.Add(compRacket);
            TablePanel.Controls.Add(ball);

            userRacket.Show();
            compRacket.Show();
            ball.Show();
        }

        private void TennisMainForm_FormClosed(object sender, FormClosedEventArgs e) {
            gameStatus = GAME_STATUS.STOP;
            mainMenu.Show();
        }

        private void TablePanel_Paint(object sender, PaintEventArgs e) {
            Pen whitePen = new Pen(Color.White, 3);
            Point leftPoint = new Point(0, TablePanel.Height / 2);
            Point rightPoint = new Point(TablePanel.Width, TablePanel.Height / 2);
            table.DrawLine(whitePen, leftPoint, rightPoint);
        }
    }

}
