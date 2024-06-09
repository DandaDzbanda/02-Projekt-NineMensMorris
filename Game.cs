public class Game
    {
        public List<Position> Positions { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public bool RemovingPiece { get; private set; }
        public string CurrentPhase { get; private set; }
        private Player player1;
        private Player player2;
        private int piecesToPlacePlayer1;
        private int piecesToPlacePlayer2;
        private bool movingPiece;
        private int selectedPieceIndex;

        public Game()
        {
            Initialize();
        }

        private void Initialize()
        {
            Positions = new List<Position>();
            for (int i = 0; i < 24; i++)
            {
                Positions.Add(new Position());
            }

            player1 = new Player("Player 1", Brushes.Red);
            player2 = new Player("Player 2", Brushes.Blue);
            CurrentPlayer = player1;
            piecesToPlacePlayer1 = 9;
            piecesToPlacePlayer2 = 9;
            CurrentPhase = "Placing";
            movingPiece = false;
            selectedPieceIndex = -1;
        }

        public bool MakeMove(int index)
        {
            if (RemovingPiece)
            {
                return RemovePiece(index);
                // Pokud je fáze odstraňování, pokusí se odstranit kámen na daném indexu.
            }
            if (CurrentPhase == "Placing")
            {
                // Pokud je fáze umisťování, pokusí se umístit kámen na daném indexu.
                return PlacePiece(index);
            }
            else if (CurrentPhase == "Moving")
            {
                if (movingPiece)
                {
                    // Pokud je fáze pohybu a hráč už vybral kámen, pokusí se přesunout kámen na daný index.
                    return MovePiece(index);
                }
                else
                {
                    // Pokud je fáze pohybu a hráč ještě nevybral kámen, pokusí se vybrat kámen na daném indexu.
                    return SelectPiece(index);
                }
            }

            return false;
        }

        // Vybere kámen pro pohyb.
        private bool PlacePiece(int index)
        {
            Position pos = Positions[index];
            // Zkontroluje, zda pozice není obsazená
            if (!pos.IsOccupied)
            {
                // Nastaví hráče, který umisťuje kámen, jako vlastníka této pozice
                pos.OccupyingPlayer = CurrentPlayer;
                pos.IsOccupied = true;

                // Sníží počet kamenů, které má aktuální hráč k dispozici k umístění
                if (CurrentPlayer == player1)
                {
                    piecesToPlacePlayer1--;
                }
                else
                {
                    piecesToPlacePlayer2--;
                }

                // Zkontroluje, zda umístěním kamene nevznikl mlýn (tři kameny v řadě)
                if (CheckMill(index))
                {
                    RemovingPiece = true; 
                }
                else
                {
                    SwitchPlayer(); // Přepne na druhého hráče, pokud mlýn nevznikl
                }

                // Zkontroluje, zda už oba hráči umístili všechny své kameny
                if (piecesToPlacePlayer1 == 0 && piecesToPlacePlayer2 == 0)
                {
                    CurrentPhase = "Moving"; // Přepne hru do fáze "pohybování kamenů"
                }

                return true; 
            }
            return false; // Pokud je pozice obsazená, nelze kámen umístit
        }


        private bool SelectPiece(int index)
        {
            Position pos = Positions[index];
            // Zkontroluje, zda je pozice obsazená a kámen patří aktuálnímu hráči
            if (pos.IsOccupied && pos.OccupyingPlayer == CurrentPlayer)
            {
                movingPiece = true; 
                selectedPieceIndex = index; // Uloží index vybraného kamene
                return true; 
            }
            return false; 
        }

        // Přesune vybraný kámen na novou pozici.
        private bool MovePiece(int index)
        {
            Position targetPos = Positions[index];
            // Zkontroluje, zda cílová pozice je neobsazená a je sousední s vybraným kamenem
            if (!targetPos.IsOccupied && IsAdjacent(selectedPieceIndex, index))
            {
                // Vyčistí původní pozici
                Positions[selectedPieceIndex].OccupyingPlayer = null;
                Positions[selectedPieceIndex].IsOccupied = false;

                // Nastaví novou pozici
                targetPos.OccupyingPlayer = CurrentPlayer;
                targetPos.IsOccupied = true;

                movingPiece = false;
                selectedPieceIndex = -1; // Vymaže index vybraného kamene

                // Zkontroluje, zda tah nevytvořil mlýn
                if (CheckMill(index))
                {
                    RemovingPiece = true; 
                }
                else
                {
                    SwitchPlayer(); 
                }
                return true; 
            }
            return false; 
        }

        private bool CheckMill(int index)
        {
            var mills = new List<int[]>
            {
                new int[] {0, 1, 2}, new int[] {2, 3, 4}, new int[] {4, 5, 6},
                new int[] {6, 7, 0}, new int[] {8, 9, 10}, new int[] {10, 11, 12},
                new int[] {12, 13, 14}, new int[] {14, 15, 8},
                new int[] {1, 9, 17}, new int[] {3, 11, 19}, new int[] {5, 13, 21}, new int[] {7, 15, 23},
                new int[] {16, 17, 18}, new int[] {18, 19, 20}, new int[] {20, 21, 22},
                new int[] {22, 23, 16}, 
                //new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
                //new int[] {9, 10, 11}, new int[] {12, 13, 14}, new int[] {15, 16, 17},
                //new int[] {18, 19, 20}, new int[] {21, 22, 23},
                //new int[] {0, 9, 21}, new int[] {3, 10, 18}, new int[] {6, 11, 15},
                //new int[] {1, 4, 7}, new int[] {16, 19, 22}, new int[] {8, 12, 17},
                //new int[] {5, 13, 20}, new int[] {2, 14, 23}
            };

            return mills.Any(mill => mill.Contains(index) && mill.All(i => Positions[i].IsOccupied && Positions[i].OccupyingPlayer == CurrentPlayer));
        }

        // Odstraní kámen z dané pozice.
        private bool RemovePiece(int index)
        {
            Position pos = Positions[index];
            // Zkontroluje, zda je pozice obsazená a kámen nepatří aktuálnímu hráči
            if (pos.IsOccupied && pos.OccupyingPlayer != CurrentPlayer)
            {
                pos.IsOccupied = false;
                pos.OccupyingPlayer = null;
                RemovingPiece = false; 

                // Sníží počet kamenů protivníka
                if (CurrentPlayer == player1)
                {
                    player2.Pieces--;
                }
                else
                {
                    player1.Pieces--;
                }

                // Zkontroluje, zda hra neskončila
                if (player1.Pieces < 3 || player2.Pieces < 3 || !CanMove(player1) || !CanMove(player2))
                {
                    CurrentPhase = "GameOver"; // Nastaví fázi hry na GameOver
                }
                else
                {
                    SwitchPlayer(); // Přepne na druhého hráče
                }

                return true; 
            }
            return false; 
        }

        private bool IsAdjacent(int index1, int index2)
        {
            int[][] adjacency = new int[][]
            {
                new int[] { 1, 7 },//0
                new int[] { 0, 2, 9 },//1
                new int[] { 1, 3 },//2
                new int[] { 2, 4, 11 },//3
                new int[] { 3, 5 },//4
                new int[] { 4, 6, 13 },//5
                new int[] { 5, 7 },//6
                new int[] { 0, 6, 15 },//7
                new int[] { 9, 15 },//8
                new int[] { 1, 8, 10, 17 },//9
                new int[] { 9, 11 },//10
                new int[] { 10, 12, 19, 3 },//11
                new int[] { 11, 13 },//12
                new int[] { 5, 12, 14, 21 },//13
                new int[] { 13, 15 },//14
                new int[] { 8, 14, 7, 23 },//15
                new int[] { 23, 17 },//16
                new int[] { 9, 16, 18 },//17
                new int[] { 17, 19 },//18
                new int[] { 11, 18, 20 },//19
                new int[] { 19, 21 },//20
                new int[] { 13, 20, 22 },//21
                new int[] { 21, 23 },//22
                new int[] { 15, 22, 16 }//23
                //new int[] { 1, 7 },
                //new int[] { 0, 2, 9 },
                //new int[] { 1, 3, 14 },
                //new int[] { 2, 4, 10 },
                //new int[] { 3, 5 },
                //new int[] { 4, 6, 13 },
                //new int[] { 5, 7 },
                //new int[] { 0, 6, 8 },
                //new int[] { 7, 9, 15 },
                //new int[] { 1, 8, 10, 17 },
                //new int[] { 3, 9, 11 },
                //new int[] { 10, 12, 19 },
                //new int[] { 11, 13 },
                //new int[] { 5, 12, 14, 20 },
                //new int[] { 2, 13, 15 },
                //new int[] { 8, 14, 16, 21 },
                //new int[] { 15, 17 },
                //new int[] { 9, 16, 18, 22 },
                //new int[] { 17, 19 },
                //new int[] { 11, 18, 20, 23 },
                //new int[] { 13, 19, 21 },
                //new int[] { 15, 20, 22 },
                //new int[] { 17, 21, 23 },
                //new int[] { 19, 22 }
            };

            return adjacency[index1].Contains(index2);
        }

        // Přepne aktuálního hráče na druhého hráče.
        private void SwitchPlayer()
        {
            // Pokud je aktuálním hráčem player1, přepne na player2, jinak na player1.
            CurrentPlayer = (CurrentPlayer == player1) ? player2 : player1;
        }


        // Kontroluje, zda hráč může provést nějaký tah.
        private bool CanMove(Player player)
        {
            // Procházení všech 24 pozic na hrací ploše.
            for (int i = 0; i < 24; i++)
            {
                // Kontrola, zda je pozice i obsazena kamenem daného hráče.
                if (Positions[i].OccupyingPlayer == player)
                {
                    // Procházení všech 24 pozic na hrací ploše pro kontrolu sousedních pozic.
                    for (int j = 0; j < 24; j++)
                    {
                        // Kontrola, zda je pozice j sousední s pozicí i a není obsazena.
                        if (IsAdjacent(i, j) && !Positions[j].IsOccupied)
                        {
                            return true;
                        }
                    }
                }
            }
            // Pokud není nalezena žádná platná sousední neobsazená pozice, hráč nemůže provést tah.
            return false;
        }



    }
