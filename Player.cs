public class Player
    {
        public string Name { get; private set; }
        public Brush Color { get; private set; }
        public int Pieces { get; set; }

        public Player(string name, Brush color)
        {
            Name = name;
            Color = color;
            Pieces = 9;
        }
    }
