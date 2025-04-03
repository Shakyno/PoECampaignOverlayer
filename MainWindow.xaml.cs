using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Overlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OverlayData OverlayData { get; set; }
        readonly ObservableCollection<TextBlock> removedTextBlocks = [];
        int counter = 0;

        Settings settings;
        readonly List<int> actPositions = [];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RemovedTextBlocks_Changed(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.pnlText.Children.Count <= 2)
            {
                btnNext.IsEnabled = false;
                btnNext.Opacity = 0.2;
            }
            else
            {
                btnNext.IsEnabled = true;
                btnNext.Opacity = 1;
            }

            if (counter == 0)
            {
                btnBack.IsEnabled = false;
                btnBack.Opacity = 0.2;
            }
            else
            {
                btnBack.IsEnabled = true;
                btnBack.Opacity = 1;
            }
        }

        private void ReadSettings()
        {
            settings = Settings.ReadSettings();
            OverlayData = new OverlayData(settings);
            counter = (settings.CurrentStep >= 0 && settings.CurrentStep < OverlayData.StepCountWithoutActs ? settings.CurrentStep : 0) ?? 0;
        }

        private void AddAllTextBlocks()
        {
            foreach (var text in OverlayData.CampaignText)
            {
                TextBlock textBlock = new()
                {
                    Text = text,
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(5),
                    Width = this.pnlText.ActualWidth,
                    MaxHeight = 219
                };

                this.pnlText.Children.Add(textBlock);
            }
        }

        private void SetUserName()
        {
            this.lblUserName.Content = OverlayData.Settings.UserName;
        }

        private void RemoveActTextBlocks()
        {
            List<int> tempActPositions = [];

            for (int i = 0; i < this.pnlText.Children.Count; i++)
            {
                if (this.pnlText.Children[i] is TextBlock block)
                {
                    if (block.Text.StartsWith("act"))
                    {
                        actPositions.Add(i - actPositions.Count);
                        tempActPositions.Add(i);

                    }
                }
            }

            //remove act text blocks from pnltext starting with the last one
            for (int i = tempActPositions.Count - 1; i >= 0; i--)
            {
                this.pnlText.Children.RemoveAt(tempActPositions[i]);
            }
        }

        private void SetInitialButtonOpacity()
        {
            if (counter == 0)
            {
                this.btnBack.Opacity = 0.2;
                this.btnBack.IsEnabled = false;
            }
            if (counter >= OverlayData.StepCountWithoutActs)
            {
                this.btnNext.Opacity = 0.2;
                this.btnNext.IsEnabled = false;
            }
        }

        private void RemoveInitialTextBlocks()
        {
            if (counter == 0)
            {
                return;
            }

            for (int i = 0; i < counter - 1; i++)
            {
                if (this.pnlText.Children.Count <= 2)
                {
                    return;
                }
                var blockToRemove = (TextBlock)this.pnlText.Children[0];
                this.pnlText.Children.RemoveAt(0);
                removedTextBlocks.Add(blockToRemove);
            }
        }

        private void SetInitialTextBlockTextColor()
        {
            if (counter == 0)
            {
                if ((this.pnlText.Children[0] is TextBlock currentBlock))
                {
                    currentBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                    currentBlock.FontWeight = FontWeights.Bold;
                }
            }
            else
            {
                if ((this.pnlText.Children[1] is TextBlock currentBlock))
                {
                    currentBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                    currentBlock.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void RemoveTextBlock()
        {
            if (counter == 0)
            {
                return;
            }

            if (this.pnlText.Children.Count > 2)
            {
                var blockToRemove = (TextBlock)this.pnlText.Children[0];
                this.pnlText.Children.RemoveAt(0);
                removedTextBlocks.Add(blockToRemove);
            }
        }

        private void SetNextBlockTextColor()
        {
            if (this.pnlText.Children.Count < 2)
            {
                return;
            }

            if (this.pnlText.Children[1] is TextBlock currentBlock)
            {
                currentBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                currentBlock.FontWeight = FontWeights.Bold;
            }
            if (this.pnlText.Children[0] is TextBlock previousBlock)
            {
                previousBlock.Foreground = new SolidColorBrush(Colors.Black);
                previousBlock.FontWeight = FontWeights.Normal;
            }
        }

        private void AddPreviousTextBlock()
        {
            if (removedTextBlocks.Count == 0)
            {
                return;
            }

            this.pnlText.Children.Insert(0, removedTextBlocks[^1]);
            removedTextBlocks.RemoveAt(removedTextBlocks.Count - 1);
        }

        private void SetPreviousBlockTextColor()
        {
            if (this.pnlText.Children.Count < 2)
            {
                return;
            }

            if (counter == 1)
            {
                if ((this.pnlText.Children[0] is TextBlock firstBlock))
                {
                    firstBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                    firstBlock.FontWeight = FontWeights.Bold;
                }
                if ((this.pnlText.Children[1] is TextBlock secondBlock))
                {
                    secondBlock.Foreground = new SolidColorBrush(Colors.Black);
                    secondBlock.FontWeight = FontWeights.Normal;
                }
                RemovedTextBlocks_Changed(null, null);
            }
            else
            {

                if ((this.pnlText.Children[1] is TextBlock secondBlock))
                {
                    secondBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                    secondBlock.FontWeight = FontWeights.Bold;
                }
                if ((this.pnlText.Children[2] is TextBlock thirdBlock))
                {
                    thirdBlock.Foreground = new SolidColorBrush(Colors.Black);
                    thirdBlock.FontWeight = FontWeights.Normal;
                }
            }

        }

        private void UpdateActLabel()
        {
            int actCount = actPositions.Count;

            for (int i = 0; i < actCount; i++)
            {
                if (counter >= actPositions[i])
                {
                    lblAct.Content = $"Act {i + 1}";
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Save settings to the settings file.
        /// </summary>
        private void SaveSettings()
        {
            OverlayData.Settings.SaveSettings();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            OverlayData.Settings.CurrentStep = counter;
            SaveSettings();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            RemoveTextBlock();
            SetNextBlockTextColor();

            counter++;
            UpdateActLabel();

            if (counter == 1)
            {
                btnBack.IsEnabled = true;
                btnBack.Opacity = 1;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AddPreviousTextBlock();
            SetPreviousBlockTextColor();

            counter--;
            UpdateActLabel();

            if (counter == 0)
            {
                btnBack.IsEnabled = false;
                btnBack.Opacity = 0.2;
            }
        }

        private void PnlText_Loaded(object sender, RoutedEventArgs e)
        {
            SetupOverlayer();
        }

        private void SetupOverlayer()
        {
            ReadSettings();
            AddAllTextBlocks();
            RemoveActTextBlocks();
            UpdateActLabel();
            RemovedTextBlocks_Changed(null, null);
            SetUserName();
            SetInitialButtonOpacity();
            removedTextBlocks.CollectionChanged += RemovedTextBlocks_Changed;
            RemoveInitialTextBlocks();
            SetInitialTextBlockTextColor();
        }
    }
}