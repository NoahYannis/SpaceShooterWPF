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
using System.Diagnostics;

namespace SpaceShooter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft;
        bool moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random random = new Random();

        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        int enemySpeed = 10;
        int limit = 50;
        int score = 0;
        int damage = 0;

        Rect playerHitBox;
        

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            SpielCanvas.Focus();


            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            SpielCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\player.png"));
            Player.Fill = playerImage;

        }

        private void GameLoop(object sender, EventArgs e)
        {
            playerHitBox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            enemyCounter -= 1;

            lblScoreText.Content = "Score: " + score;
            lblDamageText.Content = "Damage: " + damage;

            if (enemyCounter < 0)
            {
                MakeEnemies();
                enemyCounter = limit;
            }

            if (moveLeft == true && Canvas.GetLeft(Player) > 0 )
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(Player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + playerSpeed);
            }

            foreach (var x in SpielCanvas.Children.OfType<Rectangle>())
            {

                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bulletHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in SpielCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                           
                            if (bulletHitbox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score++;
                            }
                        }
                    }
                }

                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);
                    }

                    if (Canvas.GetTop(x) > 750)

                    {
                        itemRemover.Add(x);
                        damage += 10;
                    }

                    Rect enemyHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(enemyHitbox))
                    {
                        itemRemover.Add(x);
                        damage += 5;
                    }

                }



            }

            foreach (Rectangle i in itemRemover)
            {
                SpielCanvas.Children.Remove(i);
            }


            if (score > 5)
            {
                limit = 20;
                enemySpeed = 15;
                playerSpeed = 15;
            }

            if (damage > 99)
            {
                gameTimer.Stop();
                lblDamageText.Content = "Damage: 100";
                lblDamageText.Foreground = Brushes.Red;
                MessageBox.Show("Kapitän: Du hast " + score + " feindliche Schiffe zerstört!" + Environment.NewLine  + "Drücke Ok, um erneut zu spielen", "Daimyon:");

                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

            }
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red

                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(Player) + Player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(Player) - newBullet.Height);
                SpielCanvas.Children.Add(newBullet);
            }
        }

 
        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();
            enemySpriteCounter = random.Next(1, 6);

            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\1.png"));
                    break;

                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\2.png"));
                    break;

                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\3.png"));
                    break;

                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\4.png"));
                    break;

                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri(@"C:\Users\User\Downloads\mooict-com-wpf-c-space-shooter-images\5.png"));
                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite,
            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, random.Next(30, 450));
            SpielCanvas.Children.Add(newEnemy);
        }
    }

}

