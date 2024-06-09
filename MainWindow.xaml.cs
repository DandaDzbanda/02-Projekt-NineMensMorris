public partial class MainWindow 
    {
        private Game game; // Instance třídy Game, která spravuje logiku hry

        public MainWindow()
        {
            InitializeComponent(); // Inicializuje komponenty WPF
            game = new Game(); // Vytvoří novou instanci 
            UpdateUI(); // Aktualizuje uživatelské rozhraní
        }

        private void Position_Click(object sender, MouseButtonEventArgs e)
        {
            // Získá pozici na desce, na kterou bylo kliknuto
            Ellipse ellipse = (Ellipse)sender;
            int index = int.Parse(ellipse.Name.Substring(3));

            // Pokusí se provést tah na daném indexu
            if (game.MakeMove(index))
            {
                UpdateUI(); // Aktualizuje uživatelské rozhraní po tahu
                            // Zkontroluje, zda má hráč odstranit soupeřův kámen
                if (game.RemovingPiece)
                {
                    CurrentPlayerTextBlock.Text = $"{game.CurrentPlayer.Name}, remove an opponent's piece.";
                }
                else
                {
                    UpdateCurrentPlayerText(); // Aktualizuje text aktuálního hráče
                }
            }
        }

        private void UpdateUI()
        {
            // Projde všechny pozice na desce
            for (int i = 0; i < 24; i++)
            {
                // Najde kruh pro každou pozici
                Ellipse ellipse = (Ellipse)BoardCanvas.FindName($"Pos{i}");
                // Nastaví barvu kruhu podle toho, který hráč ji obsadil
                if (game.Positions[i].IsOccupied)
                {
                    ellipse.Fill = game.Positions[i].OccupyingPlayer.Color;
                }
                else
                {
                    ellipse.Fill = Brushes.Transparent; // Ztransparentní kruh, pokud není obsazený
                }
            }
            // Zobrazí aktuální fázi hry
            PhaseTextBlock.Text = $"Phase: {game.CurrentPhase}";
        }

        private void UpdateCurrentPlayerText()
        {
            if (game.CurrentPlayer.Name == "Player 1")
            {
                CurrentPlayerTextBlock.Text = $"Current Player: {game.CurrentPlayer.Name}";
                CurrentPlayerTextBlock.Foreground = Brushes.Red; 
            }
            else if (game.CurrentPlayer.Name == "Player 2")
            {
                CurrentPlayerTextBlock.Text = $"Current Player: {game.CurrentPlayer.Name}";
                CurrentPlayerTextBlock.Foreground = Brushes.Blue; 
            }
        }

    }
