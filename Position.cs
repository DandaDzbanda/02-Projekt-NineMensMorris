public class Position
    {
        //Pozice je obsazená
        public bool IsOccupied { get; set; }

        // Hráč, který obsazuje tuto pozici na desce.
        public Player OccupyingPlayer { get; set; }

        public Position()
        {
            // Nastavení počáteční pozice jako neobsazenou.
            IsOccupied = false;
        }
    }
