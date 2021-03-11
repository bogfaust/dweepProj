using System.Collections.Generic;

namespace Game
{
    public enum Entities
    {
        EmptyCell = 0,           // Пустое место    
        Wall = 1,                // Стена    
                                 
        Laser = 3,               // Лазер
                                 
        Turret2 = 2,             // Турели
        Turret4 = 4,             
        Turret6 = 6,             
        Turret8 = 8,             
                                 
        SlashMirror = 5,         // Зеркало Slash
        BackSlashMirror = 7,     // Зеркало BackSlash
                                 
        AliveHero = 9,           // Игрок
        DeadHero = -9,           // Труп игрока

        Bomb = 10,               // Бомба
        TickingBomb = 11,        // Подожжёная Бомба

        PickupSlashMirror = 101,                // Предмет Slash зеркало
        PickupBackSlashMirror = 102,            // Предмет BackSlash зеркало

        PickupRotatorClockwise = 103,           // Предмет крутилка по часовой стрелке
        PickupRotatorCounterClockwise = 104,    // Предмет крутилка против часовой стрели

        PickupBomb = 105,                       // Предмет бомба
        PickupFire = 106,                       // Предмет факел

        UnknownEnt = 9000
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
        Default = -1,

        // BEGIN. Должно строго совпадать с одноимёнными элементами Item
        SlashMirror = 0,
        BackSlashMirror = 1,

        RotatorClockwise = 2,
        RotatorCounterClockwise = 3,

        Bomb = 4,
        Fire = 5
        // END. Должно строго совпадать с одноимёнными элементами Item
    }

    public enum Item
    {
        // BEGIN. Должно строго совпадать с одноимёнными элементами PlacingMode
        SlashMirror = 0,
        BackSlashMirror = 1,

        RotatorClockwise = 2,
        RotatorCounterClockwise = 3,

        Bomb = 4,
        Fire = 5,
        // END. Должно строго совпадать с одноимёнными элементами PlacingMode

        ItemsCount = 6,

        UnknownItem = 7
    }



    class GameData
    {
        public const int iStartPos = 4; // Стартовая позииция игрока по вертикали
        public const int jStartPos = 7; // Стартовая позииция игрока по горизонтали

        // public const int iStartPos = 2; // Стартовая позииция игрока по вертикали
        // public const int jStartPos = 0; // Стартовая позииция игрока по горизонтали

        public const int fieldHeight = 10;  // высота поля
        public const int fieldWidth = 16;   // ширина поля

        public const int bombFireTicksMs = 2000; // Сколько милисекунд бомба тикает до взрыва
        public const int bombExplodeTicksMs = 500; // Сколько милисекунд бомба тикает до взрыва

        public static int restartsCount = 0; // Чтобы отменять взрыв бомб, если был рестарт
        public static int restartsCountPre = 0; // Чтобы отменять взрыв бомб, если был рестарт

        public static int iPlayerPos = iStartPos; // Текущая координата игрока по высоте
        public static int jPlayerPos = jStartPos; // Текущая координата игрока по ширине

        public static int[,] field = new int[fieldHeight, fieldWidth]; // Поле для игры. Состояние клеток меняется
        public static int[,] laserMap = new int[fieldHeight, fieldWidth]; // Поле для игры. Состояние клеток меняется

        public static List<int> iBoomQueue = new List<int>();
        public static List<int> jBoomQueue = new List<int>();

        public static bool isPlayerDead = false; // Мертв ли игрок
        public static PlacingMode Mode = PlacingMode.Default;

        public static int[] startInventory = new int[(int)Item.ItemsCount];
        public static int[] inventory = new int[(int)Item.ItemsCount]; // Поле для игры. Состояние клеток меняется
    }

    class EntGroups
    {
        public static Entities[] Walkable =
        {
            Entities.EmptyCell,
            Entities.Laser,
            Entities.PickupSlashMirror, Entities.PickupBackSlashMirror,
            Entities.PickupRotatorClockwise,Entities.PickupRotatorCounterClockwise,
            Entities.PickupBomb,
            Entities.PickupFire
        };

        public static Entities[] Rotatable =
        {
            Entities.Turret2, Entities.Turret4, Entities.Turret6, Entities.Turret8,
            Entities.SlashMirror, Entities.BackSlashMirror
        };

        public static Entities[] Fireable =
        {
            Entities.AliveHero,
            Entities.Bomb
        };

        public static Entities[] Pickable =
        {
            Entities.PickupSlashMirror,
            Entities.PickupBackSlashMirror,
            Entities.PickupRotatorClockwise,
            Entities.PickupRotatorCounterClockwise,
            Entities.PickupBomb,
            Entities.PickupFire
        };

        public static Entities[] Bombable =
        {
            Entities.AliveHero,
            Entities.Turret2, Entities.Turret4, Entities.Turret6, Entities.Turret8,
            Entities.SlashMirror, Entities.BackSlashMirror,
            Entities.Bomb,
        };

        public static Entities[] Turrets =
        {
            Entities.Turret2, Entities.Turret4, Entities.Turret6, Entities.Turret8,
        };
    }
}
