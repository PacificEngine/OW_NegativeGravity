using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PacificEngine.OW_NegativeGravity
{
    public class MainClass : ModBehaviour
    {
        enum GravityBodies
        {
            Sun,
            AshTwin,
            EmberTwin,
            TimberHearth,
            Attlerock,
            BrittleHollow,
            HollowLattern,
            GiantsDeep,
            DarkBramble,
            Interloper,
            QuantumMoon,
            EyeOfTheUniverse,
            Stranger,
            DreamWorld,
            WhiteHole
        }


        private Dictionary<GravityBodies, OWRigidbody> gravityBodies = new Dictionary<GravityBodies, OWRigidbody>();
        private const string verison = "0.1.0";
        private bool isEnabled = false;
        private float gravityMultiplier = -0.1f;
        /*private GravityVolume _activePlayerGravityVolume = null;
        private float originalAcceleration;
        private OWRigidbody _lastBody = null;*/

        void Start()
        {
            if (isEnabled)
            {
                ModHelper.Events.Player.OnPlayerAwake += (body) => onAwake();
                ModHelper.Console.WriteLine("Negative Gravity Mod ready!");
            }
        }

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;
        }

        void OnGUI()
        {
        }

        private void onAwake()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("SolarSystem");
            List<GameObject> objects = OW.Utilities.GameObjects.GameObjectUtilities.GetAllGameObjectsInScene(scene, true);
            foreach (GameObject go in objects) {
                //Locator.GetPlayerTransform().
                //Locator.GetPlayerBody().
                ModHelper.Console.WriteLine(go.tag);

                //new WarpCoreItem
            }
        }

        void Update()
        {
            if (!gravityBodies.ContainsKey(GravityBodies.Sun))
            {

                grabGravityBodies();

                foreach (GravityBodies body in gravityBodies.Keys)
                {
                    if (body == GravityBodies.Sun)
                    {

                    }
                    else if (gravityBodies[body] != null)
                    {
                        gravityBodies[body].SetMass(gravityBodies[body].GetMass() * gravityMultiplier);
                        //updateGravity(gravityBodies[body], gravityMultiplier);
                        //updateForce(gravityBodies[body], gravityMultiplier);
                    }
                }
            }
            else
            {
                if (Locator.GetAstroObject(AstroObject.Name.Sun) == null)
                {
                    gravityBodies.Clear();
                }


            }
        }

        private void updateGravity(OWRigidbody body, float multipler)
        {
            if (body && body.GetAttachedGravityVolume())
            {

                var gravity = body.GetAttachedGravityVolume();
                var originalSurfaceAcceleration = gravity.GetValue<float>("_surfaceAcceleration");
                var originalMass = gravity.GetValue<float>("_gravitationalMass");
                gravity.SetValue("_surfaceAcceleration", originalSurfaceAcceleration * multipler);
                gravity.SetValue("_gravitationalMass", originalMass * multipler);
            }
        }

        private void updateForce(OWRigidbody body, float multipler)
        {
            if (body && body.GetAttachedForceDetector())
            {
                var force = body.GetAttachedForceDetector();
                var originalField = force.GetValue<float>("_fieldMultiplier");
                var originalForceAcceleration = force.GetValue<Vector3>("_netAcceleration");

                force.SetValue("_fieldMultiplier", originalField * multipler);

                if (originalForceAcceleration != null)
                {
                    force.SetValue("_netAcceleration", new Vector3(originalForceAcceleration.x * multipler, originalForceAcceleration.y * multipler, originalForceAcceleration.z * multipler));
                }
            }
        }

        private void grabGravityBodies()
        {
            gravityBodies.Clear();
            grabGravityBody(GravityBodies.Sun, AstroObject.Name.Sun);
            grabGravityBody(GravityBodies.AshTwin, AstroObject.Name.TowerTwin);
            grabGravityBody(GravityBodies.EmberTwin, AstroObject.Name.CaveTwin);
            grabGravityBody(GravityBodies.TimberHearth, AstroObject.Name.TimberHearth);
            grabGravityBodyMoon(GravityBodies.Attlerock, AstroObject.Name.TimberHearth);
            grabGravityBody(GravityBodies.BrittleHollow, AstroObject.Name.BrittleHollow);
            grabGravityBodyMoon(GravityBodies.HollowLattern, AstroObject.Name.BrittleHollow);
            grabGravityBody(GravityBodies.GiantsDeep, AstroObject.Name.GiantsDeep);
            grabGravityBody(GravityBodies.DarkBramble, AstroObject.Name.DarkBramble);
            grabGravityBody(GravityBodies.Interloper, AstroObject.Name.Comet);
            grabGravityBody(GravityBodies.QuantumMoon, AstroObject.Name.QuantumMoon);
            grabGravityBody(GravityBodies.EyeOfTheUniverse, AstroObject.Name.Eye);
            grabGravityBody(GravityBodies.Stranger, AstroObject.Name.RingWorld);
            grabGravityBody(GravityBodies.DreamWorld, AstroObject.Name.DreamWorld);
            grabGravityBody(GravityBodies.WhiteHole, AstroObject.Name.WhiteHole);
        }

        private void grabGravityBody(GravityBodies body, AstroObject.Name name)
        {
            if (Locator.GetAstroObject(name))
            {
                gravityBodies.Add(body, Locator.GetAstroObject(name).GetAttachedOWRigidbody());
            }
        }

        private void grabGravityBodyMoon(GravityBodies body, AstroObject.Name name)
        {
            if (Locator.GetAstroObject(name) && Locator.GetAstroObject(name).GetMoon())
            {
                gravityBodies.Add(body, Locator.GetAstroObject(name).GetMoon().GetAttachedOWRigidbody());
            }
        }

        private void OnAttachPlayerToPointCustom(OWRigidbody attachBody)
        {
            ModHelper.Console.WriteLine("Negative Gravity Player Attached!");
        }

        private void OnDetachPlayerFromPointCustom()
        {
            ModHelper.Console.WriteLine("Negative Gravity Player Detached!");
        }
    }
}
