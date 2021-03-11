using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Game;

class Constants
{
    public const int windowHeight = 1200; // Размер окна по высоте
    public const int windowWidth = 800; // Размер окна по ширине

    public const int fieldHeight = 10; // Размер поля по высоте
    public const int fieldWidth = 16; // Размер поля по высоте

    //public static int[,] levelMap = new int[,] {
    //    { 0, 0, 2, 0, 104, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
    //    { 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 1, 0, 1, 6, 0, 0, 0, 1, 1, 0, 0, 4 },
    //    { 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
    //    { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0 },
    //    { 0, 1, 1, 1, 1, 1, 1, 0, 2, 1, 0, 1, 1, 1, 1, 0 },
    //    { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 6, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
    //    { 0, 0, 0, 8, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }
    //};

    public static int[,] levelMap = new int[GameData.fieldHeight, GameData.fieldWidth] {
        { 0,   0, 0, 0, 0, 0, 0,   0,   0,   0, 0, 0,  0,   1, 1,  1 },
        { 6,   0, 0, 7, 0, 0, 0,   0,   0,   0, 0, 0,  0,   1, 0,  1 },
        { 1,   1, 0, 0, 1, 1, 0,   0,   0,   1, 1, 0,  0,   1, 0,  1 },
        { 0,   0, 0, 0, 1, 0, 0,   0,   0,   0, 1, 10, 1,   1, 10, 1 },
        { 106, 0, 0, 0, 0, 0, 0,   0,   0,   0, 1, 10, 106, 1, 0,  1 },
        { 0,   0, 0, 0, 1, 0, 0,   0,   0,   0, 0, 10, 1,   1, 0,  0 },
        { 0,   0, 0, 0, 1, 0, 104, 102, 105, 0, 1, 0,  0,   1, 1,  10},
        { 0,   0, 0, 0, 1, 1, 0,   0,   0,   1, 1, 0,  0,   0, 0,  0 },
        { 0,   0, 0, 0, 0, 0, 0,   0,   0,   0, 0, 0,  0,   0, 1,  1 },
        { 0,   8, 0, 0, 0, 0, 0,   0,   0,   0, 0, 7,  0,   0,10,  103 }
    };

    public static int[,] directions = new int[,] {
    //   i2 j2      // i1 == 0, j1 == 0
        { 0, 0},
        { 1,-1},    // Num1
        { 1, 0},    // Num2
        { 1, 1},    // Num3
        { 0,-1},    // Num4
        { 0, 0},    // Num5
        { 0, 1},    // Num6
        {-1,-1},    // Num7
        {-1, 0},    // Num8
        {-1, 1},    // Num9
    };

    public const int buttonSize = 40;           // Размер кнопки
    public const int stepSize = 2 * buttonSize; // Отступ для следующей кнопки
    public const int margin = 5;                // Отступ для следующей кнопки

    public const int panelSize = Constants.buttonSize - 2 * Constants.margin; // Отступ для следующей кнопки

    // Размер панели
    public static readonly Size button2dSize = new Size(
        Constants.buttonSize - 2 * Constants.margin,
        Constants.buttonSize - 2 * Constants.margin
    );
}

namespace GraphicsObject_c
{
    /// <summary>
    /// Summary description for GraphicsObject.
    /// </summary>
    public class GraphicsObject : Form
    {
        private Button BShowField;
        private Button BClearField;

        private static Button BSlashMirror;
        private static Button BBackSlashMirror;
                 
        private static Button BRotatorClockwise;
        private static Button BRotatorCounterClockwise;

        private static Button BBomb;
        private static Button BFire;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        Panel[,] panels = new Panel[Constants.fieldHeight, Constants.fieldWidth];

        public GraphicsObject()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.KeyPreview = true; // форма получит все события клавиш
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown); // обработчик нажатий клавиш

            this.MouseClick += new MouseEventHandler(Control1_MouseClick); // Обработчик нажатий клавиши мыши

