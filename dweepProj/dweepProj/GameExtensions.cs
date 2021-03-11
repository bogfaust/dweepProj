namespace Game
{
    public static class GameExtensions
    {
        public static void startInventoryInit()
        {
            GameData.startInventory[(int)Item.SlashMirror] = 0;
            GameData.startInventory[(int)Item.BackSlashMirror] = 0;
            GameData.startInventory[(int)Item.RotatorClockwise] = 0;
            GameData.startInventory[(int)Item.RotatorCounterClockwise] = 0;
            GameData.startInventory[(int)Item.Bomb] = 0;
            GameData.startInventory[(int)Item.Fire] = 0;
        }
        public static void resetInventory()
        {
            for (int i = 0; i < (int)Item.ItemsCount; i++)
            {
                GameData.inventory[i] = GameData.startInventory[i];
            }
        }

        public static Item GetItemByMode(PlacingMode mode)
        {
            if ((int)mode != (int)(Item)mode) return Item.UnknownItem;

            return (Item)mode;
        }

        public static Entities GetEntByItem(Item item)
        {
            switch (item)
            {
                case Item.SlashMirror:
                    return Entities.SlashMirror;

                case Item.BackSlashMirror:
                    return Entities.BackSlashMirror;

                case Item.Bomb:
                    return Entities.Bomb;
            }
            
            return Entities.UnknownEnt;
        }

        public static Item GetItemByEnt(Entities ent)
        {
            switch (ent)
            {
                case Entities.PickupSlashMirror:
                case Entities.SlashMirror:
                    return Item.SlashMirror;

                case Entities.PickupBackSlashMirror:
                case Entities.BackSlashMirror:
                    return Item.BackSlashMirror;

                case Entities.PickupRotatorClockwise:
                    return Item.RotatorClockwise;

                case Entities.PickupRotatorCounterClockwise:
                    return Item.RotatorCounterClockwise;

                case Entities.PickupBomb:
                case Entities.Bomb:
                    return Item.Bomb;

                case Entities.PickupFire:
                    return Item.Fire;
            }

            return Item.UnknownItem;
        }

        public static string GetPrefixByItem(Item item)
        {
            switch (item)
            {
                case Item.SlashMirror:
                    return "/";

                case Item.BackSlashMirror:
                    return "\\";

                case Item.RotatorClockwise:
                    return "-->";

                case Item.RotatorCounterClockwise:
                    return "<--";

                case Item.Bomb:
                    return "Bomb";

                case Item.Fire:
                    return "Fire";
            }

            return "ERROR";
        }

        public static string GetPrefixByEnt(Entities ent)
        {
            switch (ent)
            {
                case Entities.PickupSlashMirror:
                    return "/";

                case Entities.PickupBackSlashMirror:
                    return "\\";

                case Entities.PickupRotatorClockwise:
                    return "-->";

                case Entities.PickupRotatorCounterClockwise:
                    return "<--";

                case Entities.PickupBomb:
                    return "Bomb";

                case Entities.PickupFire:
                    return "Fire";
            }
            return null;
        }
        public static bool IsEntWalkable(int i, int j)
        {
            foreach (Entities element in EntGroups.Walkable)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Через сущность можно пройти

            return false;
        }
        public static bool IsEntRotatable(int i, int j)
        {
            foreach (Entities element in EntGroups.Rotatable)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Сущность можно вращать

            return false;
        }

        public static bool IsEntFireable(int i, int j)
        {
            foreach (Entities element in EntGroups.Fireable)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Сущность можно поджечь

            return false;
        }
        public static bool IsEntPickable(int i, int j)
        {
            foreach (Entities element in EntGroups.Pickable)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Сущность можно положить в инвентарь

            return false;
        }

        public static bool IsEntBombable(int i, int j)
        {
            foreach (Entities element in EntGroups.Bombable)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Сущность можно подорвать бомбой

            return false;
        }

        public static bool IsCellInbound(int i, int j)
        {
            if (
                i >= 0 && i < Constants.fieldHeight &&            // Граница поля прекращает лазер
                j >= 0 && j < Constants.fieldWidth                // Граница поля прекращает лазер
            )
                return true;

            return false;
        }

        public static bool IsEntTurret(int i, int j)
        {
            foreach (Entities element in EntGroups.Turrets)
                if ((Entities)GameData.field[i, j] == element)
                    return true; // Сущность можно подорвать бомбой

            return false;
        }
    }
}
