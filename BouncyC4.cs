namespace Oxide.Plugins
{
    [Info("Bouncy C4", "Dana", "1.0.0")]
    [Description("Toss bouncy non-sticking explosives.")]
    public class BouncyC4 : RustPlugin
    {
        #region Fields

        private static BouncyC4 _instance;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _instance = this;
            PermissionUtils.Register();
        }

        private void Unload()
        {
            _instance = null;
        }

        private object CanExplosiveStick(TimedExplosive explosive, BaseEntity entity)
        {
            BasePlayer entityOwner = FindPlayerById(entity.OwnerID);
            if (entityOwner != null && PermissionUtils.Verify(entityOwner))
                return false;

            return null;
        }

        #endregion Oxide Hooks

        #region Helper Classes

        private static class PermissionUtils
        {
            public const string USE = "bouncyc4.use";

            public static void Register()
            {
                _instance.permission.RegisterPermission(USE, _instance);
            }

            public static bool Verify(BasePlayer player, string permissionName = USE)
            {
                return _instance.permission.UserHasPermission(player.UserIDString, permissionName);
            }
        }

        #endregion Helper Classes

        #region Helper Functions

        private BasePlayer FindPlayerById(ulong playerId)
        {
            return RelationshipManager.FindByID(playerId);
        }

        #endregion Helper Functions
    }
}