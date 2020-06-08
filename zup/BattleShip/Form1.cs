using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class Form1 : Form
    {

        private PictureBox[] _playerPicBox;
        private PictureBox[] _pcPicBox;
        private string[] _playerGrid;
        private string[] _pcGrid;
        private int _playerX;
        private int _playerY;
        private int _pcX;
        private int _pcY;
        private int _pcShipsNum;
        private int _playerShipsNum;
        private int _cpuLastHitCell = -1;
        private bool _cpuHasHit = false;
        private Queue _cellNeighbors = new Queue();
        private Queue _clickedCpuQueue = new Queue();


        public Form1()
        {
            InitializeComponent();
        }

        private void InitGame()
        {
            InitGrids();
            PlaceShips();

        }

        private void InitGrids()
        {
           
            _playerX = 10;
            _playerY = 100;
            _pcX = 610;
            _pcY = 100;
            _pcShipsNum = 6; 
            _playerShipsNum = 6;
            _playerPicBox = new PictureBox[100];
            _pcPicBox = new PictureBox[100];
            _playerGrid = new string[100];
            _pcGrid = new string[100];

            for (int i = 0; i < 100; i++)
            {

                _playerPicBox[i] = new PictureBox();
                _playerPicBox[i].Name = "p" + (i + 1);
                _playerPicBox[i].ImageLocation = "Water.jpg";
                _playerPicBox[i].Location = new Point(_playerX, _playerY);
                _playerPicBox[i].Size = new Size(50, 50);
                this.Controls.Add(_playerPicBox[i]);

                _playerGrid[i] = "W";

                _pcPicBox[i] = new PictureBox();
                _pcPicBox[i].Name = "c" + (i + 1);
                _pcPicBox[i].ImageLocation = "Water.jpg";
                _pcPicBox[i].Location = new Point(_pcX, _pcY);
                _pcPicBox[i].Size = new Size(50, 50);
                _pcPicBox[i].Click += new EventHandler(OnComputerClick);
                this.Controls.Add(_pcPicBox[i]);

                _pcGrid[i] = "W";

                if ((i + 1) % 10 == 0)
                {
                    _playerX = 10;
                    _playerY += 50;

                    _pcX = 610;
                    _pcY += 50;
                }
                else
                {
                    _playerX += 50;
                    _pcX += 50;
                }

            }
        }

        private void PlaceShips()
        {
            int[] availableNumbers = 
                {11, 12, 13, 14, 15, 16, 17, 18,
                21, 22, 23, 24, 25, 26, 27, 28,
                31, 32, 33, 34, 35, 36, 37, 38,
                41, 42, 43, 44, 45, 46, 47, 48,
                51, 52, 53, 54, 55, 56, 57, 58,
                61, 62, 63, 64, 65, 66, 67, 68,
                71, 72, 73, 74, 75, 76, 77, 78,
                81, 82, 83, 84, 85, 86, 87, 88};

            int shipLocation;
            for (int i = 0; i <= 5; i++)
            {
                while (true)
                {
                    Random rnd = new Random();
                    shipLocation = availableNumbers[rnd.Next(0, availableNumbers.Length - 1)];

                    if (i <= 2)
                    {
                        if (_playerGrid[shipLocation] == "W" &&
                            _playerGrid[shipLocation - 1] == "W" &&
                            _playerGrid[shipLocation + 10] == "W")
                            break;
                    }
                    else
                    {
                        if (_pcGrid[shipLocation] == "W" &&
                            _pcGrid[shipLocation - 1] == "W" &&
                            _pcGrid[shipLocation + 10] == "W")
                            break;
                    }
                }

                if (shipLocation % 2 == 0)
                    {
                       
                        if (i <= 2)
                        {
                            _playerGrid[shipLocation - 1] = "S";
                            _playerGrid[shipLocation] = "S";

                            _playerPicBox[shipLocation - 1].ImageLocation = "BattleH1.jpg";
                            _playerPicBox[shipLocation].ImageLocation = "BattleH2.jpg";

                        }
                        else
                        {
                            _pcGrid[shipLocation - 1] = "S";
                            _pcGrid[shipLocation] = "S";

                           
                    }
                    }
                    else
                    {
                         
                        if (i <= 2)
                        {
                            _playerGrid[shipLocation] = "S";
                            _playerGrid[shipLocation + 10] = "S";

                            _playerPicBox[shipLocation].ImageLocation = "BattleV1.jpg";
                            _playerPicBox[shipLocation + 10].ImageLocation = "BattleV2.jpg";

                        }
                        else
                        {
                            _pcGrid[shipLocation] = "S";
                            _pcGrid[shipLocation + 10] = "S";

                            
                    }
                    }
                

            }

            for (int i = 0; i < _pcGrid.Length; i++)
            {

                Console.Write(_pcGrid[i]);
                if (i % 10 == 0) Console.Write("\n");
            }

        }

        private void OnPlayerClick()
        {
            var cellIndex = -1;
            if (_cpuHasHit && _cellNeighbors.Count == 0 )
            {
                _cellNeighbors.Enqueue(_cpuLastHitCell -1);
                _cellNeighbors.Enqueue(_cpuLastHitCell + 1);
                _cellNeighbors.Enqueue(_cpuLastHitCell - 10);
                _cellNeighbors.Enqueue(_cpuLastHitCell + 10);
            }

            if (_cpuHasHit)
            {
                 cellIndex = (int)_cellNeighbors.Dequeue();
            }
            else
            {
                do
                {
                    Random rnd = new Random();
                    cellIndex = rnd.Next(0, 99);
                } while (_clickedCpuQueue.Contains(cellIndex));
               
            }

            if (_playerGrid[cellIndex] == "W")
            {
                _playerPicBox[cellIndex].ImageLocation = "Miss.jpg";
                if (!_cpuHasHit) _cpuLastHitCell = -1;
                
            }
            else
            {
                _playerPicBox[cellIndex].ImageLocation = "Hit.jpg";
                _cpuLastHitCell = cellIndex;
               

                if (_cpuHasHit)
                {
                    _cpuHasHit = false;
                    _cellNeighbors.Clear();
                    _cpuLastHitCell = -1;
                }
                else
                {
                    _cpuHasHit = true;
                }
                _playerShipsNum--;
            }
            _clickedCpuQueue.Enqueue(cellIndex);
            if (_playerShipsNum != 0) return;
            MessageBox.Show("CPU WON!!");
            this.Controls.Clear();
            InitGame();
        }


        private void OnComputerClick(object sender, EventArgs eventArgs)
        {
            var clickedCell = (PictureBox) sender;

            var cellIndex = Convert.ToInt32(clickedCell.Name.Substring(1, clickedCell.Name.Length - 1)) - 1;

            if (_pcGrid[cellIndex] == "W")
            {
                _pcPicBox[cellIndex].ImageLocation = "Miss.jpg";
            }
            else
            {
                _pcPicBox[cellIndex].ImageLocation = "Hit.jpg";
                _pcShipsNum--;
            }

            if (_pcShipsNum == 0) 
            {
                MessageBox.Show("YOU WON!!");
                this.Controls.Clear();
                InitGame();
               
            }

            OnPlayerClick();

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            InitGame();
        }
    }
}
