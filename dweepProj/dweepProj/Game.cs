namespace Game
{
    public enum Entities
    {
        EmptyCell = 0,       // Пустое место    
        Wall = 1,            // Стена    
                             
        Laser = 3,           // Лазер
                             
        Turret2 = 2,         // Турели
        Turret4 = 4,         
        Turret6 = 6,         
        Turret8 = 8,         
                             
        SlashMirror = 5,     // Зеркало Slash
        BackSlashMirror = 7, // Зеркало BackSlash
                             
        AliveHero = 9,       // Игрок
        DeadHero = -9        // Труп игрока
    }

    public enum Moving
    {
        StandStill = 0, // Не двигаться

        Left = -1,    
        Right = 1,    

        Top = -1,     
        Bottom = 1,

        FromLeft = 4,
        FromRight = 6,

        FromTop = 8,
        FromBottom = 2
    }

    public enum PlacingMode
    {
        Default = 0,
        SlashMirror = 1,
        BackSlashMirror = 2
    }

    class GameData
    {
        public static int iPlayerPos = Constants.iStartPost; // Текущая координата игрока по высоте
        public static int jPlayerPos = Constants.jStartPost; // Текущая координата игрока по ширине

        public static int[,] field = new int[10, 16]; // Поле для игры. Состояние клеток меняется

        public static bool isPlayerDead = false; // Мертв ли игрок
        public static PlacingMode Mode = PlacingMode.Default; // 0 - Default, 1 - Slash зеркало, 2 - BackSlash зеркало 
        public static int SlashMirrorsAmount = 3; // Доступное количество зеркал типа Slash
        public static int BackSlashMirrorsAmount = 3; // Доступное количество зеркал типа BackSlash
    }

    class EntGroups
    {
        public static Entities[] noPanel =
        {
            Entities.Wall,
            Entities.Turret2, Entities.Turret4, Entities.Turret6, Entities.Turret8,
            Entities.SlashMirror, Entities.BackSlashMirror,
            Entities.AliveHero, Entities.DeadHero
        };

        public static Entities[] Turrets =
        {
            Entities.Turret2, Entities.Turret4, Entities.Turret6, Entities.Turret8,
        };
    }
}
