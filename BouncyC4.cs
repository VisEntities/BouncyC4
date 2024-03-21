using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Bouncy C4", "VisEntities", "1.1.0")]
    [Description("Throw non-sticking bouncy explosives.")]
    public class BouncyC4 : RustPlugin
    {
        #region Fields

        private static BouncyC4 _plugin;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
            PermissionUtil.RegisterPermissions();
        }

        private void Unload()
        {
            _plugin = null;
        }

        private object CanExplosiveStick(TimedExplosive explosive, BaseEntity entity)
        {
            BasePlayer entityOwner = FindPlayerById(entity.OwnerID);
            if (entityOwner != null && PermissionUtil.VerifyHasPermission(entityOwner, PermissionUtil.USE))
            {
                Rigidbody rb = explosive.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = explosive.gameObject.AddComponent<Rigidbody>();

                Collider collider = explosive.gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.material = new PhysicMaterial
                    {
                        bounciness = 1.0f,
                        bounceCombine = PhysicMaterialCombine.Maximum
                    };
                }

                // Slow down the rotation over time.
                rb.angularDrag = 0.05f;
                // Random spin in all directions.
                rb.angularVelocity = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));

                return false;
            }

            return null;
        }

        #endregion Oxide Hooks

        #region Helper Functions

        public static BasePlayer FindPlayerById(ulong playerId)
        {
            return RelationshipManager.FindByID(playerId);
        }

        #endregion Helper Functions

        #region Utility Classes

        private static class PermissionUtil
        {
            public const string USE = "bouncyc4.use";

            public static void RegisterPermissions()
            {
                _plugin.permission.RegisterPermission(USE, _plugin);
            }

            public static bool VerifyHasPermission(BasePlayer player, string permissionName = USE)
            {
                return _plugin.permission.UserHasPermission(player.UserIDString, permissionName);
            }
        }

        #endregion Utility Classes
    }
}