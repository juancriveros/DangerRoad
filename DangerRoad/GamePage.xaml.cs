using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DangerRoad
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        int speed = 10;
        int levelIntervals = 150;
        int intervals = 150;
        Random rand = new Random();

        List<Rectangle> itemRemover = new List<Rectangle>();

        ImageBrush backgroungImage = new ImageBrush();

        int carSkins;
        int i;

        int missedCars;

        bool gameIsActive;

        int score;
        int lifes = 3;

        int[] rails = new int[4] { 90, 160, 230, 290 };

        MediaPlayer player = new MediaPlayer();


        public GamePage()
        {
            InitializeComponent();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            RestarGame();
        }

        private void GameEngine(object sender, EventArgs e)
        {

            scoreText.Content = "Score: " + score;

            intervals -= 5;

            if (intervals < 1)
            {
                ImageBrush carImage = new ImageBrush();

                if (score > 50)
                    carSkins = rand.Next(1, 11);
                else
                    carSkins = rand.Next(1, 6);

                switch (carSkins)
                {
                    case 1:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/BlueCar.png"));
                        break;
                    case 2:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/RedCar.png"));
                        break;
                    case 3:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/YellowCar.png"));
                        break;
                    case 4:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/GreenCar.png"));
                        break;
                    case 5:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/PurpleCar.png"));
                        break;
                    case 6:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/BlueCarBomb.png"));
                        break;
                    case 7:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/RedCarBomb.png"));
                        break;
                    case 8:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/YellowCarBomb.png"));
                        break;
                    case 9:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/GreenCarBomb.png"));
                        break;
                    case 10:
                        carImage.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/PurpleCarBomb.png"));
                        break;
                }

                Rectangle newCar = new Rectangle
                {
                    Tag = "car",
                    Height = 90,
                    Width = 50,
                    Fill = carImage
                };

                if (carSkins > 5)
                {
                    newCar.Tag = "bomb";
                }

                Canvas.SetLeft(newCar, rails[rand.Next(0, 4)]);
                Canvas.SetTop(newCar, 600);

                MyCanvas.Children.Add(newCar);

                intervals = levelIntervals;

            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x.Tag.ToString() == "car" || x.Tag.ToString() == "bomb")
                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);

                if (Canvas.GetTop(x) < -80 && x.Tag.ToString() == "car")
                {
                    itemRemover.Add(x);
                    missedCars++;
                }
            }

            if (missedCars == 10)
            {
                gameIsActive = false;
                gameTimer.Stop();
                MessageBox.Show("You missed 10 Balloons, press space to go back to menu");
                this.NavigationService.Navigate(new MenuPage());
            }

            CheckLevels();

            foreach (Rectangle item in itemRemover)
                MyCanvas.Children.Remove(item);

        }

        private void CheckLevels()
        {
            switch (score)
            {
                case 5:
                    levelIntervals = 100;
                    break;
                case 10:
                case 55:
                    speed = 15;
                    break;
                case 15:
                case 65:
                    levelIntervals = 90;
                    break;
                case 20:
                case 75:
                    speed = 20;
                    break;
                case 40:
                case 100:
                    levelIntervals = 80;
                    break;
                case 50:
                    speed = 10;
                    levelIntervals = 150;
                    break;
            }
        }

        private void SaveCar(object sender, MouseButtonEventArgs e)
        {
            if (gameIsActive)
            {
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    if (activeRec.Tag.ToString() == "car")
                    {
                        MyCanvas.Children.Remove(activeRec);
                        score += 1;
                    }
                    else if (activeRec.Tag.ToString() == "bomb")
                    {
                        ImageBrush explosion = new ImageBrush();
                        ImageBrush lostLife = new ImageBrush();
                        explosion.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/Bang.png"));
                        activeRec.Width += 10;
                        activeRec.Height += 10;
                        activeRec.Fill = explosion;
                        lostLife.ImageSource = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Resources/Images/FuelEmpty.png"));
                        switch (lifes)
                        {
                            case 3:
                                Life1.Fill = lostLife;
                                break;
                            case 2:
                                Life2.Fill = lostLife;
                                break;
                            case 1:
                                Life3.Fill = lostLife;
                                break;
                        }
                        lifes--;

                        if (lifes == 0)
                        {
                            gameIsActive = false;
                            gameTimer.Stop();
                        }

                    }

                }
            }
        }

        private void StartGame()
        {
            gameTimer.Start();

            missedCars = 0;
            score = 0;
            intervals = 150;
            gameIsActive = true;
            speed = 10;
            lifes = 3;

        }

        private void RestarGame()
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x.Tag.ToString() == "car")
                    itemRemover.Add(x);
            }

            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
            itemRemover.Clear();
            StartGame();
        }

    }
}
