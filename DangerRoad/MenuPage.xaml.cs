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
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        int speed = 10;
        int levelIntervals = 150;
        int intervals = 150;
        Random rand = new Random();

        int carSkins;

        int[] rails = new int[4] { 90, 160, 230, 290 };
        List<Rectangle> itemRemover = new List<Rectangle>();


        public MenuPage()
        {
            InitializeComponent();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            intervals -= 5;

            if (intervals < 1)
            {
                ImageBrush carImage = new ImageBrush();

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
                }

                Rectangle newCar = new Rectangle
                {
                    Tag = "car",
                    Height = 90,
                    Width = 50,
                    Fill = carImage
                };

                Canvas.SetLeft(newCar, rails[rand.Next(0, 4)]);
                Canvas.SetTop(newCar, 600);

                MyCanvas.Children.Add(newCar);

                intervals = levelIntervals;

            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x.Tag.ToString() == "car" || x.Tag.ToString() == "bomb")
                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
            }
        }

        private void PlayGame(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            Panel.SetZIndex(PlayButton, 0);
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }
            foreach (Rectangle item in itemRemover)
            {
                MyCanvas.Children.Remove(item);
            }
            this.NavigationService.Navigate(new GamePage());
        }

    }
}
