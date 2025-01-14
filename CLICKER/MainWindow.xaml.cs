using System.Text;
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

namespace CLICKER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        List<Ellipse> removeThis = new List<Ellipse>();

        int spawnRate = 60;
        int currentRate;
        
        int health = 350;
        int posX;
        int posY;
        int score = 0;

        double growthRate = 0.6;

        Random random = new Random();

        MediaPlayer playerClickSound = new MediaPlayer();
        MediaPlayer playerPopSound = new MediaPlayer();

        Uri ClickedSound;
        Uri PoppedSound;

        Brush brush;





        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            currentRate = spawnRate;
            ClickedSound = new Uri("pack://siteoforigin:,,,/ASSETS/HIT.mp3");
            PoppedSound = new Uri("pack://siteoforigin:,,,/ASSETS/Miss.mp3");

            playerClickSound.Open(ClickedSound);
            playerPopSound.Open(PoppedSound);


            
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            

            currentRate -= 2;

            if (currentRate < 1)
            {
                currentRate = spawnRate;
                posX = random.Next(15, 700);
                posY = random.Next(50, 350);

                brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255)));

                Ellipse circle = new Ellipse
                {

                    Tag = "circle",
                    Height = 10,
                    Width = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = brush
                };

                Canvas.SetLeft(circle, posX);   
                Canvas.SetTop(circle, posY);

                MyCanvas.Children.Add(circle);


            }

            foreach (var x in MyCanvas.Children.OfType<Ellipse>())
            {
                x.Height += growthRate;
                x.Width += growthRate;
                x.RenderTransformOrigin = new Point(0.5, 0.5);


                if (x.Width > 100)
                {
                    removeThis.Add(x);
                    health -= 15;
                    playerPopSound.Open(PoppedSound);
                    playerPopSound.Play();
                }
            }
            if (health > 1)
            {
                healthBar.Width = health;

            }

            else
            {
                GameOverFunction();
            }

            foreach (Ellipse i in removeThis)
            {
                MyCanvas.Children.Remove(i);
            }

            if (score > 5)
            {
                growthRate = 1;
            }

            if (score > 20)
            { 
                spawnRate = 25;
                growthRate = 1.25; 

            }
            if (score > 50)
            {
                growthRate = 1.5;
            }
            
        }


        private void ClickOnCanvas(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Ellipse)
            {
                Ellipse circle = (Ellipse)e.OriginalSource;

                MyCanvas.Children.Remove(circle);

                score++;

                playerClickSound.Position = TimeSpan.Zero;
                playerClickSound.Play();
            }

        }

        private void GameOverFunction() 
        {
            gameTimer.Stop();
            txtBubblesPopped.Text = "You Popped " + score + " Bubbles!";
            GameOverCanvas.Visibility = Visibility.Visible;

            
            
        }
        
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameOverCanvas.Visibility = Visibility.Collapsed;
            score = 0;
            
            health = 350;
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(healthBar);
            MyCanvas.Children.Add(txtScore);
            spawnRate = 60;
            growthRate = 0.6;

            
            gameTimer.Start();


        }

        


            

    }
}