            this.ShowLaser();
        }
        private void Control1_MouseClick(Object sender, MouseEventArgs e)
        {
            //MessageBox.Show(
            //    $"e.X = {e.X} \n" +
            //    $"e.Y = {e.Y} \n"
            //    );

            int i = 0;
            int j = 0;

            j = e.X / Constants.buttonSize;
            i = e.Y / Constants.buttonSize;

            clickCell(i, j, sender);
        }

        public void RestartGame()
        {
            // Восстановление клеток по умолчанию
            for (int i = 0; i < Constants.fieldHeight; i++)
                for (int j = 0; j < Constants.fieldWidth; j++)
                {
                    GameData.field[i, j] = Constants.levelMap[i, j];
                    GameData.laserMap[i, j] = Constants.levelMap[i, j];
                }

            // Задаём координаты игрока по умолчанию 
            GameData.iPlayerPos = GameData.iStartPos;
            GameData.jPlayerPos = GameData.jStartPos;
            GameData.field[GameData.iPlayerPos, GameData.jPlayerPos] = (int)Entities.AliveHero; // Размещаем игрока на поле
            GameData.isPlayerDead = false; // Игрок снова жив

            // Восстанавливаем инвентарь по умолчанию
            GameExtensions.resetInventory();

            BSlashMirror.Text = ConstructButtonNameByItem(Item.SlashMirror);
            BBackSlashMirror.Text = ConstructButtonNameByItem(Item.BackSlashMirror);
            BRotatorClockwise.Text = ConstructButtonNameByItem(Item.RotatorClockwise);
            BRotatorCounterClockwise.Text = ConstructButtonNameByItem(Item.RotatorCounterClockwise);
            BBomb.Text = ConstructButtonNameByItem(Item.Bomb);
            BFire.Text = ConstructButtonNameByItem(Item.Fire);

            GameData.restartsCount++;

            ProcessAndShowField();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int keyNum = -1; // Какая кнопка была нажата, 1..9 - нампад кнопки, -1 - НЕнампадная кнопка
            int iMove = 0; // передвижение игрока по i
            int jMove = 0; // передвижение игрока по j

            // NumPad5 делает рестарт игры
            if (e.KeyCode == Keys.NumPad5)
            {
                keyNum = 5;
                RestartGame();
                return;
            }

            // Если игрок умер, то никакие кнопки не работают (кроме 5)
            if (GameData.isPlayerDead == true)
            {
                MessageBox.Show("Игрок умер");
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                    keyNum = 1;
                    iMove = (int)Moving.Bottom;
                    jMove = (int)Moving.Left;
                    break;

                case Keys.NumPad2:
                    keyNum = 2;
                    iMove = (int)Moving.Bottom;
                    jMove = (int)Moving.StandStill;
                    break;

                case Keys.NumPad3:
                    keyNum = 3;
                    iMove = (int)Moving.Bottom;
                    jMove = (int)Moving.Right;
                    break;

                case Keys.NumPad4:
                    keyNum = 4;
                    iMove = (int)Moving.StandStill;
                    jMove = (int)Moving.Left;
                    break;

                case Keys.NumPad6:
                    keyNum = 6;
                    iMove = (int)Moving.StandStill;
                    jMove = (int)Moving.Right;
                    break;

                case Keys.NumPad7:
                    keyNum = 7;
                    iMove = (int)Moving.Top;
                    jMove = (int)Moving.Left;
                    break;

                case Keys.NumPad8:
                    keyNum = 8;
                    iMove = (int)Moving.Top;
                    jMove = (int)Moving.StandStill;
                    break;

                case Keys.NumPad9:
                    keyNum = 9;
                    iMove = (int)Moving.Top;
                    jMove = (int)Moving.Right;
                    break;
            }

            if (keyNum == -1) return; // Нажатая кнопка не является NumPad кнопкой

            ProcessNextFrame(keyNum, iMove, jMove);
        }

        // Обрабатываем поле после нажатия кнопки передвижения
        public void ProcessNextFrame(int keyNum, int iMove, int jMove)
        {
            // // Игрок не перемещается
            if (iMove == 0 && jMove == 0) return;

            if(
                GameData.iPlayerPos + iMove < 0 || (GameData.iPlayerPos + iMove > GameData.fieldHeight - 1) ||
                GameData.jPlayerPos + jMove < 0 || (GameData.jPlayerPos + jMove > GameData.fieldWidth - 1)
            )
            {
                MessageBox.Show("Граница карты");

                // Отрисовывать фрейм не нужно, так как перемещения не было
                return;
            }

            if(!GameExtensions.IsEntWalkable(GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove))
            {
                MessageBox.Show("Упёрлись в объект");

                // Отрисовывать фрейм не нужно, так как перемещения не было
                return;
            }

            // Не даём переместиться, если игрок мёртв
            if (GameData.isPlayerDead) return;

            // // Перемещение игрока
            GameData.field[GameData.iPlayerPos, GameData.jPlayerPos] = (int)Entities.EmptyCell; // Убираем игрока с клетки

            if (GameExtensions.IsEntPickable(GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove))
            {
                Entities ent = (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove];

                Item item = GameExtensions.GetItemByEnt(ent);
                GameData.inventory[(int)item]++; // Добавляем предмет в инвентарь
                GetButtonByEnt(ent).Text = ConstructButtonNameByItem(item); // Обновляем число на соотв. кнопке
            }

            if ((Entities)GameData.laserMap[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Laser)
                killHero(GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove); // Игрок переместился и умер

            // Изменяем координаты игрока
            GameData.iPlayerPos += iMove;
            GameData.jPlayerPos += jMove;

            GameData.field[GameData.iPlayerPos, GameData.jPlayerPos] = (int)Entities.AliveHero; // Ставим игрока в клетку

            ProcessAndShowField();
        }

        public static Button GetButtonByEnt(Entities ent)
        {
            switch (ent)
            {
                case Entities.PickupSlashMirror:
                case Entities.SlashMirror:
                    return BSlashMirror;

                case Entities.PickupBackSlashMirror:
                case Entities.BackSlashMirror:
                    return BBackSlashMirror;

                case Entities.PickupRotatorClockwise:
                    return BRotatorClockwise;

                case Entities.PickupRotatorCounterClockwise:
                    return BRotatorCounterClockwise;

                case Entities.PickupBomb:
                case Entities.Bomb:
                    return BBomb;

                case Entities.PickupFire:
                    return BFire;
            }

            return null;
        }

        public static Button GetButtonByItem(Item item)
        {
            switch (item)
            {
                case Item.SlashMirror:
                    return BSlashMirror;

                case Item.BackSlashMirror:
                    return BBackSlashMirror;

                case Item.RotatorClockwise:
                    return BRotatorClockwise;

                case Item.RotatorCounterClockwise:
                    return BRotatorCounterClockwise;

                case Item.Bomb:
                    return BBomb;

                case Item.Fire:
                    return BFire;
            }

            return null;
        }

        // Просчитываем движение лазера и отрисовываем
        public static void ProcessLaser(int i, int j, Entities turretType, GraphicsObject gObj)
        {
            int iLaser = i; // Текущая клетка, до которой дошел лазер
            int jLaser = j;

            int iMove = (int)Moving.StandStill; // В какую сторону будет двигаться лазер
            int jMove = (int)Moving.StandStill;

            switch (turretType)
            {
                case Entities.Turret2:
                    iMove = (int)Moving.Bottom; // Вниз
                    break;

                case Entities.Turret4:
                    jMove = (int)Moving.Left;   // Влево
                    break;

                case Entities.Turret6:
                    jMove = (int)Moving.Right;  // Вправо
                    break;

                case Entities.Turret8:
                    iMove = (int)Moving.Top;    // Вверх
                    break;
            }

            bool inMuzzle = true;

            // Пройденный лазером путь 
            int[,] laserWayPoints = new int[GameData.field.GetLength(0) * GameData.field.GetLength(1), 2];
            int laserWayLen = 0; // Длина пройденного лазером пути

            bool laserSourceDestroyed = false;

            int directFrom = 0; // Откуда пришел лазер
            int iLaserFrom = 0;
            int jLaserFrom = 0;

            while (
                GameExtensions.IsCellInbound(iLaser, jLaser) &&
                GameData.field[iLaser, jLaser] != (int)Entities.Wall     // Стена прекращает лазер
            )
            {
                // Вычисляем в какую сторону пойдёт лазер
                if (GameData.field[iLaser, jLaser] == (int)Entities.SlashMirror) // зеркало Slash
                {
                    // Луч сверху вниз \/
                    //                  /
                    if ((Moving)iMove == Moving.Bottom)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Left;
                        directFrom = (int)Moving.FromRight;
                    }
                    //                  /
                    // Луч снизу вверх  /\
                    else if ((Moving)iMove == Moving.Top)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Right;
                        directFrom = (int)Moving.FromLeft;
                    }
                    // Луч справа налево /<--
                    else if ((Moving)jMove == Moving.Left)
                    {
                        iMove = (int)Moving.Bottom;
                        jMove = (int)Moving.StandStill;
                        directFrom = (int)Moving.FromTop;
                    }
                    // Луч слева направо -->/
                    else if ((Moving)jMove == Moving.Right)
                    {
                        iMove = (int)Moving.Top;
                        jMove = (int)Moving.StandStill;
                        directFrom = (int)Moving.FromBottom;
                    }
                }

                else if (GameData.field[iLaser, jLaser] == (int)Entities.BackSlashMirror) // зеркало BackSlash
                {
                    // Луч сверху вниз \/
                    //                 \
                    if ((Moving)iMove == Moving.Bottom)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Right;
                        directFrom = (int)Moving.FromLeft;
                    }
                    //                   \
                    // Луч снизу вверх  /\
                    else if ((Moving)iMove == Moving.Top)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Left;
                        directFrom = (int)Moving.FromRight;
                    }
                    // Луч справа налево \<--
                    else if ((Moving)jMove == Moving.Left)
                    {
                        iMove = (int)Moving.Top;
                        jMove = (int)Moving.StandStill;
                        directFrom = (int)Moving.FromBottom;
                    }
                    // Луч слева направо -->\
                    else if ((Moving)jMove == Moving.Right)
                    {
                        iMove = (int)Moving.Bottom;
                        jMove = (int)Moving.StandStill;
                        directFrom = (int)Moving.FromTop;
                    }
                }
                else
                {   // Читаемость или размер кода не улучшатся, если переделать в switch
                    if ((Moving)iMove == Moving.Bottom) directFrom = (int)Moving.FromTop;
                    else if ((Moving)iMove == Moving.Top) directFrom = (int)Moving.FromBottom;
                    else if ((Moving)jMove == Moving.Left) directFrom = (int)Moving.FromRight;
                    else if ((Moving)jMove == Moving.Right) directFrom = (int)Moving.FromLeft;
                }

                // Получение направления на основе номера турели
                if (iLaserFrom < 0) iLaserFrom = 0;
                else if  (iLaserFrom > GameData.fieldHeight - 1) iLaserFrom = GameData.fieldHeight - 1;

                if (jLaserFrom < 0) jLaserFrom = 0;
                else if (jLaserFrom > GameData.fieldWidth - 1) jLaserFrom = GameData.fieldWidth - 1;

                try
                {
                    // Проверяем есть ли в клетке какая-либо турель
                    bool isTurret = false;
                    foreach (Entities turret in EntGroups.Turrets)
                        if ((Entities)GameData.field[iLaser, jLaser] == turret)
                        {
                            isTurret = true;
                            break; // выход из foreach
                        }

                    for (int temp = 0; temp < 1; temp++) // Чтобы не создавать функцию
                    {
                        if (!isTurret) break;

                        // // На пути лазера попалась турель Х

                        // Случай когда лазер только вышел из турели X. directFrom должен совпадать с позицией турели X
                        // С турелью ничего не происходит
                        if (laserWayLen == 0)
                        {
                            iLaserFrom = iLaser;
                            jLaserFrom = jLaser;
                            break; // Выход из for
                        }

                        if (inMuzzle) break; // Выход из for

                        // Разрушение турелей лазером
                        // Случилось ли лобовое столкновение лазеров из разных турелей
                        switch ((Entities)GameData.field[iLaser, jLaser])
                        {
                            case Entities.Turret2:
                                if ((Moving)directFrom == Moving.FromBottom) laserSourceDestroyed = true;
                                break;

                            case Entities.Turret4:
                                if ((Moving)directFrom == Moving.FromLeft) laserSourceDestroyed = true;
                                break;

                            case Entities.Turret6:
                                if ((Moving)directFrom == Moving.FromRight) laserSourceDestroyed = true;
                                break;

                            case Entities.Turret8:
                                if ((Moving)directFrom == Moving.FromTop) laserSourceDestroyed = true;
                                break;
                        }

                        // Лазер уничтожил турель, из которой исходил
                        if (iLaser == i && jLaser == j)
                        {
                            laserSourceDestroyed = true;
                            throw new Exception("Turret");
                        }

                        // Одна турель уничтожила другую (либо обе друг друга одновременно)
                        GameData.field[iLaser, jLaser] = (int)Entities.Laser;

                        // Записываем в пройденный путь текущую клетку
                        laserWayPoints[laserWayLen, 0] = iLaser;
                        laserWayPoints[laserWayLen, 1] = jLaser;
                    }
                }

                // Случай, когда турель источник была уничтожена
                catch (Exception ex)
                {
                    break; // Выход из while
                }

                if (!GameExtensions.IsEntTurret(iLaser, jLaser)) // На сами турели лазер ставить не нужно
                {
                    if ((Entities)GameData.field[iLaser, jLaser] == Entities.Bomb) // Бомбу поджигаем
                        BombStartTicking(iLaser, jLaser, gObj, GameData.bombFireTicksMs);

                    // Если на пути лазера встретился игрок, то убиваем игрока
                    if ((Entities)GameData.field[iLaser, jLaser] == Entities.AliveHero)
                        killHero(iLaser, jLaser);

                    else GameData.laserMap[iLaser, jLaser] = (int)Entities.Laser; // Поставили лазер
                }

                // Записываем в пройденный путь текущую клетку
                laserWayPoints[laserWayLen, 0] = iLaser;
                laserWayPoints[laserWayLen, 1] = jLaser;
                
                laserWayLen++;

                // Лазер пошел в следующую клетку
                iLaser += iMove;
                jLaser += jMove;

                inMuzzle = false; // Лазер вышел за пределы турели-источника
            }

            if (!laserSourceDestroyed) return;

            // Удаление лучей лазера, если источник лазера был уничтожен
            for (int wayCell = 0; wayCell < laserWayPoints.GetLength(0); wayCell++)
            {
                if ((Entities)GameData.laserMap[laserWayPoints[wayCell, 0], laserWayPoints[wayCell, 1]] == Entities.Laser)
                    GameData.field[laserWayPoints[wayCell, 0],laserWayPoints[wayCell, 1]] = (int)Entities.EmptyCell;
            }

            GameData.field[i, j] = (int)Entities.EmptyCell;
        }

        public static void PlaceItem(PlacingMode mode, int i, int j)
        {
            Item item = GameExtensions.GetItemByMode(mode);

            if (item == Item.UnknownItem) return;

            Entities ent = GameExtensions.GetEntByItem(item);

            GameData.field[i, j] = (int)ent; // Ставим сущность в клетку

            GameData.inventory[(int)mode]--; // Уменьшаем количество предметов этого типа в инвентаре

            // Меняем текст на кнопке
            GetButtonByEnt(ent).Text = ConstructButtonNameByItem(item);
        }

        public static void Rotate(PlacingMode mode, int i, int j)
        {
            Item item = GameExtensions.GetItemByMode(mode);

            if (item == Item.UnknownItem) return;

            switch((Entities)GameData.field[i, j])
            {
                case Entities.Turret2:
                    GameData.field[i, j] = mode == PlacingMode.RotatorClockwise ? (int)Entities.Turret4 : (int)Entities.Turret6;
                    break;
                case Entities.Turret4:
                    GameData.field[i, j] = mode == PlacingMode.RotatorClockwise ? (int)Entities.Turret8 : (int)Entities.Turret2;
                    break;
                case Entities.Turret6:
                    GameData.field[i, j] = mode == PlacingMode.RotatorClockwise ? (int)Entities.Turret2 : (int)Entities.Turret8;
                    break;
                case Entities.Turret8:
                    GameData.field[i, j] = mode == PlacingMode.RotatorClockwise ? (int)Entities.Turret6 : (int)Entities.Turret4;
                    break;
                case Entities.SlashMirror:
                case Entities.BackSlashMirror:
                    GameData.field[i, j] = (Entities)GameData.field[i, j] == Entities.SlashMirror ? (int)Entities.BackSlashMirror : (int)Entities.SlashMirror;
                    break;
            }

            GameData.inventory[(int)mode]--; // Уменьшаем количество предметов этого типа в инвентаре

            // Меняем текст на кнопке
            GetButtonByItem(item).Text = ConstructButtonNameByItem(item);
        }

        public static void BombStartTicking(int i, int j, GraphicsObject gObj, int ticksMs)
        {
            System.Threading.Timer timer;

            GameData.iBoomQueue.Add(i); // Добавляем координаты бомбы в очередь на взрыв
            GameData.jBoomQueue.Add(j);

            GameData.field[i, j] = (int)Entities.TickingBomb;

            GameData.restartsCountPre = GameData.restartsCount;

            timer = new System.Threading.Timer(
                callback: new TimerCallback(gObj.BombExplode),
                state: null,
                dueTime: ticksMs,
                period: Timeout.Infinite);
        }

        public void BombExplode(object obj)
        {
            if (GameData.restartsCountPre != GameData.restartsCount) return; // Чтобы бомбы не взрывались после рестарта

            int i = GameData.iBoomQueue[0]; // Где взорвалась бомба
            int j = GameData.jBoomQueue[0];

            // Смотрим что вокруг бомбы
            for (int dir = 1; dir < Constants.directions.GetLength(0); dir++)
            {
                int iEnt = i + Constants.directions[dir, 0]; // Координаты соседней сущности
                int jEnt = j + Constants.directions[dir, 1];

                if (!GameExtensions.IsCellInbound(iEnt, jEnt)) continue; // За границей карты смотреть не нужно

                Entities ent = (Entities)GameData.field[iEnt, jEnt];

                // Смотрим что подорвалось
                if (GameExtensions.IsEntBombable(iEnt, jEnt))
                {
                    switch (ent)
                    {
                        case Entities.AliveHero:
                            killHero(iEnt, jEnt);
                            break;

                        case Entities.Turret2:
                        case Entities.Turret4:
                        case Entities.Turret6:
                        case Entities.Turret8:
                        case Entities.SlashMirror:
                        case Entities.BackSlashMirror:
                            GameData.field[iEnt, jEnt] = (int)Entities.EmptyCell;
                            break;

                        case Entities.Bomb:
                            BombStartTicking(iEnt, jEnt, this, GameData.bombExplodeTicksMs);
                            break;
                    }
                }
            }

            GameData.field[i, j] = (int)Entities.EmptyCell; // После взрыва бомбы клетка стала пустой

            GameData.iBoomQueue.RemoveAt(0); // Удаляем координаты бомбы из очереди на подрыв
            GameData.jBoomQueue.RemoveAt(0);

            ProcessAndShowField();
        }

        public static void FireUp(PlacingMode mode, int i, int j, GraphicsObject gObj)
        {
            Item item = GameExtensions.GetItemByMode(mode); // Получаем какой предмет в инвентаре соотв. моду

            if (item == Item.UnknownItem) return;

            switch ((Entities)GameData.field[i, j])
            {
                case Entities.AliveHero:
                    killHero(i, j);
                    break;

                case Entities.Bomb:
                    BombStartTicking(i, j, gObj, GameData.bombFireTicksMs); // Бомба взорвется через bombFireTicksMs мс
                    break;
            }

            GameData.inventory[(int)mode]--; // Уменьшаем количество предметов этого типа в инвентаре
            
            GetButtonByItem(item).Text = ConstructButtonNameByItem(item); // Меняем текст на кнопке
        }

        public static void killHero(int i, int j)
        {
            GameData.field[i, j] = (int)Entities.DeadHero;
            GameData.isPlayerDead = true;
        }

        public void clickCell(int i, int j, Object gObj)
        {
            // Не выбран Мод или закончились предметы
            if (GameData.Mode == PlacingMode.Default) return;

            // Смотрим сколько единиц предмета есть
            Item item = GameExtensions.GetItemByMode(GameData.Mode);
            int amount = GameData.inventory[(int)item];

            if (amount <= 0) return; // Предметов этого типа нет в инвентаре

            switch (GameData.Mode)
            {
                case PlacingMode.SlashMirror:
                case PlacingMode.BackSlashMirror:
                case PlacingMode.Bomb:
                    if ((Entities)GameData.field[i, j] != Entities.EmptyCell) return; // В клетке уже что-то есть

                    // Клетка пустая. Размещаем предмет на поле
                    PlaceItem(GameData.Mode, i, j);

                    break;

                case PlacingMode.RotatorClockwise:
                case PlacingMode.RotatorCounterClockwise:
                    if (!GameExtensions.IsEntRotatable(i, j)) return; // Сущность не вращается

                    Rotate(GameData.Mode, i, j); // Вращаем сущность
                    break;

                case PlacingMode.Fire:
                    if (!GameExtensions.IsEntFireable(i, j)) return;

                    FireUp(GameData.Mode, i, j, this); // Поджигаем сущность
                    break;
            }

            if (amount == 0)  // Предмет этого типа закончился
                GameData.Mode = PlacingMode.Default;

            this.ProcessAndShowField();
        }

        public static string ConstructButtonNameByItem(Item item)
        {
            return
                GameExtensions.GetPrefixByItem(item) + " " + "\n" +
                "(" + GameData.inventory[(int)item].ToString() + ")";
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BShowField = new Button();
            BClearField = new Button();
            BSlashMirror = new Button();
            BBackSlashMirror = new Button();

            BRotatorClockwise = new Button();
            BRotatorCounterClockwise = new Button();
            
            BBomb = new Button();
            BFire = new Button();

            this.SuspendLayout();

            // Создание стартового инвентаря
            GameExtensions.startInventoryInit();

            // Заполнение текущего инвентаря
            GameExtensions.resetInventory();

            const int buttonSizeX = 100;
            const int buttonSizeY = 100;
            const int extraSpaceX = 100; // Дополнительное место для отступа от другой кнопки

            // 
            // BShowField (Показать поле в численном виде)
            // 
            this.BShowField.Location = new Point(Constants.fieldWidth * Constants.buttonSize + extraSpaceX, 0);
            this.BShowField.Name = "BShowField";
            this.BShowField.Size = new Size(3 * buttonSizeX, 3 * buttonSizeY);
            this.BShowField.TabIndex = 3;
            this.BShowField.Text = "Show field";
            this.BShowField.Click += new EventHandler(this.BShowField_Click);
            // 
            // BClearField (Обнулить поле)
            // 
            this.BClearField.Location = new Point(Constants.fieldWidth * Constants.buttonSize + extraSpaceX, 3 * buttonSizeY);
            this.BClearField.Name = "BClearField";
            this.BClearField.Size = new Size(buttonSizeX, buttonSizeY);
            this.BClearField.TabIndex = 3;
            this.BClearField.Text = "Restart";
            this.BClearField.Click += new EventHandler(this.BClearField_Click);

            int k = 0;
            // 
            // BSlashMirror (Кнопка установки Slash зеркала)
            // 
            BSlashMirror.Location = new Point(k * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BSlashMirror.Name = "BSlashMirror";
            BSlashMirror.Size = new Size(buttonSizeX, buttonSizeY);
            BSlashMirror.TabIndex = 3;
            //this.BSlashMirror.Text = "/";
            BSlashMirror.Text = ConstructButtonNameByItem(Item.SlashMirror);
            BSlashMirror.Click += new EventHandler(this.BSlashMirror_Click);

            k++;
            // 
            // BBackSlashMirror (Кнопка установки BackSlash зеркала)
            // 
            BBackSlashMirror.Location = new Point(1 * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BBackSlashMirror.Name = "BBackSlashMirror";
            BBackSlashMirror.Size = new Size(buttonSizeX, buttonSizeY);
            BBackSlashMirror.TabIndex = 3;
            //this.BBackSlashMirror.Text = "\\";
            BBackSlashMirror.Text = ConstructButtonNameByItem(Item.BackSlashMirror);
            BBackSlashMirror.Click += new EventHandler(this.BBackSlashMirror_Click);

            k++;
            // 
            // BRotatorClockwise ()
            // 
            BRotatorClockwise.Location = new Point(k * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BRotatorClockwise.Name = "BRotatorClockwise";
            BRotatorClockwise.Size = new Size(buttonSizeX, buttonSizeY);
            BRotatorClockwise.TabIndex = 3;
            BRotatorClockwise.Text = ConstructButtonNameByItem(Item.RotatorClockwise);
            BRotatorClockwise.Click += new EventHandler(this.BRotatorClockwise_Click);

            k++;
            // 
            // BRotatorCounterClockwise ()
            // 
            BRotatorCounterClockwise.Location = new Point(k * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BRotatorCounterClockwise.Name = "BRotatorCounterClockwise";
            BRotatorCounterClockwise.Size = new Size(buttonSizeX, buttonSizeY);
            BRotatorCounterClockwise.TabIndex = 3;
            BRotatorCounterClockwise.Text = ConstructButtonNameByItem(Item.RotatorCounterClockwise);
            BRotatorCounterClockwise.Click += new EventHandler(this.BRotatorCounterClockwise_Click);

            k++;
            // 
            // BBomb (Кнопка установки Бомбы)
            // 
            BBomb.Location = new Point(k * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BBomb.Name = "BBomb";
            BBomb.Size = new Size(buttonSizeX, buttonSizeY);
            BBomb.TabIndex = 3;
            BBomb.Text = ConstructButtonNameByItem(Item.Bomb);
            BBomb.Click += new EventHandler(this.BBomb_Click);

            k++;
            // 
            // BFire (Кнопка установки Бомбы)
            // 
            BFire.Location = new Point(k * buttonSizeX, Constants.fieldHeight * Constants.buttonSize);
            BFire.Name = "BFire";
            BFire.Size = new Size(buttonSizeX, buttonSizeY);
            BFire.TabIndex = 3;
            BFire.Text = ConstructButtonNameByItem(Item.Fire);
            BFire.Click += new EventHandler(this.BFire_Click);

            // 
            // GraphicsObject
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(Constants.windowHeight, Constants.windowWidth);
            this.Controls.AddRange(new Control[] {
                this.BShowField,
                this.BClearField,
                BSlashMirror,
                BBackSlashMirror,
                BRotatorClockwise,
                BRotatorCounterClockwise,
                BBomb,
                BFire,
            });
            this.Name = "GraphicsObject";
            this.Text = "GraphicsObject";
            this.Load += new EventHandler(this.GraphicsObject_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new GraphicsObject());
        }

        private void GraphicsObject_Load(object sender, System.EventArgs e)
        {

        }

        public void ProcessAndShowField()
        {
            // Заливка дефолтным цветом, чтобы убрать всё, что было нарисовано
            Graphics G;
            G = this.CreateGraphics();
            G.Clear(Control.DefaultBackColor);

            // Вертикальные линии поля
            for (int i = 0; i < Constants.fieldWidth + 1; i++)
                G.DrawLine(
                    new Pen(Color.DarkMagenta, 2),
                    i * Constants.buttonSize, 0, i * Constants.buttonSize,
                    Constants.fieldHeight * Constants.buttonSize
            );

            // Горизонтальные линии поля
            for (int j = 0; j < Constants.fieldHeight + 1; j++)
                G.DrawLine(
                    new Pen(Color.DarkMagenta, 2),
                    0, j * Constants.buttonSize,
                    Constants.fieldWidth * Constants.buttonSize, j * Constants.buttonSize
            );

            // Удаление лучей лазера для дальнейшей перерисовки
            for (int i = 0; i < Constants.fieldHeight; i++)
                for (int j = 0; j < Constants.fieldWidth; j++)
                    if ((Entities)GameData.laserMap[i, j] == Entities.Laser) GameData.laserMap[i, j] = (int)Entities.EmptyCell;

            // Проходим лазером из каждой Турели Х для зеркал типа Slash и BackSlash
            for (int i = 0; i < Constants.fieldHeight; i++)
                for (int j = 0; j < Constants.fieldWidth; j++)
                    foreach (Entities turretX in EntGroups.Turrets)
                        if ((Entities)GameData.field[i, j] == turretX)
                            ProcessLaser(i, j, turretX, this);

            // Отрисовка всех клеток игры
            for (int i = 0; i < Constants.fieldHeight; i++)
                for (int j = 0; j < Constants.fieldWidth; j++)
                    switch ((Entities)GameData.field[i, j])
                    {
                        case Entities.Wall:
                            G.FillRectangle(
                                new SolidBrush(Color.Black),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );
                            break;

                        case Entities.Turret2:
                        case Entities.Turret4:
                        case Entities.Turret6:
                        case Entities.Turret8:
                            // Получение направления на основе номера турели
                            int i2coef = Constants.directions[GameData.field[i, j], 0]; // i
                            int j2coef = Constants.directions[GameData.field[i, j], 1]; // j

                            // Рисуем турель
                            G.DrawEllipse(
                                new Pen(Color.Black, 3),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                    Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin
                                )
                            );

                            // Рисуем дуло турели
                            G.DrawLine(
                                new Pen(Color.Green, 10),
                                j * Constants.buttonSize + 4 * Constants.margin, i * Constants.buttonSize + 4 * Constants.margin,
                                (j + j2coef) * Constants.buttonSize + 4 * Constants.margin, (i + i2coef) * Constants.buttonSize + 4 * Constants.margin
                            );
                            break;

                        case Entities.SlashMirror:
                            // Рисуем SlashMirror
                            G.DrawLine(
                                new Pen(Color.Blue, 4),
                                j * Constants.buttonSize, (i + 1) * Constants.buttonSize,
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize
                            );
                            break;

                        case Entities.BackSlashMirror:
                            // Рисуем BackSlashMirror
                            G.DrawLine(
                                new Pen(Color.Blue, 4),
                                j * Constants.buttonSize, i * Constants.buttonSize,
                                (j + 1) * Constants.buttonSize, (i + 1) * Constants.buttonSize
                            );
                            break;

                        case Entities.AliveHero:
                            // Рисуем Игрока
                            G.FillRectangle(
                                new SolidBrush(Color.Magenta),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );
                            break;

                        case Entities.DeadHero:
                            // Рисуем труп Игрока
                            G.DrawLine(
                                new Pen(Color.Magenta, 8),
                                j * Constants.buttonSize, i * Constants.buttonSize,
                                (j + 1) * Constants.buttonSize, (i + 1) * Constants.buttonSize
                            );
                            G.DrawLine(
                                new Pen(Color.Magenta, 8),
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize,
                                j * Constants.buttonSize, (i + 1) * Constants.buttonSize
                            );
                            break;

                        case Entities.Bomb:
                            // Рисуем бомбу
                            G.FillEllipse(
                                new SolidBrush(Color.DarkSlateGray),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                    Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin
                                )
                            );
                            break;

                        case Entities.TickingBomb:
                            // Рисуем бомбу
                            G.FillEllipse(
                                new SolidBrush(Color.Yellow),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                    Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin
                                )
                            );
                            break;

                        case Entities.PickupSlashMirror:
                            // Рисуем лут Slash зеркала
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );

                            G.DrawLine(
                                new Pen(Color.Blue, 4),
                                j * Constants.buttonSize + Constants.margin, (i + 1) * Constants.buttonSize - Constants.margin,
                                (j + 1) * Constants.buttonSize - Constants.margin, i * Constants.buttonSize + Constants.margin
                            );
                            break;

                        case Entities.PickupBackSlashMirror:
                            // Рисуем лут BackSlash зеркала
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );

                            G.DrawLine(
                                new Pen(Color.Blue, 4),
                                j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                (j + 1) * Constants.buttonSize - Constants.margin, (i + 1) * Constants.buttonSize - Constants.margin
                            );
                            break;

                        case Entities.PickupRotatorClockwise:
                            // Рисуем лут RotatorClockwise
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );
                            // Горизонтальная линия
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                j * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2
                            );
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                j * Constants.buttonSize + Constants.buttonSize / 2, i * Constants.buttonSize
                            );
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                j * Constants.buttonSize + Constants.buttonSize / 2, (i + 1) * Constants.buttonSize
                            );
                            break;

                        case Entities.PickupRotatorCounterClockwise:
                            // Рисуем лут RotatorCounterClockwise
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );
                            // Горизонтальная линия
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                j * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                (j + 1) * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2
                            );
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                j * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                j * Constants.buttonSize + Constants.buttonSize / 2, i * Constants.buttonSize
                            );
                            G.DrawLine(
                                new Pen(Color.DarkViolet, 4),
                                j * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                                j * Constants.buttonSize + Constants.buttonSize / 2, (i + 1) * Constants.buttonSize
                            );
                            break;

                        case Entities.PickupBomb:
                            // Рисуем лут бомбы
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );

                            G.FillEllipse(
                                new SolidBrush(Color.DarkSlateGray),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                    Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin
                                )
                            );
                            break;

                        case Entities.PickupFire:
                            // Рисуем лут огня
                            G.FillRectangle(
                                new SolidBrush(Color.SkyBlue),
                                new Rectangle(
                                    j * Constants.buttonSize + Constants.margin,
                                    i * Constants.buttonSize + Constants.margin,
                                    Constants.panelSize,
                                    Constants.panelSize
                                )
                            );

                            G.DrawLine(
                                new Pen(Color.Yellow, 4),
                                j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                (j + 1) * Constants.buttonSize - Constants.margin, (i + 1) * Constants.buttonSize - Constants.margin
                            );
                            break;
                    }

            this.ShowLaser();
        }

        // Проверка, что поле заполняется правильно
        private void BShowField_Click(object sender, System.EventArgs e)
        {
            Button b = (Button)sender;
            b.Text = "Show field\n";

            for (int i = 0; i < Constants.fieldHeight; i++)
            {
                for (int j = 0; j < Constants.fieldWidth; j++)
                    b.Text = b.Text + GameData.field[i, j].ToString() + "  ";

                b.Text += "\n";
            }
        }

        // Рестарт игры 
        private void BClearField_Click(object sender, System.EventArgs e)
        {
            RestartGame();

            ProcessAndShowField();
        }

        // Установка Мода Slash зеркала
        private void BSlashMirror_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.SlashMirror] > 0) GameData.Mode = PlacingMode.SlashMirror;
        }

        // Установка Мода BackSlash зеркала
        private void BBackSlashMirror_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.BackSlashMirror] > 0) GameData.Mode = PlacingMode.BackSlashMirror;
        }

        // Установка Мода RotatorClockwise
        private void BRotatorClockwise_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.RotatorClockwise] > 0) GameData.Mode = PlacingMode.RotatorClockwise;
        }

        // Установка Мода RotatorCounterClockwise
        private void BRotatorCounterClockwise_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.RotatorCounterClockwise] > 0) GameData.Mode = PlacingMode.RotatorCounterClockwise;
        }

        // Установка Мода Bomb
        private void BBomb_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.Bomb] > 0) GameData.Mode = PlacingMode.Bomb;
        }

        // Установка Мода Fire
        private void BFire_Click(object sender, System.EventArgs e)
        {
            if (GameData.inventory[(int)Item.Fire] > 0) GameData.Mode = PlacingMode.Fire;
        }

        private void ShowLaser()
        {
            for (int i = 0; i < Constants.fieldHeight; i++)
            {
                for (int j = 0; j < Constants.fieldWidth; j++)
                {
                    if ((Entities)GameData.laserMap[i, j] != Entities.Laser) continue; // Переходим к следующей панели

                    using (Graphics G = this.CreateGraphics())
                    {
                        // |
                        G.DrawLine(
                            new Pen(Color.Green, 4),
                            j * Constants.buttonSize + Constants.buttonSize/2, i * Constants.buttonSize,
                            j * Constants.buttonSize + Constants.buttonSize/2, (i + 1) * Constants.buttonSize
                        );
                        // --
                        G.DrawLine(
                            new Pen(Color.Green, 4),
                            j * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2,
                            (j + 1) * Constants.buttonSize, i * Constants.buttonSize + Constants.buttonSize / 2
                        );
                    }
                }
            }
        }
    }
}