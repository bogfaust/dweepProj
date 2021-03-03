using System;
using System.Drawing;
using System.Windows.Forms;
using Game;

class Constants
{
    public const int windowHeight = 1200; // Размер окна по высоте
    public const int windowWidth = 800; // Размер окна по ширине

    public const int fieldHeight = 10; // Размер поля по высоте
    public const int fieldWidth = 16; // Размер поля по высоте

    public static int[,] walls = new int[,] {
        { 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
        { 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 1, 0, 1, 6, 0, 0, 0, 1, 1, 0, 0, 4 },
        { 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
        { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0 },
        { 0, 1, 1, 1, 1, 1, 1, 0, 2, 1, 0, 1, 1, 1, 1, 0 },
        { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 6, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 8, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }
    };

    public static int[,] directions = new int[,] {
    //   i2 j2      // i1 == 0, j1 == 0
        { 0, 0},
        { 0, 0},
        { 1, 0},    // 2
        { 0, 0},
        { 0,-1},    // 4
        { 0, 0},
        { 0, 1},    // 6
        { 0, 0},
        {-1, 0}     // 8
    };

    public const int iStartPost = 2; // Стартовая позииция игрока по вертикали
    public const int jStartPost = 0; // Стартовая позииция игрока по горизонтали

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
        private Button BSlashMirror;
        private Button BBackSlashMirror;
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

            this.CreatePanels();
        }

        public void RestartGame()
        {
            // Восстановление клеток по умолчанию
            for (int i = 0; i < Constants.fieldHeight; i++)
                for (int j = 0; j < Constants.fieldWidth; j++)
                    GameData.field[i, j] = Constants.walls[i, j];

            // Задаём координаты игрока по умолчанию 
            GameData.iPlayerPos = Constants.iStartPost;
            GameData.jPlayerPos = Constants.jStartPost;
            GameData.field[GameData.iPlayerPos, GameData.jPlayerPos] = (int)Entities.AliveHero; // Размещаем игрока на поле
            GameData.isPlayerDead = false; // Игрок снова жив
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
                ProcessNextFrame(keyNum, (int)Moving.StandStill, (int)Moving.StandStill);
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
            // Игрок не перемещается
            if (
                GameData.iPlayerPos + iMove < 0 || (GameData.iPlayerPos + iMove > GameData.field.GetLength(0) - 1) ||
                GameData.jPlayerPos + jMove < 0 || (GameData.jPlayerPos + jMove > GameData.field.GetLength(1) - 1)
            )
            {
                MessageBox.Show("Граница карты");

                // Отрисовывать фрейм не нужно, так как перемещения не было
                return;
            }

            // Игрок не перемещается
            if (
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Wall ||   // Стена
               
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Turret2 ||   // Турель 2
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Turret4 ||   // Турель 4
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Turret6 ||   // Турель 6
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Turret8 ||   // Турель 8
                
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.SlashMirror ||   // Зеркало Slash
                (Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.BackSlashMirror      // Зеркало BackSlash
            )
            {
                MessageBox.Show("Упёрлись в объект");

                // Отрисовывать фрейм не нужно, так как перемещения не было
                return;
            }

            // // Перемещение игрока
            GameData.field[GameData.iPlayerPos, GameData.jPlayerPos] = (int)Entities.EmptyCell; // Убираем игрока с клетки

            // Если клетка свободна, то перемещаем игрока в неё
            if ((Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.EmptyCell) 
                GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] = (int)Entities.AliveHero; 

            else if ((Entities)GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] == Entities.Laser)
            {
                // Игрок переместился и умер
                GameData.field[GameData.iPlayerPos + iMove, GameData.jPlayerPos + jMove] = (int)Entities.DeadHero;
                GameData.isPlayerDead = true;
            }

            // Изменяем координаты игрока
            GameData.iPlayerPos += iMove;
            GameData.jPlayerPos += jMove;

            ProcessAndShowField(); // Отрисовываем фрейм
        }

        // Просчитываем движение лазера и отрисовываем
        public static void ProcessLaser(int i, int j, Entities turretType, Entities mirrorType) //GraphicsObject graphicsObject)
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

            int directFrom = 0; // Откуда пришел лазер

            while (
                iLaser >= 0 && iLaser < Constants.fieldHeight &&         // Граница поля прекращает лазер
                jLaser >= 0 && jLaser < Constants.fieldWidth &&          // Граница поля прекращает лазер
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
                    }
                    //                  /
                    // Луч снизу вверх  /\
                    else if ((Moving)iMove == Moving.Top)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Right;
                    }
                    // Луч справа налево /<--
                    else if ((Moving)jMove == Moving.Left)
                    {
                        iMove = (int)Moving.Bottom;
                        jMove = (int)Moving.StandStill;
                    }
                    // Луч слева направо -->/
                    else if ((Moving)jMove == Moving.Right)
                    {
                        iMove = (int)Moving.Top;
                        jMove = (int)Moving.StandStill;
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
                    }
                    //                   \
                    // Луч снизу вверх  /\
                    else if ((Moving)iMove == Moving.Top)
                    {
                        iMove = (int)Moving.StandStill;
                        jMove = (int)Moving.Left;
                    }
                    // Луч справа налево \<--
                    else if ((Moving)jMove == Moving.Left)
                    {
                        iMove = (int)Moving.Top;
                        jMove = (int)Moving.StandStill;
                    }
                    // Луч слева направо -->\
                    else if ((Moving)jMove == Moving.Right)
                    {
                        iMove = (int)Moving.Bottom;
                        jMove = (int)Moving.StandStill;
                    }
                }
                else
                {   // Читаемость или размер кода не улучшатся, если переделать в switch
                    if ((Moving)iMove == Moving.Bottom) directFrom = (int)Moving.FromTop;
                    else if ((Moving)iMove == Moving.Top) directFrom = (int)Moving.FromBottom;
                    else if ((Moving)jMove == Moving.Left) directFrom = (int)Moving.FromRight;
                    else if ((Moving)jMove == Moving.Right) directFrom = (int)Moving.FromLeft;
                }

                int iLaserFrom = 0;
                int jLaserFrom = 0;

                // Получение направления на основе номера турели
                iLaserFrom = iLaser + Constants.directions[directFrom, 0]; // i
                jLaserFrom = jLaser + Constants.directions[directFrom, 1]; // j

                // Если мы рассматриваем турель X, а не луч лазера, то directFrom должен совпадать с позицией турели X
                if (
                    (Entities)GameData.field[iLaser, jLaser] == Entities.Turret2 ||
                    (Entities)GameData.field[iLaser, jLaser] == Entities.Turret4 ||
                    (Entities)GameData.field[iLaser, jLaser] == Entities.Turret6 ||
                    (Entities)GameData.field[iLaser, jLaser] == Entities.Turret8
                )
                {
                    iLaserFrom = iLaser;
                    jLaserFrom = jLaser;
                }

                // Проверяем можно ли в клетку ставить лазер
                if (
                        (Entities)GameData.field[iLaser, jLaser] != Entities.Wall &&// Ставим лазер, если нет стены
                        (
                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == Entities.Turret2 || // Ставим лазер, если откуда пришли турель типа 2
                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == Entities.Turret4 || // Ставим лазер, если откуда пришли турель типа 4
                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == Entities.Turret6 || // Ставим лазер, если откуда пришли турель типа 6
                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == Entities.Turret8 || // Ставим лазер, если откуда пришли турель типа 8

                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == Entities.Laser ||   // Ставим лазер, если откуда пришли тоже лазер
                            (Entities)GameData.field[iLaserFrom, jLaserFrom] == mirrorType                    // Ставим лазер, если откуда пришли зеркало с лазером
                        )
                        &&
                        (Entities)GameData.field[iLaser, jLaser] != Entities.DeadHero &&        // Cтавим лазер, если на этой клетке НЕ лежит труп игрока
                        (Entities)GameData.field[iLaser, jLaser] != Entities.SlashMirror &&     // Ставим лазер, если НЕ стоит зеркало Slash
                        (Entities)GameData.field[iLaser, jLaser] != Entities.BackSlashMirror    // Ставим лазер, если НЕ стоит зеркало BackSlash
                )
                {
                    if (
                        (Entities)GameData.field[iLaser, jLaser] != Entities.Turret2 &&     // На сами турели лазер ставить не нужно
                        (Entities)GameData.field[iLaser, jLaser] != Entities.Turret4 &&
                        (Entities)GameData.field[iLaser, jLaser] != Entities.Turret6 &&
                        (Entities)GameData.field[iLaser, jLaser] != Entities.Turret8
                    )

                        // Если на пути лазера встретился игрок, то убиваем игрока
                        if ((Entities)GameData.field[iLaser, jLaser] == Entities.AliveHero)
                        {
                            GameData.isPlayerDead = true;
                            GameData.field[iLaser, jLaser] = (int)Entities.DeadHero;
                        }
                        else GameData.field[iLaser, jLaser] = (int)Entities.Laser; // Поставили лазер
                }

                // Лазер пошел в следующую клетку
                iLaser += iMove;
                jLaser += jMove;
            }
        }

        public static void PlaceMirror(PlacingMode Mode, Entities mirrorType, int i, int j, Graphics G)
        {
            int i1 = i, i2 = i;
            int j1 = j, j2 = j;

            if (Mode == PlacingMode.SlashMirror)
            {
                i1++;
                j2++;
            }
            else if (Mode == PlacingMode.BackSlashMirror)
            {
                i2++;
                j2++;
            }

            G.DrawLine(
                new Pen(Color.Blue, 4),
                j1 * Constants.buttonSize, i1 * Constants.buttonSize,
                j2 * Constants.buttonSize, i2 * Constants.buttonSize
            );

            GameData.field[i, j] = (int)mirrorType;
        }

        private class PanelClicker
        {
            private readonly int _i;
            private readonly int _j;
            private readonly GraphicsObject _graphicsObject;

            public PanelClicker(int i, int j, GraphicsObject graphicsObject)
            {
                _i = i;
                _j = j;
                _graphicsObject = graphicsObject;
            }

            public void panel_Click()
            {
                // Не выбран Мод или закончились зеркала
                if (GameData.Mode == PlacingMode.Default) return;

                _graphicsObject.panels[_i, _j].Dispose();

                // Отрисовка зеркала
                using (Graphics G = _graphicsObject.CreateGraphics())
                {
                    // Slash зеркало
                    if (GameData.Mode == PlacingMode.SlashMirror && GameData.SlashMirrorsAmount > 0)
                    {
                        PlaceMirror(PlacingMode.SlashMirror, Entities.SlashMirror, _i, _j, G);

                        GameData.SlashMirrorsAmount--;

                        if (GameData.SlashMirrorsAmount == 0)  // Зеркала закончились
                            GameData.Mode = PlacingMode.Default;
                    }

                    // BackSlash зеркало
                    else if (GameData.Mode == PlacingMode.BackSlashMirror && GameData.BackSlashMirrorsAmount > 0)
                    {
                        PlaceMirror(PlacingMode.BackSlashMirror, Entities.BackSlashMirror, _i, _j, G);

                        GameData.BackSlashMirrorsAmount--;

                        if (GameData.BackSlashMirrorsAmount == 0)  // Зеркала закончились
                            GameData.Mode = PlacingMode.Default;
                    }

                    // Из-за зеркала направление лазера могло поменяться
                    // Пересчитаем пути лазеров на этой строчке и столбце
                    _graphicsObject.ProcessAndShowField();
                }
            }
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
            this.BShowField = new Button();
            this.BClearField = new Button();
            this.BSlashMirror = new Button();
            this.BBackSlashMirror = new Button();

            this.SuspendLayout();

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
            // 
            // BSlashMirror (Показать поле в численном виде)
            // 
            this.BSlashMirror.Location = new Point(Constants.fieldWidth * Constants.buttonSize + extraSpaceX, 4 * buttonSizeY);
            this.BSlashMirror.Name = "BSlashMirror";
            this.BSlashMirror.Size = new Size(buttonSizeX, buttonSizeY);
            this.BSlashMirror.TabIndex = 3;
            this.BSlashMirror.Text = "/";
            this.BSlashMirror.Click += new EventHandler(this.BSlashMirror_Click);
            // 
            // BBackSlashMirror (Показать поле в численном виде)
            // 
            this.BBackSlashMirror.Location = new Point(Constants.fieldWidth * Constants.buttonSize + extraSpaceX, 5 * buttonSizeY);
            this.BBackSlashMirror.Name = "BBackSlashMirror";
            this.BBackSlashMirror.Size = new Size(buttonSizeX, buttonSizeY);
            this.BBackSlashMirror.TabIndex = 3;
            this.BBackSlashMirror.Text = "\\";
            this.BBackSlashMirror.Click += new EventHandler(this.BBackSlashMirror_Click);


            // 
            // GraphicsObject
            // 
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(Constants.windowHeight, Constants.windowWidth);
            this.Controls.AddRange(new Control[] {
                this.BShowField,
                this.BClearField,
                this.BSlashMirror,
                this.BBackSlashMirror
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

            this.DisposePanels();

            using (Graphics G2 = this.CreateGraphics())
            {
                // Вертикальные линии поля
                for (int i = 0; i < Constants.fieldWidth + 1; i++)
                    G2.DrawLine(
                        new Pen(Color.DarkMagenta, 2),
                        i * Constants.buttonSize, 0, i * Constants.buttonSize,
                        Constants.fieldHeight * Constants.buttonSize
                );

                // Горизонтальные линии поля
                for (int j = 0; j < Constants.fieldHeight + 1; j++)
                    G2.DrawLine(
                        new Pen(Color.DarkMagenta, 2),
                        0, j * Constants.buttonSize,
                        Constants.fieldWidth * Constants.buttonSize, j * Constants.buttonSize
                );

                // Удаление лучей лазера для дальнейшей перерисовки
                for (int i = 0; i < Constants.fieldHeight; i++)
                    for (int j = 0; j < Constants.fieldWidth; j++)
                        if ((Entities)GameData.field[i, j] == Entities.Laser) GameData.field[i, j] = (int)Entities.EmptyCell;

                // Проходим лазером из каждой Турели Х для зеркал типа 5 и 7
                for (int i = 0; i < Constants.fieldHeight; i++)
                    for (int j = 0; j < Constants.fieldWidth; j++)
                        foreach (Entities turretX in EntGroups.Turrets)
                            if ((Entities)GameData.field[i, j] == turretX)
                            {
                                ProcessLaser(i, j, turretX, Entities.SlashMirror);
                                ProcessLaser(i, j, turretX, Entities.BackSlashMirror);
                            }

                // Отрисовка всех клеток игры
                for (int i = 0; i < Constants.fieldHeight; i++)
                    for (int j = 0; j < Constants.fieldWidth; j++)
                        switch ((Entities)GameData.field[i, j])
                        {
                            case Entities.Wall:
                                SolidBrush wallBrush = new SolidBrush(Color.Black);
                                G2.FillRectangle(
                                    wallBrush, new Rectangle(
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

                                // // Рисуем турель
                                // Создаём ручку
                                Pen blackPen = new Pen(Color.Black, 3);

                                // Создание прямоугольника для элипса
                                Rectangle rect = new Rectangle(
                                    j * Constants.buttonSize + Constants.margin, i * Constants.buttonSize + Constants.margin,
                                    Constants.buttonSize - 2 * Constants.margin, Constants.buttonSize - 2 * Constants.margin
                                );

                                // Рисуем элипс
                                G2.DrawEllipse(blackPen, rect);

                                // // Рисуем дуло турели
                                // Рисуем линию
                                G2.DrawLine(
                                    new Pen(Color.Green, 10),
                                    j * Constants.buttonSize + 4 * Constants.margin, i * Constants.buttonSize + 4 * Constants.margin,
                                    (j + j2coef) * Constants.buttonSize + 4 * Constants.margin, (i + i2coef) * Constants.buttonSize + 4 * Constants.margin
                                );
                                break;

                            case Entities.SlashMirror:
                                G.DrawLine(
                                    new Pen(Color.Blue, 4),
                                    j * Constants.buttonSize, (i + 1) * Constants.buttonSize,
                                    (j + 1) * Constants.buttonSize, i * Constants.buttonSize
                                );
                                break;

                            case Entities.BackSlashMirror:
                                G.DrawLine(
                                    new Pen(Color.Blue, 4),
                                    j * Constants.buttonSize, i * Constants.buttonSize,
                                    (j + 1) * Constants.buttonSize, (i + 1) * Constants.buttonSize
                                );
                                break;

                            case Entities.AliveHero:
                                SolidBrush heroBrush = new SolidBrush(Color.Magenta);
                                G2.FillRectangle(
                                    heroBrush, new Rectangle(
                                        j * Constants.buttonSize + Constants.margin,
                                        i * Constants.buttonSize + Constants.margin,
                                        Constants.panelSize,
                                        Constants.panelSize
                                    )
                                );
                                break;

                            case Entities.DeadHero:
                                G2.DrawLine(
                                    new Pen(Color.Magenta, 4),
                                    j * Constants.buttonSize, i * Constants.buttonSize,
                                    (j + 1) * Constants.buttonSize, (i + 1) * Constants.buttonSize
                                );
                                G2.DrawLine(
                                    new Pen(Color.Magenta, 4),
                                    (j + 1) * Constants.buttonSize, i * Constants.buttonSize,
                                    j * Constants.buttonSize, (i + 1) * Constants.buttonSize
                                );
                                break;
                        }

            }

            this.CreatePanels();
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
            // Заливка дефолтным цветом, чтобы убрать нарисованную линию
            Graphics G;
            G = this.CreateGraphics();
            G.Clear(Control.DefaultBackColor);

            this.DisposePanels();
            //this.CreatePanels();

            RestartGame();

            ProcessAndShowField();
        }

        // Установка Мода Slash зеркала
        private void BSlashMirror_Click(object sender, System.EventArgs e)
        {
            if (GameData.SlashMirrorsAmount > 0) GameData.Mode = PlacingMode.SlashMirror;
        }

        // Установка Мода BackSlash зеркала
        private void BBackSlashMirror_Click(object sender, System.EventArgs e)
        {
            if (GameData.BackSlashMirrorsAmount > 0) GameData.Mode = PlacingMode.BackSlashMirror;
        }
        private void CreatePanels()
        {

            for (int i = 0; i < Constants.fieldHeight; i++)
            {
                for (int j = 0; j < Constants.fieldWidth; j++)
                {
                    bool doContinue = false; // true - перейти к следующей панели, false - продолжить работу с панелью

                    // Не создавать панель в указанных типах клеток
                    foreach (Entities element in EntGroups.noPanel) if ((Entities)GameData.field[i, j] == element)
                    {
                        doContinue = true;
                        break;
                    }

                    if (doContinue) continue; // Переходим к следующей панели

                    // Обрабатываем панель
                    panels[i, j] = new Panel();
                    panels[i, j].Location =
                        new Point(
                            j * Constants.buttonSize + Constants.margin, // Horizontal
                            i * Constants.buttonSize + Constants.margin); // Vertical

                    panels[i, j].Size = Constants.button2dSize; // Размер панели

                    // Раскраска панелей с лазером
                    if ((Entities)GameData.field[i, j] == Entities.Laser) panels[i, j].BackColor = Color.Green;

                    var clicker = new PanelClicker(i, j, this);

                    panels[i, j].Click += new EventHandler((object o, EventArgs e) => {
                        clicker.panel_Click();
                    });

                    this.Controls.Add(this.panels[i, j]);
                }
            }
        }

        private void DisposePanels()
        {
            for (int i = 0; i < Constants.fieldHeight; i++)
            {
                for (int j = 0; j < Constants.fieldWidth; j++)
                {
                    if (panels[i, j] != null) panels[i, j].Dispose();
                }
            }
        }
    }
}