using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public class PortalInstance : MonoBehaviour
    {
        private string ownerUID;
        private bool isFirstPortal;
        private float lifeTime = -1f;
        private float currentTime;
        private bool canTeleport = true;
        private float teleportCooldown = 0.5f;
        private float cooldownTimer = 0f;

        private bool canTeleportAI = false;
        private bool canTeleportPlayers = true;
        private bool canTeleportProjectiles = true;

        private Camera portalCamera;
        private RenderTexture renderTexture;
        private Material portalMaterial;
        private ParticleSystem portalParticles;
        private ParticleSystemRenderer particleRenderer;

        private const int RENDER_TEXTURE_SIZE = 512;
        private const int RENDER_TEXTURE_DEPTH = 24;
        private const string PORTAL_RENDER_PATH = "Visual/RenderPortal";
        private const float TELEPORT_OFFSET = 1.5f;

        private const float TELEPORT_OFFSET_PLAYER = 2f;
        private const float TELEPORT_OFFSET_AI = 1.5f;
        private const float TELEPORT_OFFSET_PROJECTILE = 1f;

        public void Initialize(string characterUID, bool isFirst, float teleportCooldown, bool canTeleportAI, bool canTeleportPlayers, bool canTeleportProjectiles, float lifetime = -1f)
        {
            if (string.IsNullOrEmpty(characterUID))
            {
                Debug.LogError("Portal Initialize: Invalid character UID");
                return;
            }

            ownerUID = characterUID;
            isFirstPortal = isFirst;
            lifeTime = lifetime;
            currentTime = 0f;
            this.teleportCooldown = teleportCooldown;
            this.canTeleportAI = canTeleportAI;
            this.canTeleportPlayers = canTeleportPlayers;
            this.canTeleportProjectiles = canTeleportProjectiles;


            var portalManager = ExtendedEffects.Instance?.PortalManager;
            if (portalManager == null)
            {
                Debug.LogError("Portal Initialize: PortalManager not found");
                return;
            }

            portalManager.RegisterPortalInstance(this, ownerUID, isFirstPortal);

            if (!SetupPortalComponents())
            {
                Debug.LogError("Portal Initialize: Failed to setup portal components");
                return;
            }

            if (!CreateRenderTexture())
            {
                Debug.LogError("Portal Initialize: Failed to create render texture");
                return;
            }

            if (!CreatePortalCamera())
            {
                Debug.LogError("Portal Initialize: Failed to create camera");
                return;
            }

            if (!SetupPortalMaterial())
            {
                Debug.LogError("Portal Initialize: Failed to setup material");
                return;
            }
        }

        private bool SetupPortalComponents()
        {
            Transform renderPortalTransform = transform.Find(PORTAL_RENDER_PATH);
            if (renderPortalTransform == null)
            {
                Debug.LogError($"SetupPortalComponents: Cannot find {PORTAL_RENDER_PATH}");
                return false;
            }

            portalParticles = renderPortalTransform.GetComponent<ParticleSystem>();
            if (portalParticles == null)
            {
                Debug.LogError("SetupPortalComponents: ParticleSystem not found on RenderPortal");
                return false;
            }

            particleRenderer = portalParticles.GetComponent<ParticleSystemRenderer>();
            if (particleRenderer == null)
            {
                Debug.LogError("SetupPortalComponents: ParticleSystemRenderer not found");
                return false;
            }

            return true;
        }

        private bool CreateRenderTexture()
        {
            try
            {
                renderTexture = new RenderTexture(RENDER_TEXTURE_SIZE, RENDER_TEXTURE_SIZE, RENDER_TEXTURE_DEPTH);
                renderTexture.Create();
                return renderTexture != null;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"CreateRenderTexture: Failed to create render texture: {e.Message}");
                return false;
            }
        }

        private bool CreatePortalCamera()
        {
            try
            {
                var cameraObj = new GameObject("PortalCamera");
                if (cameraObj == null) return false;

                cameraObj.transform.parent = transform;
                cameraObj.transform.localPosition = new Vector3(0, 1f, 0);
                cameraObj.transform.localRotation = Quaternion.identity;

                portalCamera = cameraObj.AddComponent<Camera>();
                if (portalCamera == null) return false;

                portalCamera.targetTexture = renderTexture;
                portalCamera.enabled = true;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"CreatePortalCamera: Failed to create camera: {e.Message}");
                return false;
            }
        }

        private bool SetupPortalMaterial()
        {
            try
            {
                if (particleRenderer == null || renderTexture == null) return false;

                portalMaterial = new Material(particleRenderer.material);
                if (portalMaterial == null) return false;

                portalMaterial.mainTexture = renderTexture;
                particleRenderer.sharedMaterial = portalMaterial;  // Changed from material to sharedMaterial
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"SetupPortalMaterial: Failed to setup material: {e.Message}");
                return false;
            }
        }


        //normally I would use an interface for this...
        public void OnTriggerEnter(Collider other)
        {
            if (!canTeleport) return;

            var portalManager = ExtendedEffects.Instance?.PortalManager;
            if (portalManager == null) return;

            PortalInstance targetPortal = isFirstPortal ?
                portalManager.GetSecondPortal(ownerUID) :
                portalManager.GetFirstPortal(ownerUID);


            if (targetPortal == null) return;


            Character character = other.GetComponentInParent<Character>();
            if (character != null)
            {
                HandleCharacterTeleport(character, targetPortal);
                return;
            }

 
            Projectile projectile = other.GetComponentInParent<Projectile>();
            if (projectile != null)
            {
                HandleProjectileTeleport(projectile, targetPortal);
                return;
            }


            PhysicProjectile physicProjectile = other.GetComponentInParent<PhysicProjectile>();
            if (physicProjectile != null)
            {
                HandlePhysicProjectileTeleport(physicProjectile, targetPortal);
                return;
            }
        }

        private void HandleCharacterTeleport(Character character, PortalInstance targetPortal)
        {
            float teleportOffset = character.IsAI ? TELEPORT_OFFSET_AI : TELEPORT_OFFSET_PLAYER;
            Vector3 teleportPosition = targetPortal.transform.position + (targetPortal.transform.forward * teleportOffset);

            DisableTeleport();
            targetPortal.DisableTeleport();
            character.Teleport(teleportPosition, targetPortal.transform.rotation);
        }

        private void HandleProjectileTeleport(Projectile projectile, PortalInstance targetPortal)
        {
            Vector3 teleportPosition = targetPortal.transform.position + (targetPortal.transform.forward * TELEPORT_OFFSET_PROJECTILE);

            DisableTeleport();
            targetPortal.DisableTeleport();
            projectile.transform.position = teleportPosition;
            projectile.transform.rotation = targetPortal.transform.rotation;
        }

        private void HandlePhysicProjectileTeleport(PhysicProjectile physicProjectile, PortalInstance targetPortal)
        {
            Vector3 teleportPosition = targetPortal.transform.position + (targetPortal.transform.forward * TELEPORT_OFFSET_PROJECTILE);

            Rigidbody rb = physicProjectile.m_rigidbody;
            Vector3 velocity = rb.velocity;
            Vector3 angularVelocity = rb.angularVelocity;

            Vector3 newVelocityDirection = targetPortal.transform.rotation * velocity.normalized;
            Vector3 newVelocity = newVelocityDirection * velocity.magnitude;

            DisableTeleport();
            targetPortal.DisableTeleport();

            physicProjectile.transform.position = teleportPosition;
            physicProjectile.transform.rotation = targetPortal.transform.rotation;
            rb.velocity = newVelocity;
            rb.angularVelocity = angularVelocity;
        }

        public void DisableTeleport()
        {
            canTeleport = false;
            cooldownTimer = teleportCooldown;
        }

        private void Update()
        {
            UpdateLifetime();
            UpdateTeleportCooldown();
        }

        private void UpdateLifetime()
        {
            if (lifeTime <= 0) return;

            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
            {
                Cleanup();
            }
        }

        private void UpdateTeleportCooldown()
        {
            if (canTeleport) return;

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canTeleport = true;
            }
        }

        public void SetLifetime(float life)
        {
            if (life < 0)
            {
                Debug.LogWarning("SetLifetime: Negative lifetime specified");
            }
            this.lifeTime = life;
        }
        public void DisableParticleSystems()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in particleSystems)
            {
                if (ps.gameObject.name == "Circle")
                {
                    continue;
                }

                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public void EnableParticleSystems()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in particleSystems)
            {
                if (ps.gameObject.name == "Circle")
                {
                    continue;
                }

                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
        }
        public void Cleanup()
        {
            var portalManager = ExtendedEffects.Instance?.PortalManager;
            if (portalManager != null)
            {
                portalManager.UnregisterPortalInstance(this, ownerUID, isFirstPortal);
            }

            CleanupRenderResources();
            Destroy(gameObject);
        }

        private void CleanupRenderResources()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }

            if (portalCamera != null)
            {
                Destroy(portalCamera.gameObject);
            }

            if (portalMaterial != null)
            {
                Destroy(portalMaterial);
            }
        }
    }
}

